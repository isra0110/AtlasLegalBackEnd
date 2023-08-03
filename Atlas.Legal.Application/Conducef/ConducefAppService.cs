using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.UI;
using Atlas.Legal.AtlasWS;
using Atlas.Legal.Autorizacion;
using Atlas.Legal.Content;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Atlas.Legal.AtlasWS.AtlasWSOutput;

namespace Atlas.Legal
{
    public class ConducefAppService : LegalAppServiceBase
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IContentRepository _contentRepository;
        private readonly IConducefRepository _conducefRepository;
        private readonly IAtlasWSRepository _atlasRepository;

        public ConducefAppService(
            IHttpContextAccessor httpContextAccessor,
            IRepository<OpeUsuario, int> opeUsuarioRepository,
            IObjectMapper objectMapper,
            IContentRepository contentRepository,
            IAtlasWSRepository atlasWSRepository,
            IConducefRepository conducefRepository
            ) : base(httpContextAccessor, opeUsuarioRepository)
        {
            _objectMapper = objectMapper;
            _contentRepository = contentRepository;
            _conducefRepository = conducefRepository;
            _atlasRepository = atlasWSRepository;
        }

        public async Task<ConsultaConducefOutput> ConsultaConducef(ConsultaConducefInput input)
        {
            var result = await _conducefRepository.ConsultaConducef(input);

            return result;
        }

        public async Task<DetalleConducefOutput> DetalleConducef(DetalleConducefInput input)
        {
            var result = await _conducefRepository.DetalleConducef(input);

            return result;
        }

        public async Task<ConducefReporteGeneralOutput> ConducefReporteGeneral(ConducefReporteGeneralInput input)
        {
            var result = await _conducefRepository.ConducefReporteGeneral(input);

            return result;
        }

        public async Task<ConducefReporteTrimestralOutput> ConducefReporteTrimestral(ConducefReporteTrimestralInput input)
        {
            var result = await _conducefRepository.ConducefReporteTrimestral(input);
            return result;
        }

