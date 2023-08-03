using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.UI;
using Atlas.Legal.Autorizacion;
using Atlas.Legal.Content;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlas.Legal
{
    public class SiabAppService : LegalAppServiceBase
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IContentRepository _contentRepository;
        private readonly ISiabRepository _siabRepository;

        public SiabAppService(IHttpContextAccessor httpContextAccessor,
            IRepository<OpeUsuario, int> opeUsuarioRepository,
            IObjectMapper objectMapper,
            IContentRepository contentRepository,
            ISiabRepository siabRepository)
            : base(httpContextAccessor, opeUsuarioRepository)
        {
            _objectMapper = objectMapper;
            _contentRepository = contentRepository;
            _siabRepository = siabRepository;
        }

        public async Task<GenerarRequerimientoSiabOutput> GenerarRequerimientoSiab(GenerarRequerimientoSiabInput input)
        {
            //Asignamos usuario
            input.UsuarioRegistro = this.InformacionUsuario.Usuario;

            var result = await _siabRepository.GenerarRequerimientoSiab(input);
            return result;
        }

        public async Task<GuardarActualizarRegistroSiabOutput> GuardarActualizarRegistroSiab(GuardarActualizarRegistroSiabInput input)
        {
            string msj = "";
            if (input.Actualizar == false)
                msj = msj = $"Se creó un nuevo registro '{input.NumeroRegistro}'";
            else
                msj = $"Se actualizo registro '{input.NumeroRegistro}'";

            var result = await _siabRepository.GuardarActualizarRegistroSiab(input);

            if (input.Documentos == null)
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
                input.Actualizar = true; }
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
                        ExpedienteLlave = input.NumeroRegistro,
                        Folder = $"siab/{input.NumeroRegistro}",
                        atributos = mAtributos,
                        NodeType = nodeType,
                        EsGuardado = true
                    };

                    _contentRepository.GuardarDocumento(mGuardarDocumento);
                    input.IdRegExp = mGuardarDocumento.IdRegExp;
                    item.ObjectId = mGuardarDocumento.ObjectId;

                    await _siabRepository.GuardaActualizaDocumentoSiab(new GuardaDocumentoSiabInput
                    {
                        IdDocumento = item.IdDocumento,
                        IdRegistro = result.IdRegistro,
                        Nombre = item.Nombre,
                        ObjectId = item.ObjectId,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = this.InformacionUsuario.Usuario,
                        SubTipoDocumento = item.SubTipoDocumento,
                        TipoMime = item.TipoMime,
                        TipoDocumento = item.TipoDocumento,
                        Borrar = false
                    });
                }
                input.Actualizar = true;
            }
            result = await _siabRepository.GuardarActualizarRegistroSiab(input);

            await _siabRepository.GuardarTrackingSiab(new GuardarTrackingSiabInput
            {
                NumeroSiab = input.NumeroRegistro,
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                Movimiento = msj
            });

            return result;
        }

        public async Task<BusquedaRequerimientoSiabOutput> BusquedaRequerimientoSiab(BusquedaRequerimientoSiabInput input)
        {
            var result = await _siabRepository.BusquedaRequerimientoSiab(input);
            return result;
        }

        public async Task<GuardarSeguimientoRegistroSiabOutput> GuardarSeguimientoRegistroSiab(GuardarSeguimientoRegistroSiabInput input)
        {
            string sMsj = "";
            
            if (input.Concluir)
                sMsj = $"Se cancelo segumiento en registro '{input.NumeroRegistro}'";
            else
                sMsj = $"Se guardó segumiento en registro '{input.NumeroRegistro}'";



            var result = await _siabRepository.GuardarSeguimientoRegistroSiab(input);

            var resp = await _siabRepository.ObtenerIdRegExpSiab(new ObtenerIdRegExpSiabInput
            {
                NumeroRegistro = input.NumeroRegistro
            });

            if (resp.IdRegExp == null)
                throw new UserFriendlyException(400, "No se puede agregar documento");

            if (input.Documentos != null)
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
                        IdRegExp = resp.IdRegExp,
                        IdTipoDocumental = idTipoDoc,
                        Nombre = item.Nombre,
                        Extension = item.TipoMime,
                        Base64 = item.Contenido,
                        ExpedienteLlave = input.NumeroRegistro,
                        Folder = $"siab/{input.NumeroRegistro}",
                        atributos = mAtributos,
                        NodeType = nodeType,
                        EsGuardado = true
                    };

                    _contentRepository.GuardarDocumento(mGuardarDocumento);
                    item.ObjectId = mGuardarDocumento.ObjectId;

                    await _siabRepository.GuardaActualizaDocumentoSiab(new GuardaDocumentoSiabInput
                    {
                        IdDocumento = item.IdDocumento,
                        IdRegistro = resp.IdRegistro,
                        Nombre = item.Nombre,
                        ObjectId = item.ObjectId,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = this.InformacionUsuario.Usuario,
                        SubTipoDocumento = item.SubTipoDocumento,
                        TipoMime = item.TipoMime,
                        TipoDocumento = item.TipoDocumento,
                        Borrar = false
                    });
                }
            }

            await _siabRepository.GuardarTrackingSiab(new GuardarTrackingSiabInput
            {
                NumeroSiab = input.NumeroRegistro,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                FechaRegistro = DateTime.Now,
                Movimiento = sMsj
            });            

            return result;
        }

        public async Task<ReporteRequerimientoSiabOutput> ReporteRequerimientoSiab(ReporteRequerimientoSiabInput input)
        {
            var result = await _siabRepository.ReporteRequerimientoSiab(input);
            return result;
        }

        public async Task<ObtenerDetalleRequerimientoSiabOutput> ObtenerDetalleRequerimientoSiab(ObtenerDetalleRequerimientoSiabInput input)
        {
            var result = await _siabRepository.ObtenerDetalleRequerimientoSiab(input);

            return result;
        }

        public async Task<GuardarComentarioSiabOutput> GuardarComentarioSiab(GuardarComentarioSiabInput input)
        {
            await _siabRepository.GuardarComentarioSiab(new GuardarComentarioSiabInput
            {
                NumeroRegistro = input.NumeroRegistro,
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                Comentario = input.Comentario
            });

            await _siabRepository.GuardarTrackingSiab(new GuardarTrackingSiabInput
            {
                NumeroSiab = input.NumeroRegistro,
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                Movimiento = $"Se agregó comentario a registro '{input.NumeroRegistro}'"
            });

            return new GuardarComentarioSiabOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public async Task<GuardaDocumentoSiabOutput> GuardaActualizaDocumentoSiab(GuardaDocumentoSiabInput input)
        {
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

            if (input.Borrar)
            {
                var resp = await _siabRepository.ObtenerObjectIdSiab(new ObtenerObjectIdSiabInput { IdDocumento = (int)input.IdDocumento });
                input.ObjectId = resp.ObjectId;
                _contentRepository.EliminarDocumento(input.ObjectId);
                await _siabRepository.GuardaActualizaDocumentoSiab(input);
            }
            else
            {
                if (input.IdDocumento != null)
                {
                    var resp = await _siabRepository.ObtenerObjectIdSiab(new ObtenerObjectIdSiabInput { IdDocumento = (int)input.IdDocumento });
                    input.ObjectId = resp.ObjectId;
                    _contentRepository.EliminarDocumento(input.ObjectId);
                }

                var mConsulta = await _siabRepository.ObtenerIdRegExpSiab(new ObtenerIdRegExpSiabInput
                {
                    IdRegistro = input.IdRegistro
                });

                var mGuardarDocumento = new GuardarDocumentoInput
                {
                    IdRegExp = mConsulta.IdRegExp,
                    IdTipoDocumental = idTipoDoc,
                    Nombre = input.Nombre,
                    Extension = input.TipoMime,
                    Base64 = input.Contenido,
                    ExpedienteLlave = mConsulta.NumeroRegistro,
                    Folder = $"siab/{mConsulta.NumeroRegistro}",
                    atributos = mAtributos,
                    NodeType = nodeType,
                    EsGuardado = true
                };
                _contentRepository.GuardarDocumento(mGuardarDocumento);
                input.ObjectId = mGuardarDocumento.ObjectId;
                input.FechaRegistro = DateTime.Now;

                await _siabRepository.GuardaActualizaDocumentoSiab(input);

            }

            return new GuardaDocumentoSiabOutput
            {
                Mensaje = "Operacion exitosa"
            };
        }

        public async Task<ObtenerTrackingsSiabOutput> ObtenerTrackingsSiab(ObtenerTrackingsSiabInput input)
        {
            var result = await _siabRepository.ObtenerTrackingsSiab(input);

            result.Trackings = result.Trackings.Where(s => s.NumeroRegistro == input.NumeroRegistro).ToList();

            return result;
        }

        public async Task<ObtenerComentariosSiabOutput> ObtenerComentariosSiab(ObtenerComentariosSiabInput input)
        {
            var result = await _siabRepository.ObtenerComentariosSiab(input);

            result.Comentarios = result.Comentarios.Where(s => s.NumeroRegistro == input.NumeroRegistro).ToList();

            return result;
        }

        public async Task<ObtenerDocumentoSiabOutput> ObtenerDocumentoSiab(ObtenerDocumentoSiabInput input)
        {
            var result = await _siabRepository.ObtenerDocumentoSiab(input);
            _contentRepository.ObtenerDocumento(result.ObjectId, out string contenido);

            result.Contenido = contenido;

            return result;

        }

        public async Task<ValidacionSiabOutput> ValidacionSiab()
        {
            ValidacionSiabInput input = new ValidacionSiabInput();
            //Asignamos usuario
            input.Usuario = this.InformacionUsuario.Usuario;
            input.FechaRegistro = DateTime.Now;

            var result = await _siabRepository.ValidacionSiab(input);
            return result;
        }

    }
}
