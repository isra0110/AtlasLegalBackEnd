using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.UI;
using Atlas.Legal.AtlasWS;
using Atlas.Legal.Autorizacion;
using Atlas.Legal.Comun;
using Atlas.Legal.Configuration;
using Atlas.Legal.Content;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Atlas.Legal.AtlasWS.AtlasWSOutput;

namespace Atlas.Legal
{
    public class OficiosAppService : LegalAppServiceBase
    {
        private const string URL_OFICIOS = "multas/consulta";
        private readonly IObjectMapper _objectMapper;
        private readonly IContentRepository _contentRepository;
        private readonly IOficiosRepository _oficiosRepository;
        private readonly IAtlasWSRepository _atlasRepository;
        private readonly IRepository<CatAbogadoInterno, int> _catAbogadoInternoRespository;
        private readonly IRepository<CatPlantillaCorreo, int> _catPlantillaCorreoRepository;
        private readonly IRepository<CatProveedor, int> _catProveedorRepository;
        private readonly IConfigurationRoot _config;

        public OficiosAppService(
            IHttpContextAccessor httpContextAccessor,
            IRepository<OpeUsuario, int> opeUsuarioRepository,
            IObjectMapper objectMapper,
            IContentRepository contentRepository,
            IRepository<CatAbogadoInterno, int> catAbogadoInternoRespository,
            IRepository<CatPlantillaCorreo, int> catPlantillaCorreoRepository,
            IRepository<CatProveedor, int> catProveedorRepository,
            IHostingEnvironment env,
            IOficiosRepository oficiosRepository,
            IAtlasWSRepository atlasWSRepository
            ) : base(httpContextAccessor, opeUsuarioRepository)
        {
            _objectMapper = objectMapper;
            _contentRepository = contentRepository;
            _oficiosRepository = oficiosRepository;
            _atlasRepository = atlasWSRepository;
            _catAbogadoInternoRespository = catAbogadoInternoRespository;
            _catPlantillaCorreoRepository = catPlantillaCorreoRepository;
            _catProveedorRepository = catProveedorRepository;
            _config = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public async Task<RegistraOficioOutput> RegistraOficio(RegistraOficioInput input)
        {
            string msj = "";
            if (input.NumeroOficio != null)
                msj = "Se actualizó registro";
            else
                msj = "Se creó un nuevo registro";

            var result = await _oficiosRepository.RegistraOficio(input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);

            input.NumeroOficio = result.NumeroOficio;
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
                        ExpedienteLlave = input.NumeroOficio,
                        Folder = $"oficios/{input.NumeroOficio}",
                        atributos = mAtributos,
                        NodeType = nodeType,
                        EsGuardado = true
                    };
                    _contentRepository.GuardarDocumento(mGuardarDocumento);
                    input.IdRegExp = mGuardarDocumento.IdRegExp;
                    item.ObjectId = mGuardarDocumento.ObjectId;

                    var resultDocumento = await _oficiosRepository.RegistraOficioDocumento(new RegistraOficioDocumentoInput
                    {
                        IdDocumento = item.IdDocumento,
                        IdOficio = result.IdOficio,
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
            result = await _oficiosRepository.RegistraOficio(input);
            _oficiosRepository.GuardaTrackingOficio(new GuardaTrackingOficioInput
            {
                NumDemanda = input.NumeroOficio,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                FechaRegistro = DateTime.Now,
                Movimiento = msj
            });
            return result;
        }

        public async Task<AsignarAbogadoOficiosOutput> AsignarAbogadoOficios(AsignarAbogadoOficiosInput input)
        {
            char cEspacio = '\t';
            var result = new AsignarAbogadoOficiosOutput();
            foreach (var item in input.AsignarAbogado)
            {
                item.FechaCreacion = DateTime.Now;
                result = await _oficiosRepository.AsignarAbogadoOficios(item);
                if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                    throw new UserFriendlyException(400, result.Mensaje);

                var mPlantilla = await _catPlantillaCorreoRepository.FirstOrDefaultAsync(s => s.Nombre.Equals("Oficios_Asignacion"));
                if (mPlantilla != null)
                {
                    var mArchivos = new List<ArchivosInput>();

                    var output = await _oficiosRepository.ConsultaOficio(new ConsultaOficioInput());

                    var mRegistro = output.Detalles.FirstOrDefault(s => !string.IsNullOrEmpty(s.NumOficio) && s.NumOficio.Equals(item.NumeroOficio));

                    var mSolicitud = _objectMapper.Map<PlantillaOficio>(mRegistro);
                    mSolicitud.AppUrl = $"{_config["EndPoints:BaseUrlAtlasLegal"]}{URL_OFICIOS}";

                    var mCuerpo = PlantillaManager.LlenarPlantilla(mPlantilla.Plantilla, mSolicitud);

                    var caracteres = mCuerpo.Split(' ');
                    var mCuerpoNuevo = string.Join("\\t", caracteres);

                    string mAsunto = $"{mPlantilla.Asunto} {item.NumeroOficio}";

                    caracteres = mAsunto.Split(' ');
                    var mAsuntoNuevo = string.Join("\\t", caracteres);


                    if (item.EsInterno.HasValue && item.EsInterno.Value)
                    {
                        var mAbogado = await _catAbogadoInternoRespository.GetAsync((int)item.IdAbogadoInterno);

                        caracteres = mAbogado.Correo.Split(' ');
                        string mCorreoNuevo = string.Join("\\t", caracteres);

                        var resultCorreo = _atlasRepository.EnviaNotificacion(new NotificacionesInput
                        {
                            asunto = mAsuntoNuevo,
                            para = mCorreoNuevo,
                            mensaje = mCuerpoNuevo,
                            numeroReporte = item.NumeroOficio,
                            archivos = mArchivos
                        });
                    }
                    else if(item.EsInterno.HasValue && !item.EsInterno.Value)
                    {
                        var mProveedor = await _catProveedorRepository.GetAsync((int)item.IdProveedor);
                        caracteres = mProveedor.Correo.Split(' ');
                        string mCorreoNuevo = string.Join("\\t", caracteres);

                        var resultCorreo = _atlasRepository.EnviaNotificacion(new NotificacionesInput
                        {
                            asunto = mAsuntoNuevo,
                            para = mCorreoNuevo,
                            mensaje = mCuerpoNuevo,
                            numeroReporte = item.NumeroOficio,
                            archivos = mArchivos
                        });
                    }
                    
                }
                
            }

            _oficiosRepository.GuardaTrackingOficio(new GuardaTrackingOficioInput
            {
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                NumDemanda = input.AsignarAbogado.Select(s => s.NumeroOficio).FirstOrDefault(),
                Movimiento = "Se asignó abogado"
            });
            return result;
        }

        public async Task<ConsultaOficioOutput> ConsultaOficio(ConsultaOficioInput input)
        {
            var result = await _oficiosRepository.ConsultaOficio(input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            return result;
        }

        public async Task<DetalleOficioOutput> DetalleOficio(DetalleOficioInput input)
        {
            var result = await _oficiosRepository.DetalleOficio(input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            return result;
        }

        public async Task<ConcluirOficioOutput> ConcluirOficio(ConcluirOficioInput input)
        {
            var result = await _oficiosRepository.ConcluirOficio(input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            _oficiosRepository.GuardaTrackingOficio(new GuardaTrackingOficioInput
            {
                NumDemanda = input.NumeroOficio,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                FechaRegistro = DateTime.Now,
                Movimiento = "Se concluyó el oficio"
            });

            return result;
        }

        public async Task<ObtenerReporteOut> ReportesOficios(ReportesOficiosInput input)
        {
            var result = await _oficiosRepository.ObtenerReporte("spReporteOficios", input);
            if (!(result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null))
                throw new UserFriendlyException(400, result.Mensaje);
            return result;
        }

        public async Task<ObtenerTrackingsOficioOutput> ObtenerTrackingsOficio(ObtenerTrackingsOficioInput input)
        {
            var result = await _oficiosRepository.ObtenerTrackingsOficio(input);

            result.Trackings = result.Trackings.Where(s => s.NumOficio == input.NumOficio).ToList();
            return result;
        }

        public async Task<ObtenerComentariosOficiosOutput> ObtenerComentariosOficios(ObtenerComentariosOficiosInput input)
        {
            var result = await _oficiosRepository.ObtenerComentariosOficios(input);

            result.Comentarios = result.Comentarios.Where(s => s.NumOficio == input.NumOficio).ToList();
            return result;
        }

        public async Task<GuardaComentarioOficiosOutput> GuardaComentarioOficios(GuardaComentarioOficiosInput input)
        {
            _oficiosRepository.GuardaComentarioOficios(new GuardaComentarioOficiosInput
            {
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                NumOficio = input.NumOficio,
                Comentario = input.Comentario
            });

            return new GuardaComentarioOficiosOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public async Task<GuardaActualizaDocumentoOficiosOutput> GuardaActualizaDocumentoOficios(RegistraOficioDocumentoInput input)
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
                string objectId = _oficiosRepository.ObtenerObjectIdOficios(new ObtenerObjectIdOficiosInput { IdDocumento = (int)input.IdDocumento });
                input.Objeto = objectId;
                _contentRepository.EliminarDocumento(input.Objeto);
                await _oficiosRepository.RegistraOficioDocumento(input);
            }
            else
            {
                if (input.IdDocumento != null)
                {
                    string objectId = _oficiosRepository.ObtenerObjectIdOficios(new ObtenerObjectIdOficiosInput { IdDocumento = (int)input.IdDocumento });
                    _contentRepository.EliminarDocumento(objectId);
                }

                var mConsulta = _oficiosRepository.ObtenerIdRegExp((int)input.IdOficio);

                var mGuardarDocumento = new GuardarDocumentoInput
                {
                    IdRegExp = mConsulta.IdRegExp,
                    IdTipoDocumental = idTipoDoc,
                    Nombre = input.Nombre,
                    Extension = input.TipoMime,
                    Base64 = input.Contenido,
                    ExpedienteLlave = mConsulta.NumOficio,
                    Folder = $"oficios/{mConsulta.NumOficio}",
                    atributos = mAtributos,
                    NodeType = nodeType,
                    EsGuardado = true
                };

                _contentRepository.GuardarDocumento(mGuardarDocumento);
                input.Objeto = mGuardarDocumento.ObjectId;
                input.FechaRegistro = DateTime.Now;
                await _oficiosRepository.RegistraOficioDocumento(input);
            }

            return new GuardaActualizaDocumentoOficiosOutput
            {
                Mensaje = "Operación Exitosa"
            };

            
        }

        public async Task<ObtenerDocumentoOficiosOutput> ObtenerDocumentoOficios(ObtenerDocumentoOficiosInput input)
        {
            var result = await _oficiosRepository.ObtenerDocumentoOficios(input);
            _contentRepository.ObtenerDocumento(result.ObjectId, out string contenido);

            result.Contenido = contenido;

            return result;
        }
    }
}