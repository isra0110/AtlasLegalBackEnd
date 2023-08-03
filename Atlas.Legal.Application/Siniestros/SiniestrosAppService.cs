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
using Atlas.Legal.AtlasWS;
using static Atlas.Legal.AtlasWS.AtlasWSOutput;
using Abp.Extensions;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
//using Microsoft.AspNetCore.Authorization;

namespace Atlas.Legal
{
    //[Authorize]
    public class SiniestrosAppService : LegalAppServiceBase, ISiniestrosAppService
    {
        private readonly IObjectMapper _objectMapper;
        private readonly ISiniestrosRepository _siniestrosRepository;
        private readonly IRepository<Siniestro, int> _opeSiniestroRepository;
        private readonly IContentRepository _contentRepository;
        private readonly IAutorizacionRepository _autorizacionRepository;
        private readonly IAtlasWSRepository _atlasRepository;

        public int idModificacion = 0;

        public SiniestrosAppService(
            IObjectMapper objectMapper,
            ISiniestrosRepository siniestrosRepository,
            IRepository<Siniestro, int> opeSiniestroRepository,
            IAutorizacionRepository autorizacionRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<OpeUsuario, int> opeUsuarioRepository,
            IAtlasWSRepository atlasWSRepository,
            IContentRepository contentRepository) : base (httpContextAccessor, opeUsuarioRepository)            
        {
            _objectMapper = objectMapper;
            _siniestrosRepository = siniestrosRepository;
            _opeSiniestroRepository = opeSiniestroRepository;
            _contentRepository = contentRepository;
            _autorizacionRepository = autorizacionRepository;
            _atlasRepository = atlasWSRepository;

        }

