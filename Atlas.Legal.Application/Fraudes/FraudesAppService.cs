using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Atlas.Legal.AtlasWS;
using Microsoft.AspNetCore.Hosting;
using Atlas.Legal.Autorizacion;
using Atlas.Legal.Comun;
using Atlas.Legal.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atlas.Legal.AtlasWS.AtlasWSOutput;
using static Atlas.Legal.FraudesInputOutput;
using Atlas.Legal.Configuration;

namespace Atlas.Legal
{
    public class FraudesAppService : LegalAppServiceBase
    {
        private const string URL_FRAUDES = "fraudes/consulta";
        private readonly IObjectMapper _objectMapper;
        private readonly IContentRepository _contentRepository;
        private readonly IAtlasWSRepository _atlasWSRepository;
        private readonly IFraudesRepository _fraudesRepository;
        private readonly IRepository<CatAbogadoInterno, int> _catAbogadoInternoRespository;
        private readonly IRepository<CatPlantillaCorreo, int> _catPlantillaCorreoRepository;
        private readonly IRepository<CatProveedor, int> _catProveedorRepository;
        private readonly IConfigurationRoot _config;

        public FraudesAppService(
            IHttpContextAccessor httpContextAccessor,
            IRepository<OpeUsuario, int> opeUsuarioRepository,
            IObjectMapper objectMapper,
            IContentRepository contentRepository,
            IAtlasWSRepository atlasWSRepository,
            IRepository<CatAbogadoInterno, int> catAbogadoInternoRespository,
            IRepository<CatPlantillaCorreo, int> catPlantillaCorreoRepository,
            IRepository<CatProveedor, int> catProveedorRepository,
            IHostingEnvironment env,
            IFraudesRepository fraudesRepository
            ) : base(httpContextAccessor, opeUsuarioRepository)
        {
            _objectMapper = objectMapper;
            _contentRepository = contentRepository;
            _atlasWSRepository = atlasWSRepository;
            _fraudesRepository = fraudesRepository;
            _catAbogadoInternoRespository = catAbogadoInternoRespository;
            _catPlantillaCorreoRepository = catPlantillaCorreoRepository;
            _catProveedorRepository = catProveedorRepository;
            _config = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        /*
         * MAHV 12/05/2022
         * Método para guardar el registro de Fraudes
         */
        public async Task<RegistroFraudesOutput> registroFraudes(RegistroFraudesInput input)
        {
            try
            {
                string msj = "";
                if (input.NumeroFraude == null)
                    msj = "Se creó un registro nuevo";
                else
                    msj = "Se actualizo el registro";

                input.UsuarioRegistro = this.InformacionUsuario.Usuario;
                input.FechaRegistro = DateTime.Now;
                input.FechaCreacion = DateTime.Now;

                var result = await _fraudesRepository.RegistroFraudes(input);

                input.NumeroFraude = result.NumeroFraude;

                if (input.CausaProblema != null)
                {
                    foreach (var c in input.CausaProblema)
                    {
                        _fraudesRepository.GuardarCausaProblemaFraudes(new GuardarCausaProblemaFraudesInput
                        {
                            Id = c.Id,
                            NumeroFraude = input.NumeroFraude,
                            CausaProblema = c.CausaProblema,
                            RealizadoPor = this.InformacionUsuario.Usuario,
                            FechaRegistro = c.FechaRegistro
                        });
                    }
                }

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
                }
                else
                {

                    foreach (var item in input.Documentos)
                    {
                        var mAtributos = new Dictionary<string, string>();
                        mAtributos.Add("nombre", item.SubTipoDocumento);

                        var mGuardarDocumento = new GuardarDocumentoInput
                        {
                            IdRegExp = input.IdRegExp,
                            IdTipoDocumental = 175,
                            Nombre = item.Nombre,
                            Extension = item.TipoMime,
                            Base64 = item.Contenido,
                            IdTipoExpediente = 178,
                            ExpedienteLlave = input.NumeroFraude,
                            Folder = $"fraudes/{input.NumeroFraude}",
                            atributos = mAtributos,
                            EsGuardado = true
                        };

                        _contentRepository.GuardarDocumento(mGuardarDocumento);
                        input.IdRegExp = mGuardarDocumento.IdRegExp;
                        item.ObjectId = mGuardarDocumento.ObjectId;

                        _fraudesRepository.GuardarDocumentoFraudes(new GuardarDocumentoFraudesInput
                        {
                            IdDocumento = item.IdDocumento,
                            IdFraudes = result.IdFraude,
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

                result = await _fraudesRepository.RegistroFraudes(input);

                _fraudesRepository.GuardarTrackingFraude(new GuardarTrackingFraudeInput
                {
                    UsuarioRegistro = this.InformacionUsuario.Usuario,
                    FechaRegistro = DateTime.Now,
                    NumeroFraude = result.NumeroFraude,
                    Movimiento = msj
                });

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 13/05/2022
         * Método que realiza el guardado de la asignación del fraude
         */
        public async Task<GuardarAsignacionFraudesOutput> asignacionFraude(GuardarAsignacionFraudesInput input)
        {
            try
            {
                
                var result = await _fraudesRepository.GuardarAsignacionFraudes(input);                         

                var mPlantilla = await _catPlantillaCorreoRepository.FirstOrDefaultAsync(s => s.Nombre.Equals("Fraudes_Asignacion"));
                if (mPlantilla != null)
                {
                    var mArchivos = new List<ArchivosInput>();

                    var output = await _fraudesRepository.ConsultaFraudes(new ConsultaFraudesInput
                    {
                        NumeroFraude = input.NumeroFraude
                    });

                    var mSolicitud = _objectMapper.Map<PlantillaFraudesModel>(output.Fraudes.FirstOrDefault());

                    mSolicitud.AppUrl = $"{_config["EndPoints:BaseUrlAtlasLegal"]}{URL_FRAUDES}";

                    var mCuerpo = PlantillaManager.LlenarPlantilla(mPlantilla.Plantilla, mSolicitud);

                    var caracteres = mCuerpo.Split(' ');
                    var mCuerpoNuevo = string.Join("\\t", caracteres);

                    string mAsunto = $"{mPlantilla.Asunto} {input.NumeroFraude}";

                    caracteres = mAsunto.Split(' ');
                    var mAsuntoNuevo = string.Join("\\t", caracteres);


                    if (input.EsInterno)
                    {
                        var mAbogado = await _catAbogadoInternoRespository.GetAsync(input.IdProveedor);

                        //TODO: Corregir para solo un correo
                        caracteres = mAbogado.Correo.Split(' ');
                        string mCorreoNuevo = string.Join("\\t", caracteres);

                        var resultCorreo = _atlasWSRepository.EnviaNotificacion(new NotificacionesInput
                        {
                            asunto = mAsuntoNuevo,
                            para = mCorreoNuevo,
                            mensaje = mCuerpoNuevo,
                            numeroReporte = input.NumeroFraude,
                            archivos = mArchivos
                        });                       

                    }
                    else
                    {
                        var mProveedor = await _catProveedorRepository.GetAsync(input.IdProveedor);
                        caracteres = mProveedor.Correo.Split(' ');
                        string mCorreoNuevo = string.Join("\\t", caracteres);

                        var resultCorreo = _atlasWSRepository.EnviaNotificacion(new NotificacionesInput
                        {
                            asunto = mAsuntoNuevo,
                            para = mCorreoNuevo,
                            mensaje = mCuerpoNuevo,
                            numeroReporte = input.NumeroFraude,
                            archivos = mArchivos
                        });
                    }
                }

                _fraudesRepository.GuardarTrackingFraude(new GuardarTrackingFraudeInput
                {
                    UsuarioRegistro = this.InformacionUsuario.Usuario,
                    FechaRegistro = DateTime.Now,
                    NumeroFraude = input.NumeroFraude,
                    Movimiento = $"Se asignó proveedor al fraude con numero de folio: '{input.NumeroFraude}'"
                });

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 16/05/2022 
         * Método que realiza la consulta de los fraudes
        */
        public async Task<ConsultaFraudesOutput> consultaFraudes(ConsultaFraudesInput input)
        {
            try
            {
                var result = await _fraudesRepository.ConsultaFraudes(input);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 17/05/2022
         * Método que realiza el llamado del WS para consultar el detalle
        */
        public async Task<DetalleFraudesOutput> detalleFraudes(DetalleFraudesInput input)
        {
            try
            {
                var result = await _fraudesRepository.DetalleFraudes(input);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 17/05/2022
         * Método que llama al WS para concluir el Fraude
         */
        public async Task<ConcluirFraudeOutput> concluirFraude(ConcluirFraudeInput input)
        {
            try
            {
                _fraudesRepository.ConcluirFraude(input);

                _fraudesRepository.GuardarTrackingFraude(new GuardarTrackingFraudeInput
                {
                    UsuarioRegistro = this.InformacionUsuario.Usuario,
                    FechaRegistro = DateTime.Now,
                    NumeroFraude = input.NumeroFraude,
                    Movimiento = $"Se concluyo fraude con numero de folio: '{input.NumeroFraude}'"
                });

                return new ConcluirFraudeOutput
                {
                    Mensaje = "Operación Exitosa"
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 17/05/2022
         * Método para guardar comentario del fraude
         */
        public async Task<GuardarComentarioFraudeOutput> guardarComentario(GuardarComentarioFraudeInput input)
        {
            try
            {
                _fraudesRepository.GuardarComentarioFraude(new GuardarComentarioFraudeInput
                {
                    Comentario = input.Comentario,
                    FechaRegistro = DateTime.Now,
                    NumeroFraude = input.NumeroFraude,
                    UsuarioRegistro = this.InformacionUsuario.Usuario
                });

                return new GuardarComentarioFraudeOutput
                {
                    Mensaje = "Operación Exitosa"
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 17/05/2022
         * Método que llama al WS para obtener los trackings de Fraudes
         */
        public async Task<ObtenerTrackingFraudesOutput> obtenerTracking(ObtenerTrackingFraudesInput input)
        {
            try
            {
                var result = await _fraudesRepository.ObtenerTrackingFraudes(input);

                result.Trackings = result.Trackings.Where(s => s.NumeroFraude == input.NumeroFraude).ToList();

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 17/05/2022 
         * Método que llama al WS para obtener los reportes de Fraudes
         */
        public async Task<ConsultaReportesFraudesOutput> busquedaReporte(ConsultaReportesFraudesInput input)
        {
            try
            {
                var result = await _fraudesRepository.ConsultaReportesFraudes(input);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 18/05/2022
         * Método que llama el WS para obtener el documento
         */
        public async Task<ObtenerDocumentoFraudesOutput> obtenerDocumentoFraudes(ObtenerDocumentoFraudesInput input)
        {
            var result = await _fraudesRepository.ObtenerDocumentoFraudes(input);
            _contentRepository.ObtenerDocumento(result.Objeto, out string contenido);

            result.Contenido = contenido;

            return result;

        }

        /*
         * MAHV 18/05/2022 
         * Método para borrar o guardar nuevos documentos
         */
        public async Task<GuardarDocumentoFraudeOutput> actualizaDocumentoFraude(GuardarDocumentoFraudesInput input)
        {
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
                string objectId = _fraudesRepository.ObtenerObjectIdFraude(new ObtenerObjectIdFraudeInput { IdDocumento = (int)input.IdDocumento });
                input.ObjectId = objectId;
                _fraudesRepository.GuardarDocumentoFraudes(input);
                _contentRepository.EliminarDocumento(input.ObjectId);
            }
            else
            {
                input.UsuarioRegistro = this.InformacionUsuario.Usuario;
                if (input.IdDocumento != null)
                {
                    string objectId = _fraudesRepository.ObtenerObjectIdFraude(new ObtenerObjectIdFraudeInput { IdDocumento = (int)input.IdDocumento });
                    _contentRepository.EliminarDocumento(objectId);
                }

                var mConsulta = _fraudesRepository.ObtenerIdRegExpFraude((int)input.IdFraudes);

                var mGuardarDocumento = new GuardarDocumentoInput
                {
                    IdRegExp = mConsulta.IdRegExp,
                    IdTipoDocumental = idTipoDoc,
                    Nombre = input.Nombre,
                    Extension = input.TipoMime,
                    Base64 = input.Contenido,
                    ExpedienteLlave = mConsulta.NumeroFraude,
                    Folder = $"fraudes/{mConsulta.NumeroFraude}",
                    NodeType = nodeType,
                    atributos = mAtributos,
                    EsGuardado = true
                };

                _contentRepository.GuardarDocumento(mGuardarDocumento);
                input.ObjectId = mGuardarDocumento.ObjectId;
                input.FechaRegistro = DateTime.Now;

                _fraudesRepository.GuardarDocumentoFraudes(input);
            }

            return new GuardarDocumentoFraudeOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        /*
         * MAHV 24/05/2022
         * Método para consultar los comentarios
         */
        public async Task<ObtenerComentariosFraudesOutput> obtenerComentariosFraudes(ObtenerComentariosFraudesInput input)
        {
            var result = await _fraudesRepository.ObtenerComentariosFraudes(input);

            result.Comentarios = result.Comentarios.Where(c => c.NumeroFraude == input.NumeroFraude).ToList();

            return result;
        }

        /*
         * MAHV 24/05/2022
         * Método para guardar el documento
         */
        public async Task<GuardarDocumentoFraudeOutput> guardarDocumentoFraude(GuardarDocumentoFraudesInput input)
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
                string objectId = _fraudesRepository.ObtenerObjectIdFraude(new ObtenerObjectIdFraudeInput { IdDocumento = (int)input.IdDocumento });
                input.ObjectId = objectId;
                _contentRepository.EliminarDocumento(input.ObjectId);
                _fraudesRepository.GuardarDocumentoFraudes(input);
            }
            else
            {
                if (input.IdDocumento != null)
                {
                    string objectId = _fraudesRepository.ObtenerObjectIdFraude(new ObtenerObjectIdFraudeInput { IdDocumento = (int)input.IdDocumento });
                    _contentRepository.EliminarDocumento(objectId);
                }

                var mConsulta = _fraudesRepository.ObtenerIdRegExpFraude((int)input.IdFraudes);

                var mGuardarDocumento = new GuardarDocumentoInput
                {
                    IdRegExp = mConsulta.IdRegExp,
                    IdTipoDocumental = idTipoDoc,
                    Nombre = input.Nombre,
                    Extension = input.TipoMime,
                    Base64 = input.Contenido,
                    ExpedienteLlave = mConsulta.NumeroFraude,
                    Folder = $"fraudes/{mConsulta.NumeroFraude}",
                    atributos = mAtributos,
                    NodeType = nodeType,
                    EsGuardado = true
                };

                _contentRepository.GuardarDocumento(mGuardarDocumento);
                input.ObjectId = mGuardarDocumento.ObjectId;
                input.FechaRegistro = DateTime.Now;

                _fraudesRepository.GuardarDocumentoFraudes(input);
            }

            return new GuardarDocumentoFraudeOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }
    }
}
