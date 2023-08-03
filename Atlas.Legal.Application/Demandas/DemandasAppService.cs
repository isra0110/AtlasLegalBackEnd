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
using static Atlas.Legal.AtlasWS.AtlasWSOutput;
using Atlas.Legal.AtlasWS;

namespace Atlas.Legal
{
    public class DemandasAppService : LegalAppServiceBase, IDemandasAppService
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IDemandasRepository _demandasRepository;
        private readonly IRepository<OpeDemanda, int> _opeDemandaRepository;
        private readonly IContentRepository _contentRepository;
        private readonly IAutorizacionRepository _autorizacionRepository;
        private readonly IAtlasWSRepository _atlasRepository;


        public DemandasAppService(
            IObjectMapper objectMapper,
            IDemandasRepository demandasRepository,
            IRepository<OpeDemanda, int> opeDemandaRepository,
            IAutorizacionRepository autorizacionRepository,
            IHttpContextAccessor httpContextAccessor,
            IAtlasWSRepository atlasWSRepository,
            IRepository<OpeUsuario, int> opeUsuarioRepository,
            IContentRepository contentRepository) : base(httpContextAccessor, opeUsuarioRepository)
        {
            _objectMapper = objectMapper;
            _opeDemandaRepository = opeDemandaRepository;
            _contentRepository = contentRepository;
            _demandasRepository = demandasRepository;
            _autorizacionRepository = autorizacionRepository;
            _atlasRepository = atlasWSRepository;
        }

        public GuardarActualizarRegistroDemandaOutput GuardarActualizarRegistroDemanda(GuardarActualizarRegistroDemandaInput input)
        {
            string msj = "";
            if (input.IdDemanda != null)
                msj = "Se realizó registro de demanda";
            else
                msj = "Se actualizó la demanda";

            if (input.SiniestroSeleccionado.FechaPagoHonorarios.ToString() == "null")
                input.SiniestroSeleccionado.FechaPagoHonorarios = null;

            var result = _demandasRepository.GuardarActualizarRegistroDemanda(input);

            if (input.IdRegExp == null)
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

            result = _demandasRepository.GuardarActualizarRegistroDemanda(input);

            

            _demandasRepository.GuardarTracking(result.NumDemanda, this.InformacionUsuario.Usuario, DateTime.Now, msj);


            return result;
        }

        public EliminarAsignacionOutput EliminarAsignacion(EliminarAsignacionInput input)
        {
            var result = _demandasRepository.EliminarAsignacion(input);            

            _demandasRepository.GuardarTracking(input.IdDemanda, this.InformacionUsuario.Usuario, DateTime.Now, "Se eliminó asignacion");

            return result;
        }

        public AsignarActualizarAbogadoOutput AsignarActualizarAbogado(AsignarActualizarAbogadoInput input)
        {
            char cEspacio = '\t';

            var result = _demandasRepository.AsignarActualizarAbogado(input);
            string msj = "";

            if (input.IdAsigna != null)
                msj = "Se realizó asignación de abogado";
            else
                msj = "Se actualizó la asignación de abogado";

            //Envio de notificacion por correo
            //NotificacionesInput notificaciones = new NotificacionesInput();
            //notificaciones.para = "emanuel.hernandez@ies-systems.com";
            //notificaciones.cc = null;
            //notificaciones.cco = null;
            //notificaciones.asunto = "Se\\tasigno\\tel\\tsiguiente\\tlitigio\\tpara\\tsu\\tseguimiento";
            //notificaciones.mensaje = "Por\\tmedio\\tde\\tla\\tpresente\\tNotificación\\tse\\thace\\tformal\\tla\\tasignación\\tdel\\tjuicio\\,\\tmas\\tadelante\\tdescrito\\tpara\\tque\\tel\\tmismo\\tsea\\tatendido\\thasta\\tsu\\ttotal\\ty\\tdefinida\\tconclusión\\ten\\ttodas\\ty\\tcada\\tuna\\tde\\tsus\\tinstancias.<br><br><br>" +
            //                         "Datos:<br><br><br>" +
            //                         "Materia:" + input.DetalleRegistro.Materia.Replace(' ', cEspacio) +
            //                         "Especifique\\tMateria:" + input.DetalleRegistro.EspecifiqueMateria.Replace(' ', cEspacio) +
            //                         "Actor:" + input.DetalleRegistro.Actor.Replace(' ', cEspacio) +
            //                         "Notificados\\ten\\tcalidad\\tde:" + input.DetalleRegistro.NotificacionCalidad.Replace(' ', cEspacio) +
            //                         "Expediente\\tdel\\tjuicio:" + input.DetalleRegistro.ExpedienteJuicio.Replace(' ', cEspacio) +
            //                         "Codemandados:" + input.DetalleRegistro.Codemandados.Replace(' ', cEspacio) +
            //                         "Términos\\tde\\tla\\t\\contestación:" + input.DetalleRegistro.TerminosContestacion +
            //                         "Fecha\\tde\\tvencimiento:" + input.DetalleRegistro.FechaVencimientoContestacion.ToString().Replace(' ', cEspacio) +
            //                         "Autoridad\\tjudicial:" + input.DetalleRegistro.Autoridad.Replace(' ', cEspacio) +
            //                         "Tipo\\tde\\tjuicio:" + input.DetalleRegistro.TipoJuicio.Replace(' ', cEspacio) +
            //                         "Motivo\\tdemanda:"+ input.DetalleRegistro.MotivoDemanda.Replace(' ', cEspacio);

            //notificaciones.archivos = new List<ArchivosInput>();

            //var resNotificacion = _atlasRepository.EnviaNotificacion(notificaciones);

            _demandasRepository.GuardarTracking(input.IdDemanda, this.InformacionUsuario.Usuario, DateTime.Now, msj);

            return result;
        }