        public async Task<RegistroConducefOutput> RegistroConducef(RegistroConducefInput input)
        {
            string msj = "";
            if (input.IdConducef == null)
                msj = "Se creó un nuevo registro";
            else
                msj = "Se actualizo registro";

            var result = await _conducefRepository.RegistroConducef(input);

            input.NumeroConducef = result.NumeroConducef;
            input.IdConducef = result.Id;

            if (input.Siniestros != null)
            {
                foreach (var s in input.Siniestros)
                {
                    _conducefRepository.GuardarSiniestroConducef(new GuardarSiniestroConducefInput
                    {
                        IdSiniestro = s.IdSiniestro,
                        IdRamoBusqueda = s.IdRamoBusqueda,
                        Poliza = s.Poliza,
                        Siniestro = s.Siniestro,
                        NombreAsegurado = s.NombreAsegurado,
                        ClaveAgente = s.ClaveAgente,
                        NombreAgente = s.NombreAgente,
                        FechaOcurrido = s.FechaOcurrido,
                        HonorariosPagados = s.HonorariosPagados,
                        FechaHonorariosPagados = s.FechaHonorariosPagados,
                        OtrosGastos = s.OtrosGastos,
                        NumeroConducef = input.NumeroConducef
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
                        ExpedienteLlave = input.NumeroConducef,
                        Folder = $"conducef/{input.NumeroConducef}",
                        atributos = mAtributos,
                        NodeType = nodeType,
                        EsGuardado = true
                    };

                    _contentRepository.GuardarDocumento(mGuardarDocumento);
                    input.IdRegExp = mGuardarDocumento.IdRegExp;
                    item.ObjectId = mGuardarDocumento.ObjectId;

                    _conducefRepository.GuardarDocumentoConducef(new GuardarDocumentoConducefInput
                    {
                        IdDocumento = item.IdDocumento,
                        IdConducef = result.Id,
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

            result = await _conducefRepository.RegistroConducef(input);

            if (input.Observaciones != null)
            {
                foreach (var o in input.Observaciones)
                {
                    _conducefRepository.GuardarObservacionConducef(new GuardarObservacionConducefInput
                    {
                        FechaCreacion = DateTime.Now,
                        RealizadoPor = this.InformacionUsuario.Usuario,
                        NumeroConducef = result.NumeroConducef,
                        Comentario = o.Comentario
                    });
                }
            }
            _conducefRepository.GuardarTrackingConducef(new GuardarTrackingConducefInput
            {
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                FechaRegistro = DateTime.Now,
                NumeroConducef = result.NumeroConducef,
                Movimiento = msj
            });

            return result;
        }

        public async Task<GuardarSiniestroConducefOutput> AgregarSiniestroConducef(GuardarSiniestroConducefInput input)
        {
            _conducefRepository.GuardarSiniestroConducef(input);

            _conducefRepository.GuardarTrackingConducef(new GuardarTrackingConducefInput
            {
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                Movimiento = $"Se agregó siniestro al registro con numero conducef '{input.NumeroConducef}'",
                NumeroConducef = input.NumeroConducef
            });

            return new GuardarSiniestroConducefOutput
            {
                Mensaje = "Operacion Exitosa"
            };
        }

        public async Task<GuardarSiniestroConducefOutput> AgregarSiniestroSisa(SiniestrosSISAInput input)
        {            
            //Registro de Siniestros
            if(input.siniestrosSISA.Count != 0)
            {
                foreach (var siniestro in input.siniestrosSISA)
                {
                    var siniestroCondusef = new GuardarSiniestroConducefInput
                    {
                        IdRamoBusqueda = siniestro.IdRamoBusqueda,
                        Poliza = siniestro.Poliza,
                        Siniestro = siniestro.Siniestro,
                        NombreAsegurado = siniestro.NombreAsegurado,
                        ClaveAgente = siniestro.ClaveAgente,
                        NombreAgente = siniestro.NombreAgente,
                        FechaOcurrido = Convert.ToDateTime(siniestro.FechaOcurrido),
                        HonorariosPagados = siniestro.HonorariosPagados,
                        FechaHonorariosPagados = Convert.ToDateTime(siniestro.FechaPagoHonorarios),
                        OtrosGastos = siniestro.OtrosGastos,
                        NumeroConducef = siniestro.NumeroConducef
                    };

                    _conducefRepository.GuardarSiniestroSisa(siniestroCondusef);

                    _conducefRepository.GuardarTrackingConducef(new GuardarTrackingConducefInput
                    {
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = this.InformacionUsuario.Usuario,
                        Movimiento = $"Se agregó siniestro al registro con numero conducef '{siniestro.NumeroConducef}'",
                        NumeroConducef = siniestro.NumeroConducef
                    });
                }
            }

            return new GuardarSiniestroConducefOutput
            {
                Mensaje = "Operacion Exitosa"
            };

        }

        public async Task<AsignarAbogadoConducefOutput> AsignarAbogadoConducef(AsignarAbogadoConducefInput input)
        {
            _conducefRepository.AsignarAbogadoConducef(input);

            _conducefRepository.GuardarTrackingConducef(new GuardarTrackingConducefInput
            {
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                NumeroConducef = input.NumeroConducef,
                Movimiento = $"Se asignó abogado al registro con numero '{input.NumeroConducef}'"
            });

            return new AsignarAbogadoConducefOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public async Task<ConcluirRegistroConducefOutput> ConcluirRegistroConducef(ConcluirRegistroConducefInput input)
        {
            _conducefRepository.ConcluirRegistroConducef(input);

            _conducefRepository.GuardarTrackingConducef(new GuardarTrackingConducefInput
            {
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = this.InformacionUsuario.Usuario,
                NumeroConducef = input.NumeroConducef,
                Movimiento = $"Se concluyó registro con número '{input.NumeroConducef}'"
            });

            return new ConcluirRegistroConducefOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public async Task<GuardarDocumentoConducefOutput> GuardarDocumentoConducef(GuardarDocumentoConducefInput input)
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
                string objectId = _conducefRepository.ObtenerObjectIdConducef(new ObtenerObjectIdConducefInput { IdDocumento = (int)input.IdDocumento });
                input.ObjectId = objectId;
                _contentRepository.EliminarDocumento(input.ObjectId);
                _conducefRepository.GuardarDocumentoConducef(input);
            }
            else
            {
                if (input.IdDocumento != null)
                {
                    string objectId = _conducefRepository.ObtenerObjectIdConducef(new ObtenerObjectIdConducefInput { IdDocumento = (int)input.IdDocumento });
                    _contentRepository.EliminarDocumento(objectId);
                }

                var mConsulta = _conducefRepository.ObtenerIdRegExpConducef((int)input.IdConducef);

                var mGuardarDocumento = new GuardarDocumentoInput
                {
                    IdRegExp = mConsulta.IdRegExp,
                    IdTipoDocumental = idTipoDoc,
                    Nombre = input.Nombre,
                    Extension = input.TipoMime,
                    Base64 = input.Contenido,
                    ExpedienteLlave = mConsulta.NumeroConducef,
                    Folder = $"conducef/{mConsulta.NumeroConducef}",
                    atributos = mAtributos,
                    NodeType = nodeType,
                    EsGuardado = true
                };

                _contentRepository.GuardarDocumento(mGuardarDocumento);
                input.ObjectId = mGuardarDocumento.ObjectId;
                input.FechaRegistro = DateTime.Now;

                _conducefRepository.GuardarDocumentoConducef(input);
            }

            return new GuardarDocumentoConducefOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public async Task<ObtenerDocumentoConducefOutput> ObtenerDocumentoConducef(ObtenerDocumentoConducefInput input)
        {
            var result = await _conducefRepository.ObtenerDocumentoConducef(input);
            _contentRepository.ObtenerDocumento(result.ObjectId, out string contenido);

            result.Contenido = contenido;

            return result;

        }

        public async Task<GuardarComentarioConducefOutput> GuardarComentarioConducef(GuardarComentarioConducefInput input)
        {
            _conducefRepository.GuardarComentarioConducef(new GuardarComentarioConducefInput
            {               
                Comentario = input.Comentario,
                FechaRegistro = DateTime.Now,
                NumeroConducef = input.NumeroConducef,
                UsuarioRegistro = this.InformacionUsuario.Usuario
            });

            return new GuardarComentarioConducefOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public async Task<GuardarObservacionConducefOutput> GuardarActualizarObservacionConducef(GuardarObservacionConducefInput input)
        {
            _conducefRepository.GuardarObservacionConducef(new GuardarObservacionConducefInput
            {
                Comentario = input.Comentario,
                FechaCreacion = DateTime.Now,
                IdObservacion = input.IdObservacion,
                NumeroConducef = input.NumeroConducef,
                RealizadoPor = this.InformacionUsuario.Usuario
            });

            return new GuardarObservacionConducefOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public async Task<ObtenerTrackingsConducefOutput> ObtenerTrackingsConducef(ObtenerTrackingsConducefInput input)
        {
            var result = await _conducefRepository.ObtenerTrackingsConducef(input);

            result.Trackings = result.Trackings.Where(s => s.NumeroConducef == input.NumeroConducef).ToList();

            return result;
        }

        public async Task<ObtenerComentariosConducefOutput> ObtenerComentariosConducef(ObtenerComentariosConducefInput input)
        {
            var result = await _conducefRepository.ObtenerComentariosConducef(input);

            result.Comentarios = result.Comentarios.Where(c => c.NumeroConducef == input.NumeroConducef).ToList();

            return result;
        }

        public async Task<ObtenerObservacionesConducefOutput> ObtenerObservacionesConducef(ObtenerObservacionesConducefInput input)
        {
            var result = await _conducefRepository.ObtenerObservacionesConducef(input);

            result.Observaciones = result.Observaciones.Where(o => o.NumeroConducef == input.NumeroConducef).ToList();

            return result;
        }

        public async Task<SiniestroAtlasOutput> ObtenerSiniestroSisa(SiniestroAtlasInput input)
        {
            var result = await _atlasRepository.ConsultaSiniestros(input);

            return result;
        }
    }
}