        public async Task<SiniestroOutput> CrearActualizarSiniestroProveedor(SiniestroInput input)
        {
            string jsonEntrada = null;
            var mBitacora = new BitacoraEventosInput
            {
                Fecha = DateTime.Now,
                IdModulo = 2,
                //JsonEntrada = jsonEntrada,
                NumeroReporte = input.Bitacora.NumeroReporteProveedorLegal
            };

            try
            {
                //var mRegex = new Regex("[a-zA-Z]");
                //if (mRegex.IsMatch(input.Bitacora.NumeroReporteProveedorLegal))
                    //throw new UserFriendlyException(400, "El NumeroReporteProveedorLegal contiene caracteres alfanumericos");
                var numeroReporte = input.Bitacora.NumeroReporteProveedorLegal.Split('-');
                input.Bitacora.NumeroReporteProveedorLegal = numeroReporte[0];

                idModificacion = 3;

                if (input.Siniestralidad.Id == 0)
                    idModificacion = 1;
                if (input.Siniestralidad.IdEstatus == 2)
                    idModificacion = 6;
                else if (input.Siniestralidad.IdEstatus == 3)
                    idModificacion = 2;

                _siniestrosRepository.InsertOrUpdateSiniestro(input);

                _siniestrosRepository.ObtenerIdRegExp(input.Siniestralidad.NumeroSiniestroAtlas, out int idRegExp);
                if (idRegExp > 0 || idRegExp != 0)
                    input.IdRegExp = idRegExp;

                if (input.Documentos != null)
                {
                    foreach (var mDoc in input.Documentos)
                    {
                        string subTipoDocumento = mDoc.SubTipoDocumento;
                        int largo = 0;
                        if (mDoc.SubTipoDocumento.Length > 20)
                        {
                            largo = mDoc.SubTipoDocumento.Length - 20;
                            subTipoDocumento = mDoc.SubTipoDocumento.Remove(20, largo);
                        }
                        else
                            subTipoDocumento = mDoc.SubTipoDocumento;


                        var mAtributos = new Dictionary<string, string>();
                        var mTipoDoc = mDoc.TipoDocumento;
                        int idTipoDoc = 0;
                        string nodeType = string.Empty;


#if (DEBUG || TEST)
                        mAtributos.Add("AsistenciaLegal", mDoc.SubTipoDocumento);
                        idTipoDoc = 184;
                        nodeType = "AtlasLegal_Desarrollo";
#else
                        idTipoDoc = 1;
                        mAtributos.Add("AsistenciaLegal", subTipoDocumento);
                        nodeType = "AtlasLegal_AsistenciaLegal";
#endif

                        var mGuardarDocumento = new GuardarDocumentoInput
                        {
                            IdRegExp = input.IdRegExp,
                            IdTipoDocumental = idTipoDoc,
                            Nombre = mDoc.Nombre,
                            Extension = mDoc.TipoMime,
                            Base64 = mDoc.Contenido,
                            IdTipoExpediente = 178,
                            ExpedienteLlave = input.Siniestralidad.NumeroSiniestroAtlas,
                            Folder = $"siniestro/{input.Siniestralidad.NumeroSiniestroAtlas}",
                            atributos = mAtributos,
                            NodeType = nodeType,
                            EsGuardado = true
                        };

                        _contentRepository.GuardarDocumento(mGuardarDocumento);

                        input.IdRegExp = mGuardarDocumento.IdRegExp;
                        input.ObjectId = mGuardarDocumento.ObjectId;

                        _siniestrosRepository.GuardarActualizarDocumento(null, (int)input.Siniestralidad.Id, mDoc.Nombre, input.ObjectId, mDoc.TipoMime, mDoc.TipoDocumento, mDoc.SubTipoDocumento, this.InformacionUsuario.Usuario, false);
                    }
                }
                else
                {
                    if (input.IdRegExp == 0 || !input.IdRegExp.HasValue)
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
                this.CurrentUnitOfWork.SaveChanges();

                if (input.IdRegExp > 0 || !input.IdRegExp.HasValue)
                    _siniestrosRepository.InsertOrUpdateSiniestro(input);

                //if (input.Documentos != null)


                _siniestrosRepository.GuardaBitacora(input.Siniestralidad.NumeroSiniestroAtlas, input.Bitacora.NumeroReporteProveedorLegal, this.InformacionUsuario.Usuario, idModificacion);
               
            }
            catch (Exception ex)
            {
                if (input.Documentos != null)
                {
                    foreach (var item in input.Documentos)
                    {
                        item.Contenido = "";
                    }
                }
                
                jsonEntrada = JsonConvert.SerializeObject(input);
                mBitacora.Respuesta = ex.Message;
                mBitacora.JsonEntrada = jsonEntrada;

                await _siniestrosRepository.GuardaBitacoraEventos(mBitacora);
                this.CurrentUnitOfWork.SaveChanges();

                return new SiniestroOutput
                {
                    Mensaje = ex.Message,
                    IdSiniestro = 0,
                    Exito = false
                };
            }

            if (input.Documentos != null)
            {
                foreach (var item in input.Documentos)
                {
                    item.Contenido = "";
                }
            }
            
            jsonEntrada = JsonConvert.SerializeObject(input);
            mBitacora.JsonEntrada = jsonEntrada;
            mBitacora.Respuesta = "Operacion Exitosa";
            await _siniestrosRepository.GuardaBitacoraEventos(mBitacora);

            return new SiniestroOutput
            {
                Mensaje = "Operación Exitosa",
                IdSiniestro = input.Siniestralidad.Id,
                Exito = true
            };


        }

        public async Task<SiniestroSisaOutput> CrearActualizarSiniestroSisa(SiniestroSISAInput input)
        {
            var mBitacora = new BitacoraEventosInput
            {
                Fecha = DateTime.Now,
                IdModulo = 3,
            };

            JuridicoOutput objSisa = new JuridicoOutput();
            JuridicoInput juridicoInput = new JuridicoInput();
            SiniestroSisaInput siniestroSisaInput = new SiniestroSisaInput();

            try
            {
                idModificacion = 3;
                
                if (input.NumeroReporte != null)
                {
                    juridicoInput.numeroReporte = input.NumeroReporte;
                    objSisa = await _atlasRepository.ConsultaJuridico(juridicoInput);
                    //if(objSisa.Siniestralidad.NumeroReporteProveedorLegal.Equals("") || objSisa.Siniestralidad.NumeroReporteProveedorLegal == null)
                    //    objSisa.Siniestralidad.NumeroReporteProveedorLegal = input.NumeroReporte;
                }

                if (objSisa.Siniestralidad.NumeroReporteProveedorLegal.Length == 0)
                {
                    objSisa.Siniestralidad.NumeroReporteProveedorLegal = await _siniestrosRepository.ObtenerNumeroReporteProveedor(input.NumeroReporte);
                }

                mBitacora.NumeroReporte = objSisa.Siniestralidad.NumeroReporteProveedorLegal;
                mBitacora.JsonEntrada = JsonConvert.SerializeObject(objSisa);

                if (objSisa.Siniestralidad.NumeroSiniestroAtlas.Contains("0-0-0--0-0"))
                {
                    return new SiniestroSisaOutput
                    {
                        Mensaje = "No hay información para actualizar"
                    };
                }

                if (objSisa.Siniestralidad.Id == 0)
                    idModificacion = 1;

                //Registro de Siniestros
                _siniestrosRepository.InserOrUpdateSiniestroSisa(objSisa, input.IdRegExp);

                //Registro de recuperaciones
                if (objSisa.Recuperaciones.Count != 0)
                {
                    foreach (var recuperacion in objSisa.Recuperaciones)
                    {
                        if (recuperacion.DeducibleEstimado == "")
                            recuperacion.DeducibleEstimado = null;

                        if (recuperacion.MontoRecuperado == "")
                            recuperacion.MontoRecuperado = null;

                        if (recuperacion.HonorarioComision == "")
                            recuperacion.HonorarioComision = null;

                        if (recuperacion.Iva == "")
                            recuperacion.Iva = null;

                        if (recuperacion.Total == "")
                            recuperacion.Total = null;

                        if (recuperacion.DeducibleValuacion == "")
                            recuperacion.DeducibleValuacion = null;

                        var mRecuperaciones = new RecuperacionesInput
                        {
                            NumeroReporte = input.NumeroReporte,
                            Regional = recuperacion.Regional,
                            DeducibleEstimado = Convert.ToDouble(recuperacion.DeducibleEstimado),
                            MontoRecuperado = Convert.ToDouble(recuperacion.MontoRecuperado),
                            HonorarioComision = Convert.ToDouble(recuperacion.HonorarioComision),
                            Iva = Convert.ToDouble(recuperacion.Iva),
                            Total = Convert.ToDouble(recuperacion.Total),
                            FechaRecuperacionR = recuperacion.FechaRecuperacion,
                            DeducibleValuacion = Convert.ToDouble(recuperacion.DeducibleValuacion),
                            FolioFactura = recuperacion.FolioFactura,
                            NumeroSiniestroAtlas = objSisa.Siniestralidad.NumeroSiniestroAtlas
                        };

                        _siniestrosRepository.InsertOrUpdateRecuperacionSisa(mRecuperaciones);
                    }
                }

                //Registro de Pago terceros
                if (objSisa.PagoTerceros.Count != 0)
                {
                    foreach (var pago in objSisa.PagoTerceros)
                    {
                        var mPagoTerceros = new PagoTercerosInput
                        {
                            NumeroReporte = input.NumeroReporte,
                            Regional = pago.Regional,
                            Beneficiario = pago.Beneficiario,
                            MontoPago = pago.MontoPago,
                            FechaComprobacion = pago.FechaComprobacion,
                            FechaPago = pago.FechaPago,
                            NumeroSiniestroAtlas = objSisa.Siniestralidad.NumeroSiniestroAtlas
                        };

                        _siniestrosRepository.InsertOrUpdatePagoTercerosSisa(mPagoTerceros);
                    }
                }


                _siniestrosRepository.ObtenerIdRegExp(objSisa.Siniestralidad.NumeroSiniestroAtlas, out int idRegExp);

                if (idRegExp > 0 || idRegExp != 0)
                    input.IdRegExp = idRegExp;

                if (objSisa.Documentos != null && objSisa.Documentos[0].Nombre != "")
                {
                    foreach (var mDoc in objSisa.Documentos)
                    {
                        var mAtributos = new Dictionary<string, string>();
                        var mTipoDoc = mDoc.TipoDocumento;
                        int idTipoDoc = 0;
                        string nodeType = string.Empty;
#if (DEBUG || TEST)
                        idTipoDoc = 184;
                        mAtributos.Add("AsistenciaLegal", mDoc.SubTipoDocumento);
                        nodeType = "AtlasLegal_Desarrollo";
#else
                        idTipoDoc = 1;
                        mAtributos.Add("AsistenciaLegal", mDoc.SubTipoDocumento);
                        nodeType = "AtlasLegal_AsistenciaLegal";
#endif
                        var mGuardarDocumento = new GuardarDocumentoInput
                        {
                            IdRegExp = input.IdRegExp,
                            IdTipoDocumental = idTipoDoc,
                            Nombre = mDoc.Nombre,
                            Extension = mDoc.TipoMime,
                            Base64 = mDoc.Contenido,
                            IdTipoExpediente = 178,
                            ExpedienteLlave = objSisa.Siniestralidad.NumeroSiniestroAtlas,
                            Folder = $"siniestro/{objSisa.Siniestralidad.NumeroSiniestroAtlas}",
                            atributos = mAtributos,
                            NodeType = nodeType,
                            EsGuardado = true
                        };

                        _contentRepository.GuardarDocumento(mGuardarDocumento);

                        input.IdRegExp = mGuardarDocumento.IdRegExp;
                        input.ObjectId = mGuardarDocumento.ObjectId;

                        _siniestrosRepository.GuardarActualizarDocumento(null, (int)objSisa.Siniestralidad.Id, mDoc.Nombre, input.ObjectId, mDoc.TipoMime, mDoc.TipoDocumento, mDoc.SubTipoDocumento, this.InformacionUsuario.Usuario, false);
                    }
                }
                else
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
                this.CurrentUnitOfWork.SaveChanges();

                if (objSisa.Documentos != null && objSisa.Documentos[0].Nombre != "")
                    _siniestrosRepository.InserOrUpdateSiniestroSisa(objSisa, input.IdRegExp);

                if (input.IdRegExp.HasValue)
                    _siniestrosRepository.InserOrUpdateSiniestroSisa(objSisa, input.IdRegExp);

                _siniestrosRepository.GuardaBitacora(objSisa.Siniestralidad.NumeroSiniestroAtlas, objSisa.Siniestralidad.NumeroReporteProveedorLegal, this.InformacionUsuario.Usuario, idModificacion);
            }
            catch (Exception ex)
            {
                mBitacora.Respuesta = ex.Message;

                await _siniestrosRepository.GuardaBitacoraEventos(mBitacora);
                this.CurrentUnitOfWork.SaveChanges();

                return new SiniestroSisaOutput
                {
                    Mensaje = ex.Message,
                    IdSiniestro = 0,
                    Exito = false
                };
            }

            mBitacora.Respuesta = "Operacion Exitosa";
            await _siniestrosRepository.GuardaBitacoraEventos(mBitacora);

            return new SiniestroSisaOutput
            {
                Mensaje = "Operación Exitosa",
                IdSiniestro = objSisa.Siniestralidad.Id,
                Exito = true
            };
        }

        public async Task<SiniestroPrometeoOutput> CrearActualizarSiniestroPrometeo(SiniestroPrometeoInput input)
        {
            var jsonEntrada = JsonConvert.SerializeObject(input);
            var mBitacora = new BitacoraEventosInput
            {
                Fecha = DateTime.Now,
                IdModulo = 1,
                JsonEntrada = jsonEntrada,
                NumeroReporte = input.Siniestralidad.NumeroReporteProveedorLegal
            };

            try
            {
                idModificacion = 3;

                if (input.Siniestralidad.Id == 0)
                    idModificacion = 1;

                _siniestrosRepository.InsertOrUpdateSiniestroPrometeo(input);

                _siniestrosRepository.ObtenerIdRegExp(input.Siniestralidad.NumeroSiniestroAtlas, out int idRegExp);
                if (idRegExp > 0 || idRegExp != 0)
                    input.IdRegExp = idRegExp;

                if (input.Documentos != null)
                {
                    foreach (var mDoc in input.Documentos)
                    {
                        try
                        {
                            byte[] hashBytes = Convert.FromBase64String(mDoc.Contenido);
                        }
                        catch (Exception ex)
                        {
                            throw new UserFriendlyException("El formato Base-64 es invalido");
                        }
                        var mAtributos = new Dictionary<string, string>();
                        var mTipoDoc = mDoc.TipoDocumento;
                        int idTipoDoc = 0;
                        string nodeType = string.Empty;

#if (DEBUG || TEST)
                        mAtributos.Add("AsistenciaLegal", mDoc.SubTipoDocumento);
                        idTipoDoc = 184;
                        nodeType = "AtlasLegal_Desarrollo";
#else
                        idTipoDoc = 1;
                        mAtributos.Add("AsistenciaLegal", mDoc.SubTipoDocumento);
                        nodeType = "AtlasLegal_AsistenciaLegal";
#endif

                        var mGuardarDocumento = new GuardarDocumentoInput
                        {
                            IdRegExp = input.IdRegExp,
                            IdTipoDocumental = idTipoDoc,
                            Nombre = mDoc.Nombre,
                            Extension = mDoc.TipoMime,
                            Base64 = mDoc.Contenido,
                            IdTipoExpediente = 178,
                            ExpedienteLlave = input.Siniestralidad.NumeroSiniestroAtlas,
                            Folder = $"siniestro/{input.Siniestralidad.NumeroSiniestroAtlas}",
                            atributos = mAtributos,
                            NodeType = nodeType,
                            EsGuardado = true
                        };

                        _contentRepository.GuardarDocumento(mGuardarDocumento);

                        input.IdRegExp = mGuardarDocumento.IdRegExp;
                        input.ObjectId = mGuardarDocumento.ObjectId;

                        _siniestrosRepository.GuardarActualizarDocumento(null, (int)input.Siniestralidad.Id, mDoc.Nombre, input.ObjectId, mDoc.TipoMime, mDoc.TipoDocumento, mDoc.SubTipoDocumento, this.InformacionUsuario.Usuario, false);
                    }
                }
                else
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

                this.CurrentUnitOfWork.SaveChanges();

                if (input.Documentos != null)
                    _siniestrosRepository.InsertOrUpdateSiniestroPrometeo(input);

                _siniestrosRepository.GuardaBitacora(input.Siniestralidad.NumeroSiniestroAtlas, input.Siniestralidad.NumeroReporteProveedorLegal, this.InformacionUsuario.Usuario, idModificacion);

            }
            catch (Exception ex)
            {
                mBitacora.Respuesta = ex.Message;

                await _siniestrosRepository.GuardaBitacoraEventos(mBitacora);
                this.CurrentUnitOfWork.SaveChanges();

                return new SiniestroPrometeoOutput
                {
                    Mensaje = ex.Message,
                    IdSiniestro = 0,
                    Exito = false
                };
            }

            mBitacora.Respuesta = "Operacion Exitosa";
            await _siniestrosRepository.GuardaBitacoraEventos(mBitacora);

            return new SiniestroPrometeoOutput
            {
                Mensaje = "Operación Exitosa",
                IdSiniestro = input.Siniestralidad.Id,
                Exito = true
            };
        }

        public async Task<GuardaBitacoraSolicitudOutput> GuardaBitacoraSolicitud(GuardaBitacoraSolicitudInput input)
        {
            _siniestrosRepository.GuardaBitacora(input.NoSiniestroAtlas, input.NumeroReporteProveedorLegal, input.Username, input.IdModificacionIntegracion);

            return new GuardaBitacoraSolicitudOutput
            {
                Mensaje = "Operacion Exitosa"
            };
        }

        public BusquedaSolicitudOutput BusquedaSolicitud(BusquedaSolicitudInput input)
        {
            var result = _siniestrosRepository.BusquedaSolicitud(input);

            return result;
        }

        public DetalleSolicitudOutput DetalleSolicitud(DetalleSolicitudInput input)
        {
            var result = _siniestrosRepository.DetalleSolicitud(input);

            return result;
        }

        public ConsultaBitacoraOutput ConsultaBitacora(ConsultaBitacoraInput input)
        {
            var result = _siniestrosRepository.ConsultaBitacora(input);

            return result;

        }

        public List<Dictionary<string, object>> BusquedaReporte(BusquedaReporteInput input)
        {
            if (input.IdTipoReporte == null)
                throw new UserFriendlyException(400, "Se necesita indicar el tipo de reporte y proveedor");
            else if (input.IdProveedor == null)
                throw new UserFriendlyException(400, "Se necesita indicar el tipo de reporte y proveedor");

            var result = _siniestrosRepository.BusquedaReporte(input);

            return result;
        }

        public ObtenerDocumentoSiniestroOutput ObtenerDocumentoSiniestro(ObtenerDocumentoSiniestroInput input)
        {
            var result = _siniestrosRepository.ObtenerDocumentoSiniestro(input);

            _contentRepository.ObtenerDocumento(result.ObjectId, out string contenido);

            result.Contenido = contenido;

            return result;
        }

        public async Task<AgregarActualizarDocumentoSiniestroOutput> AgregarActualizarDocumentoSiniestro(AgregarActualizarDocumentoSiniestroInput input)
        {
            _siniestrosRepository.ConsultarIdRegExp(input.IdSiniestro, out int idRegExp, out string numSiniestro);

            var mAtributos = new Dictionary<string, string>();
            
            var mTipoDoc = input.TipoDocumento;
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
            var mGuardarDocumento = new GuardarDocumentoInput
            {
                IdRegExp = idRegExp,
                IdTipoDocumental = idTipoDoc,
                Nombre = input.Nombre,
                Extension = input.TipoMime,
                Base64 = input.Contenido,
                IdTipoExpediente = 178,
                ExpedienteLlave = numSiniestro,
                Folder = $"siniestro/{numSiniestro}",
                atributos = mAtributos,
                NodeType = nodeType,
                EsGuardado = true
            };

            _contentRepository.GuardarDocumento(mGuardarDocumento);
            
            var mObjectId = mGuardarDocumento.ObjectId;

            _siniestrosRepository.GuardarActualizarDocumento(input.IdDocumento, (int)input.IdSiniestro, input.Nombre, mObjectId, input.TipoMime, input.TipoDocumento, input.SubTipoDocumento, this.InformacionUsuario.Usuario, false);

            return new AgregarActualizarDocumentoSiniestroOutput
            {
                Mensaje = "Operación Exitosa"
            };

        }


    }
}