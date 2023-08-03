    using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Atlas.Legal.Content;
using Atlas.Legal.Autorizacion;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Atlas.Legal.AtlasWS;
using static Atlas.Legal.AtlasWS.AtlasWSOutput;

namespace Atlas.Legal
{
    public class RecuperacionesAppService : LegalAppServiceBase
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IContentRepository _contentRepository;
        private readonly IRecuperacionesRepository _recuperacionesRepository;

        private readonly IAtlasWSRepository _atlasRepository;

        public RecuperacionesAppService(
            IHttpContextAccessor httpContextAccessor,
            IRepository<OpeUsuario, int> opeUsuarioRepository,
            IObjectMapper objectMapper,
            IContentRepository contentRepository,
            IAtlasWSRepository atlasWSRepository,
            IRecuperacionesRepository recuperacionesRepository
            ) : base(httpContextAccessor, opeUsuarioRepository)
        {
            _objectMapper = objectMapper;
            _contentRepository = contentRepository;
            _recuperacionesRepository = recuperacionesRepository;
            _atlasRepository = atlasWSRepository;
        }

        public async Task<BuscarSiniestroRecuperacionesOutput> BuscarSiniestroRecuperaciones(BuscarSiniestroRecuperacionesInput input)
        {
            var result = await _recuperacionesRepository.BusquedaRecuperacionSiniestroTransporte(input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            return result;
        }

        public async Task<RegistroDemandaRecuperacionesOutput> RegistroDemandaRecuperaciones(RegistroDemandaRecuperacionesInput input)
        {
            string msj = "";
            if (input.NumDemanda != null)
                msj = "Se actualizó registro";
            else
                msj = "Se creó un nuevo registro";

            var result = await _recuperacionesRepository.RegistroDemandaRecuperaciones(input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            
            input.NumDemanda = result.NumeroDemanda;

            if ((input.Documentos == null))
            {
                if (input.IdRegExp == null || !input.IdRegExp.HasValue)
                {
                    var mCrearExpediente = new GuardarDocumentoInput
                    {
                        IdRegExp = input.IdRegExp,
                        IdTipoExpediente = 178,
                        EsGuardado = false
                    };

                    _contentRepository.GuardarDocumento(mCrearExpediente);
                    input.IdRegExp = mCrearExpediente.IdRegExp;
                }
            }
            else
            {                
                foreach (var item in input.Documentos)
                {
                    var mAtributos = new Dictionary<string, string>();
                    int idTipoDoc = 0;
                    string nodeType = string.Empty;

#if (DEBUG || TEST)
                    mAtributos.Add("AsistenciaLegal", item.SubTipoDocumento);
                    idTipoDoc = 184;
                    nodeType = "AtlasLegal_Desarrollo";
#else
                        idTipoDoc = 1;
                        mAtributos.Add("AsistenciaLegal", item.SubTipoDocumento);
                        nodeType = "AtlasLegal_AsistenciaLegal";
#endif
                    var mGuardarDocumento = new GuardarDocumentoInput
                    {
                        IdRegExp = input.IdRegExp,
                        IdTipoDocumental = idTipoDoc,
                        Nombre = item.Nombre,
                        Extension = item.TipoMime,
                        Base64 = item.Contenido,
                        ExpedienteLlave = input.NumDemanda,
                        Folder = $"recuperaciones/{input.NumDemanda}",
                        atributos = mAtributos,
                        NodeType = nodeType,
                        EsGuardado = true
                    };
                    _contentRepository.GuardarDocumento(mGuardarDocumento);

                    input.IdRegExp = mGuardarDocumento.IdRegExp;
                    item.ObjectId = mGuardarDocumento.ObjectId;                    

                    var resultDocumento = await _recuperacionesRepository.RegistroDemandaRecuperacionesDocumento(new RegistroDemandaRecuperacionesDocumentoInput
                    {
                        IdDocumento = item.IdDocumento,
                        IdDemanda = result.Id,
                        Nombre = item.Nombre,
                        Objeto = item.ObjectId,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = this.InformacionUsuario.Usuario,
                        SubTipoDocumento = item.SubTipoDocumento,
                        TipoMime = item.TipoMime,
                        TipoDocumento = item.TipoDocumento,
                        Borrar = false
                    });
                    if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                        throw new UserFriendlyException(400, result.Mensaje);
                }
            }
            result = await _recuperacionesRepository.RegistroDemandaRecuperaciones(input);
            _recuperacionesRepository.GuardaTrackingRecuperaciones(new GuardaTrackingRecuperacionesInput
            {
                NumDemanda = input.NumDemanda,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                FechaRegistro = DateTime.Now,
                Movimiento = msj
            });

            return result;
        }

        public async Task<AsignarAbogadoRecuperacionesOutput> AsignarAbogadoRecuperaciones(AsignarAbogadoRecuperacionesInput input)
        {
            char cEspacio = '\t';
            var result = new AsignarAbogadoRecuperacionesOutput();
            foreach (var item in input.AsignarAbogado)
            {
                item.FechaCreacion = DateTime.Now;
                result = await _recuperacionesRepository.AsignarAbogadoRecuperaciones(item);
                if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                    throw new UserFriendlyException(400, result.Mensaje);

                //Envio de notificacion por correo
                NotificacionesInput notificaciones = new NotificacionesInput();
                notificaciones.para = "miguel.herrera@ies-systems.com";
                notificaciones.cc = null;
                notificaciones.cco = null;
                notificaciones.asunto = "Asignación\\tde\\tRecuperación\\tSiniestro\\t" + item.NumeroDemanda.Replace(' ', cEspacio); // + " - " + item.Asegurado.Replace(' ', cEspacio) + " - " + item.Ramo.Replace(' ', cEspacio);
                notificaciones.mensaje = "Sirva\\tel\\tpresente\\tpara\\tencontrar\\tasignación\\tdel\\tsiniestro\\ten\\treferencia\\,\\tpara\\tque\\tse\\tinicien\\tlas\\tgestiones\\tde\\trecuperación\\ten\\tcontra\\tdel\\tresponsable\\,\\tAgradeceré\\tconsultar\\tla\\tsiguiente\\tliga,\\tpara\\tdescargar\\tlos\\tdocumentos\\tdel\\tsiniestro.<br>";

                notificaciones.archivos = new List<ArchivosInput>();

                var resNotificacion = _atlasRepository.EnviaNotificacion(notificaciones);
            }            

            //Guardar Tracking
            _recuperacionesRepository.GuardaTrackingRecuperaciones(new GuardaTrackingRecuperacionesInput
            {
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                NumDemanda = input.AsignarAbogado.Select(s => s.NumeroDemanda).FirstOrDefault(),
                Movimiento = "Se asignó abogado"
            });
            return result;
        }

        public async Task<DetalleRecuperacionOutput> DetalleRecuperacion(DetalleRecuperacionInput input)
        {
            var result = await _recuperacionesRepository.DetalleRecuperacion(input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            return result;
        }

        public async Task<ConsultaRecuperacionOutput> ConsultaRecuperacion(ConsultaRecuperacionInput input)
        {
            var result = await _recuperacionesRepository.ConsultaRecuperacion(input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            return result;
        }

        public async Task<ConcluirRecuperacionOutput> ConcluirRecuperacion(ConcluirRecuperacionInput input)
        {
            var result = await _recuperacionesRepository.ConcluirRecuperacion(input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            
            _recuperacionesRepository.GuardaTrackingRecuperaciones(new GuardaTrackingRecuperacionesInput
            {
                NumDemanda = input.IdDemanda,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                FechaRegistro = DateTime.Now,
                Movimiento = "Se concluyó recuperación"
            });

            return result;
        }

        public async Task<AperturarRechazarSiniestroRecuperacionesOut> AperturarRechazarSiniestroRecuperaciones(AperturarRechazarSiniestroRecuperacionesInput input)
        {
            string msj = "";

            if (input.AperturarRechazar)
                msj = "Se aperturó siniestro recuperaciones";
            else
                msj = "Se rechazó siniestro recuperaciones";

            var result = await _recuperacionesRepository.AperturarRechazarSiniestroRecuperacion(input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);

            //_recuperacionesRepository.GuardaTrackingRecuperaciones(new GuardaTrackingRecuperacionesInput
            //{

            //});

            return result;
        }

        public async Task<ObtenerReporteOut> ReporteRecuperacionTransporte(ReporteRecuperacionTransporteInput input)
        {
            var result = await _recuperacionesRepository.ObtenerReporte("spReporteRecuperacionTransporte", input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            return result;
        }

        public async Task<ObtenerReporteOut> ReporteRecuperacionAutos(ReporteRecuperacionTransporteInput input)
        {
            var result = await _recuperacionesRepository.ObtenerReporte("spReporteRecuperacionAutos", input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            return result;
        }

        public async Task<ObtenerReporteOut> ReporteRecuperacionDiversos(ReporteRecuperacionTransporteInput input)
        {
            var result = await _recuperacionesRepository.ObtenerReporte("spReporteRecuperacionDiversos", input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            return result;
        }

        public async Task<RegistroDocumentoRecuperacionesOutput> GuardaActualizaDocumentoRecuperaciones(RegistroDemandaRecuperacionesDocumentoInput input)
        {
            char cEspacio = '\t';
            input.UsuarioRegistro = this.InformacionUsuario.Usuario;
            var mAtributos = new Dictionary<string, string>();
            int idTipoDoc = 0;
            string nodeType = string.Empty;

#if (DEBUG || TEST)
            mAtributos.Add("AsistenciaLegal", input.SubTipoDocumento);
            idTipoDoc = 184;
            nodeType = "AtlasLegal_Desarrollo";
#else
                        idTipoDoc = 1;
                        mAtributos.Add("AsistenciaLegal", input.SubTipoDocumento);
                        nodeType = "AtlasLegal_AsistenciaLegal";
#endif

            if (input.Borrar == true)
            {
                string objectId = _recuperacionesRepository.ObtenerObjectIdRecuperaciones(new ObtenerObjectIdRecuperacionesInput { IdDocumento = (int)input.IdDocumento });
                input.Objeto = objectId;
                _contentRepository.EliminarDocumento(input.Objeto);
                await _recuperacionesRepository.RegistroDemandaRecuperacionesDocumento(input);
            }
            else
            {
                if (input.IdDocumento != null)
                {
                    string objectId = _recuperacionesRepository.ObtenerObjectIdRecuperaciones(new ObtenerObjectIdRecuperacionesInput { IdDocumento = (int)input.IdDocumento });
                    _contentRepository.EliminarDocumento(objectId);
                }

                var mConsulta = _recuperacionesRepository.ObtenerIdRegExp((int)input.IdDemanda);

                var mGuardarDocumento = new GuardarDocumentoInput
                {
                    IdRegExp = mConsulta.IdRegExp,
                    IdTipoDocumental = idTipoDoc,
                    Nombre = input.Nombre,
                    Extension = input.TipoMime,
                    Base64 = input.Contenido,
                    ExpedienteLlave = mConsulta.NumDemanda,
                    Folder = $"recuperaciones/{mConsulta.NumDemanda}",
                    atributos = mAtributos,
                    NodeType = nodeType,
                    EsGuardado = true
                };
                _contentRepository.GuardarDocumento(mGuardarDocumento);
                input.Objeto = mGuardarDocumento.ObjectId;
                input.FechaRegistro = DateTime.Now;
                await _recuperacionesRepository.RegistroDemandaRecuperacionesDocumento(input);

                //Envio de notificacion por correo
                //NotificacionesInput notificaciones = new NotificacionesInput();
                //notificaciones.para = "miguel.herrera@ies-systems.com";
                //notificaciones.cc = null;
                //notificaciones.cco = null;
                //notificaciones.asunto = "Asignación\\tde\\tRecuperación\\tSiniestro\\t" + input.IdDemanda.ToString().Replace(' ', cEspacio) + " - " + input.Asegurado.Replace(' ', cEspacio) + " - " + input.Ramo.Replace(' ', cEspacio);
                //notificaciones.mensaje = "Sirva\\tel\\tpresente\\tpara\\tencontrar\\tasignación\\tdel\\tsiniestro\\ten\\treferencia\\,\\tpara\\tque\\tse\\tinicien\\tlas\\tgestiones\\tde\\trecuperación\\ten\\tcontra\\tdel\\tresponsable\\,\\tAgradeceré\\tconsultar\\tla\\tsiguiente\\tliga,\\tpara\\tdescargar\\tlos\\tdocumentos\\tdel\\tsiniestro.<br>";

                //notificaciones.archivos = new List<ArchivosInput>();

                //var resNotificacion = _atlasRepository.EnviaNotificacion(notificaciones);
            }

            return new RegistroDocumentoRecuperacionesOutput
            {
                Mensaje = "Operacion Exitosa"
            };
        }

        public async Task<ObtenerDocumentoRecuperacionesOutput> ObtenerDocumentoRecuperaciones(ObtenerDocumentoRecuperacionesInput input)
        {
            var result = await _recuperacionesRepository.ObtenerDocumentoRecuperaciones(input);
            _contentRepository.ObtenerDocumento(result.ObjectId, out string contenido);

            result.Contenido = contenido;
            return result;
        }

        public async Task<ObtenerTrackingsRecuperacionesOutput> ObtenerTrackingsRecuperaciones(ObtenerTrackingsRecuperacionesInput input)
        {
            var result = await _recuperacionesRepository.ObtenerTrackingsRecuperaciones(input);
            return result;
        }

        public async Task<GuardarComentarioRecuperacionesOutput> GuardarComentarioRecuperaciones(GuardarComentarioRecuperacionesInput input)
        {
            _recuperacionesRepository.GuardarComentarioRecuperaciones(new GuardarComentarioRecuperacionesInput
            {
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                Comentario = input.Comentario,
                NumDemanda = input.NumDemanda
            }) ;

            return new GuardarComentarioRecuperacionesOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public async Task<ObtenerComentariosRecuperacionesOutput> ObtenerComentariosRecuperaciones(ObtenerComentariosRecuperacionesInput input)
        {
            var result = await _recuperacionesRepository.ObtenerComentariosRecuperaciones(input);

            return result;
        }

        public async Task<RecuperacionesSiniestroOutput> BuscarSiniestroRecuperacionesSisa(RecuperacionesSiniestroInput input)
        {
            var result = await _atlasRepository.ConsultaRecuperaciones(input);

            return result;
        }

    }
}
