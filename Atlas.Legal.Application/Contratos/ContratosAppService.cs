using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Atlas.Legal.Autorizacion;
using Atlas.Legal.Content;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atlas.Legal.ContratosInputOutput;

namespace Atlas.Legal
{
    public class ContratosAppService : LegalAppServiceBase
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IContentRepository _contentRepository;
        private readonly IContratosRepository _contratosRepository;

        public ContratosAppService(IHttpContextAccessor httpContextAccessor,
            IRepository<OpeUsuario, int> opeUsuarioRepository,
            IObjectMapper objectMapper,
            IContentRepository contentRepository,
            IContratosRepository contratosRepository
            ) : base(httpContextAccessor, opeUsuarioRepository)
        {
            _objectMapper = objectMapper;
            _contentRepository = contentRepository;
            _contratosRepository = contratosRepository;
        }

        /*
         * MAHV 31/05/2022
         * Controllador para Registro de nueva solicitud
         */
        public async Task<RegistroNuevoContratoOutput> registroNuevaSolicitud(RegistroNuevoContratoInput input)
        {
            try
            {
                string msj = "";
                if (input.NumeroContrato == null)
                    msj = "Se creó un registro nuevo";
                else
                    msj = "Se actualizo el registro";

                input.FechaCreacion = DateTime.Now;

                var result = await _contratosRepository.RegistroNuevaSolicitud(input);

                input.NumeroContrato = result.FolioContrato;

                if (input.IdTipoContrato != null)
                {
                    _contratosRepository.GuardarListaArchivosContrato(input);
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
                            ExpedienteLlave = input.NumeroContrato,
                            Folder = $"contratos/{input.NumeroContrato}",
                            atributos = mAtributos,
                            NodeType = nodeType,
                            EsGuardado = true
                        };

                        _contentRepository.GuardarDocumento(mGuardarDocumento);
                        input.IdRegExp = mGuardarDocumento.IdRegExp;
                        item.ObjectId = mGuardarDocumento.ObjectId;

                        _contratosRepository.GuardarDocumentoContrato(new GuardarDocumentoContratosInput
                        {
                            IdDocumento = item.IdDocumento,
                            IdContrato = result.IdContrato,
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

                result = await _contratosRepository.RegistroNuevaSolicitud(input);

                _contratosRepository.GuardarTrackingContrato(new GuardarTrackingContratoInput
                {
                    UsuarioRegistro = this.InformacionUsuario.Usuario,
                    FechaRegistro = DateTime.Now,
                    NoFolio = result.FolioContrato,
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
         * MAHV 02/06/2022
         * Controlador para consultar contratos
         */
        public async Task<ConsultaContratoOutput> consultaContrato(ConsultaContratoInput input)
        {
            try
            {
                var result = await _contratosRepository.ConsultaContrato(input);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 02/06/2022
         * Controlador para el guardado de documentos
         */
        public async Task<GuardarDocumentoContratosOutput> guardarDocumentoContrato(GuardarDocumentoContratosInput input)
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
                string objectId = _contratosRepository.ObtenerObjectIdContrato(new ObtenerObjectIdContratoInput { IdDocumento = (int)input.IdDocumento });
                input.ObjectId = objectId;
                _contentRepository.EliminarDocumento(input.ObjectId);
                _contratosRepository.GuardarDocumentoContrato(input);
            }
            else
            {
                if (input.IdDocumento != null)
                {
                    string objectId = _contratosRepository.ObtenerObjectIdContrato(new ObtenerObjectIdContratoInput { IdDocumento = (int)input.IdDocumento });
                    _contentRepository.EliminarDocumento(objectId);
                }

                var mConsulta = _contratosRepository.ObtenerIdRegExpContrato((int)input.IdContrato);

                var mGuardarDocumento = new GuardarDocumentoInput
                {
                    IdRegExp = mConsulta.IdRegExp,
                    IdTipoDocumental = idTipoDoc,
                    Nombre = input.Nombre,
                    Extension = input.TipoMime,
                    Base64 = input.Contenido,
                    ExpedienteLlave = mConsulta.NumeroContrato,
                    Folder = $"contratos/{mConsulta.NumeroContrato}",
                    atributos = mAtributos,
                    NodeType = nodeType,
                    EsGuardado = true
                };

                _contentRepository.GuardarDocumento(mGuardarDocumento);
                input.ObjectId = mGuardarDocumento.ObjectId;
                input.FechaRegistro = DateTime.Now;

                _contratosRepository.GuardarDocumentoContrato(input);
            }

            return new GuardarDocumentoContratosOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        /*
         * MAHV 03/06/2022
         * Controlador para consultar contratos
         */
        public async Task<ConsultaSolicitanteOutput> consultaSolicitante(ConsultaSolicitanteInput input)
        {
            try
            {
                var result = await _contratosRepository.ConsultaSolicitante(input);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 03/06/2022
         * Controlador para consultar contratos
         */
        public async Task<ConsultaAbogadoOutput> consultaAbogado(ConsultaAbogadoInput input)
        {
            try
            {
                var result = await _contratosRepository.ConsultaAbogado(input);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 03/06/2022
         * Controlador para consultar el detalle
         */
        public async Task<DetalleContratoOutput> detalleSolicitud(DetalleContratoInput input)
        {
            try
            {
                var result = await _contratosRepository.DetalleSolicitud(input);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 06/06/2022
         * Controlador para obtener los comentarios
         */
        public async Task<ComentarioContratoOutput> obtenerComentarioContrato(ComentarioContratoInput input)
        {
            try
            {
                var result = await _contratosRepository.ObtenerComentarioContrato(input);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 06/06/2022 
         * Controlador para guardar los comentarios
         */
        public async Task<GuardarComentarioContratoOutput> guardarComentarioContrato(GuardarComentarioContratoInput input)
        {
            try
            {
                _contratosRepository.GuardarComentarioContrato(new GuardarComentarioContratoInput
                {
                    Comentario = input.Comentario,
                    FechaRegistro = DateTime.Now,
                    NoFolio = input.NoFolio,
                    UsuarioRegistro = this.InformacionUsuario.Usuario
                });

                return new GuardarComentarioContratoOutput
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
         * MAHV 06/06/2022
         * Controlador para obtener el tracking
         */
        public async Task<ObtenerTrackingContratoOutput> obtenerTrackingContrato(ObtenerTrackingContratoInput input)
        {
            try
            {
                var result = await _contratosRepository.ObtenerTrackingContrato(input);

                result.Trackings = result.Trackings.Where(s => s.NumeroContrato == input.NoFolio).ToList();

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * 
         * 
         */
        public async Task<ReporteGeneralContratoOutput> ReporteGeneralContrato(ReporteGeneralContratoInput input)
        {
            var result = await _contratosRepository.ReporteGeneralContrato(input);

            return result;
        }

        /*
         * 
         * 
         */
        public async Task<ReportePrestadoresOutput> ReportePrestadores(ReportePrestadoresInput input)
        {
            var result = await _contratosRepository.ReportePrestadores(input);

            return result;
        }
    }
}