        public ConsultaRegistroDemandaOutput ConsultaRegistroDemanda(ConsultarRegistroDemandaInput input)
        {
            var result = _demandasRepository.ConsultaRegistroDemanda(input);
            return result;
        }

        public async Task<GuardarComentarioOutput> GuardarComentario(GuardarComentarioInput input)
        {
            _demandasRepository.GuardarComentario(input);

            return new GuardarComentarioOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public ConsultaDemandaTrackingOutput ConsultaDemandaTracking(ConsultaDemandaTrackingInput input)
        {
            var result = _demandasRepository.ConsultaDemandaTracking(input);

            return result;
        }

        public ConsultaDemandaComentarioOutput ConsultaDemandaComentario(ConsultaDemandaComentarioInput input)
        {
            var result = _demandasRepository.ConsultaDemandaComentario(input);
            return result;
        }

        public ObtenerDetalleDemandaOutput ObtenerDetalleDemanda(ObtenerDetalleDemandaInput input)
        {
            var result = _demandasRepository.ObtenerDetalleDemanda(input);

            return result;
        }

        public ConsultaReporteJuiciosOutput ConsultaReporteJuicios(ConsultaReporteJuiciosInput input)
        {
            var result = _demandasRepository.ConsultaReporteJuicios(input);
            return result;
        }

        public ConsultaReporteJuiciosRelevantesOutput ConsultaReporteJuiciosRelevantes(ConsultaReporteJuiciosRelevantesInput input)
        {
            var result = _demandasRepository.ConsultaReporteJuiciosRelevantes(input);
            return result;
        }

        public ConsultaReporteJuiciosEnContraAseguradosOutput ConsultaReporteJuiciosEnContraAsegurados(ConsultaReporteJuiciosEnContraAseguradosInput input)
        {
            var result = _demandasRepository.ConsultaReporteJuiciosEnContraAsegurados(input);
            return result;
        }

        public ConsultaReporteConcluidosOutput ConsultaReporteConcluidos(ConsultaReporteConcluidosInput input)
        {
            var result = _demandasRepository.ConsultaReporteConcluidos(input);
            return result;
        }

        public async Task<AgregarActualizarDocumentoLitigioOutput> AgregarActualizarDocumentoLitigio(AgregarActualizarDocumentoLitigioInput input)
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


            if (input.Borrar && this.InformacionUsuario.IdRol != 1)
                throw new UserFriendlyException("Usted no cuenta con permisos para eliminar el documento");

            if (input.Borrar == true && this.InformacionUsuario.IdRol == 1)
            {
                var mObjectId = _demandasRepository.ObtenerObjectId(new ObtenerObjectIdInput { IdDocumento = (int)input.IdDocumento });
                input.ObjectId = mObjectId.ObjectId;
                _contentRepository.EliminarDocumento(input.ObjectId);
                _demandasRepository.AgregarActualizarDocumentoLitigio(input);
            }
            else
            {
                if (input.IdDocumento != null)
                {
                    var objectId = _demandasRepository.ObtenerObjectId(new ObtenerObjectIdInput { IdDocumento = (int)input.IdDocumento });
                    _contentRepository.EliminarDocumento(objectId.ObjectId);
                }                

                var mConsulta = _demandasRepository.ObtenerRegExp(new ObtenerRegExpInput
                {
                    IdDemanda = input.IdDemanda
                });

                var mGuardarDocumento = new GuardarDocumentoInput
                {
                    IdRegExp = mConsulta.IdRegExp,
                    IdTipoDocumental = idTipoDoc,
                    Nombre = input.Nombre,
                    Extension = input.TipoMime,
                    Base64 = input.Contenido,
                    ExpedienteLlave = mConsulta.NumDemanda,
                    Folder = $"litigio/{mConsulta.NumDemanda}",
                    NodeType = nodeType,
                    atributos = mAtributos,
                    EsGuardado = true
                };
                _contentRepository.GuardarDocumento(mGuardarDocumento);
                input.ObjectId = mGuardarDocumento.ObjectId;
                
                _demandasRepository.AgregarActualizarDocumentoLitigio(input);

                //Envio de notificacion por correo
                NotificacionesInput notificaciones = new NotificacionesInput();
                notificaciones.para = "miguel.herrera@ies-systems.com";
                notificaciones.cc = null;
                notificaciones.cco = null;
                notificaciones.asunto = "Carga\\tde\\tDocumento";
                notificaciones.mensaje = "La\\tpresente\\tNotificación\\tes\\tpara\\tindicar\\tque\\tel\\tproveedor\\t" + input.UsuarioRegistro.Replace(' ', cEspacio) + "\\tcargo\\tel\\tdocumento\\t" + input.TipoDocumento.Replace(' ', cEspacio) + ".<br><br><br>" +
                                         "Datos:<br><br><br>" +
                                         "Materia:" + input.DetalleRegistro.Materia.Replace(' ', cEspacio) +
                                         "Especifique\\tMateria:" + input.DetalleRegistro.EspecifiqueMateria.Replace(' ', cEspacio) +
                                         "Actor:" + input.DetalleRegistro.Actor.Replace(' ', cEspacio) +
                                         "Notificados\\ten\\tcalidad\\tde:" + input.DetalleRegistro.NotificacionCalidad.Replace(' ', cEspacio) +
                                         "Expediente\\tdel\\tjuicio:" + input.DetalleRegistro.ExpedienteJuicio.Replace(' ', cEspacio) +
                                         "Codemandados:" + input.DetalleRegistro.Codemandados.Replace(' ', cEspacio) +
                                         "Términos\\tde\\tla\\t\\contestación:" + input.DetalleRegistro.TerminosContestacion +
                                         "Fecha\\tde\\tvencimiento:" + input.DetalleRegistro.FechaVencimientoContestacion.ToString().Replace(' ', cEspacio) +
                                         "Autoridad\\tjudicial:" + input.DetalleRegistro.Autoridad.Replace(' ', cEspacio) +
                                         "Tipo\\tde\\tjuicio:" + input.DetalleRegistro.TipoJuicio.Replace(' ', cEspacio) +
                                         "Motivo\\tdemanda:" + input.DetalleRegistro.MotivoDemanda.Replace(' ', cEspacio);
                
                notificaciones.archivos = new List<ArchivosInput>();

                var resNotificacion = _atlasRepository.EnviaNotificacion(notificaciones);
            }

            return new AgregarActualizarDocumentoLitigioOutput
            {
                Mensaje = "Operacion Exitosa"
            };
        }

        public ObtenerDocumentoLitigioOuput ObtenerDocumentoLitigio(ObtenerDocumentoLitigioInput input)
        {
            var result = _demandasRepository.ObtenerDocumentoLitigio(input);
            _contentRepository.ObtenerDocumento(result.ObjectId, out string contenido);

            result.Contenido = contenido;

            return result;
        }

        public async Task<ConcluirDemandaLitigioOutput> ConcluirDemandaLitigio(ConcluirDemandaLitigioInput input)
        {
            await _demandasRepository.ConcluirDemandaLitigio(input);

            return new ConcluirDemandaLitigioOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public async Task<LitigioAtlasOutput> BuscarSiniestroLitigio(LitigioAtlasInput input)
        {
            var result = await _atlasRepository.ConsultaLitigio(input);

            return result;
        }

    }
}
