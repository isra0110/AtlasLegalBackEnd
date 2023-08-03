using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Legal
{
    public class ContratosInputOutput
    {
        public class RegistroNuevoContratoOutput
        {   
            public int? IdContrato { get; set; }
            public string FolioContrato { get; set; }
            public int? IdRegExp { get; set; }
            public string Mensaje { get; set; }
            public int? CodigoMensaje { get; set; }
        }

        public class RegistroNuevoContratoInput
        {
            public string NumeroContrato { get; set; }
            public DateTime FechaCreacion { get; set; }
            public bool NuevaSolicitud { get; set; }
            public bool PerteneceContrato { get; set; }
            public string CorreoSolicitante { get; set; }
            public string NoExtension { get; set; }
            public int IdArea { get; set; }

            public bool AgenteSeguros { get; set; }
            public string NombreAgente { get; set; }
            public string ClaveAgente { get; set; }
            public int IdEstatus { get; set; }
            public string Rfc { get; set; }
            public string Calle { get; set; }
            public string NoExterior { get; set; }
            public string NoInterior { get; set; }
            public string CodigoPostal { get; set; }
            public string Colonia { get; set; }
            public string Municipio { get; set; }
            public string Estado { get; set; }
            public string Poblacion { get; set; }
            public string TipoCedula { get; set; }
            public string Cedula { get; set; }
            public DateTime FechaVtoCedula { get; set; }
            public string ClaveAgenteB { get; set; }
            public string NombreAgenteB { get; set; }

            public int IdTipoPersona { get; set; }
            public string PrimerNombre { get; set; }
            public string SegundoNombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public string RazonSocial { get; set; }
            public string Representante { get; set; }
            public string RfcPersona { get; set; }
            public string Curp { get; set; }
            public int IdPais { get; set; }
            public int IdEntidadFederativa { get; set; }
            public string MunicipioPersona { get; set; }
            public string ColoniaPersona { get; set; }
            public string CalleNumero { get; set; }
            public string CodigoPostalPersona { get; set; }
            public string Telefono { get; set; }
            public string CorreoElectronico { get; set; }
            public string Descripcion { get; set; }
            public int? IdRegExp { get; set; }

            public List<GuardarDocumentoContratosInput> Documentos { get; set; }

            public int? IdTipoContrato { get; set; }
            public int? IdEspecifiqueCualPrestador { get; set; }
            public string DefineOtros { get; set; }
            public string DescripcionOtros { get; set; }
            public bool? ActaConstitutiva { get; set; }
            public bool? CartaPoder { get; set; }
            public bool? ComprobanteDomicilio { get; set; }
            public bool? ConstanciaFiscal { get; set; }
            public bool? CotizacionServicios { get; set; }
            public bool? CurriculumEmpresarial { get; set; }
            public bool? CuentaBancario { get; set; }
            public bool? FichaTecnica { get; set; }
            public bool? Identificacion { get; set; }
            public bool? IdentificacionFuncionario { get; set; }
            public bool? IdentificacionRepresentante { get; set; }
            public bool? OficioContestar { get; set; }
            public bool? PoderNombramiento { get; set; }
            public bool? PrestacionServicios { get; set; }
            public bool? Solicitud { get; set; }
            public bool? FichaTecnicaContratos { get; set; }
            public bool? ExpedienteInterno { get; set; }
            public bool? EscriturasInmueble { get; set; }
            public bool? UsoSuelo { get; set; }
            public bool? Planos { get; set; }
            public bool? PredialInmueble { get; set; }
            public bool? Subarrendamiento { get; set; }
            public bool? ContratoArrendamiento { get; set; }
            public bool? DocumentoAnalisis { get; set; }
            public bool? CaratulaPoliza { get; set; }
            public bool? PropuestaComercial { get; set; }
            public bool? FlujoOperacion { get; set; }
            public bool? Licitacion { get; set; }
            public bool? JuntasAclaraciones { get; set; }
            public bool? ModeloContrato { get; set; }
            public bool? DocumentoRelacionados { get; set; }
            public bool? Cv { get; set; }
            public bool? PropuestaServicios { get; set; }
            public bool? PlanContingencia { get; set; }
            public bool? SiniestrosGastosMedicos { get; set; }
            public bool? LicenciaSanitaria { get; set; }
            public bool? AvisoFuncionamiento { get; set; }
            public bool? Otros { get; set; }

        }


        public class GuardarDocumentoContratosInput
        {
            public int? IdDocumento { get; set; }
            public int? IdContrato { get; set; }
            public string TipoMime { get; set; }
            public string TipoDocumento { get; set; }
            public string SubTipoDocumento { get; set; }
            public string Nombre { get; set; }
            public string ObjectId { get; set; }
            public string Contenido { get; set; }
            public DateTime? FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public bool Borrar { get; set; }
        }

        public class GuardarDocumentoContratosOutput
        {
            public string Mensaje { get; set; }
        }

        public class ConsultaContratoInput
        {
            public string NoContrato { get; set; }
            public string Rfc { get; set; }
            public string RazonSocial { get; set; }
            public string ClaveAgente { get; set; }
        }

        public class ConsultaContratoOutput
        {
            public List<ContratoDTO> Contrato { get; set; }

            public ConsultaContratoOutput()
            {
                Contrato = new List<ContratoDTO>();
            }
            public class ContratoDTO
            {
                public string NoContrato { get; set; }
                public string Rfc { get; set; }
                public string RazonSocial { get; set; }
                public string ClaveAgente { get; set; }
                public string Agente { get; set; }
                public string Area { get; set; }
                public string TipoContrato { get; set; }
                public DateTime? FechaCreacion { get; set; }
            }
        }

        public class ObtenerObjectIdContratoInput
        {
            public int IdDocumento { get; set; }
        }

        public class ObtenerIdRegExpContratoOutput
        {
            public int? IdRegExp { get; set; }
            public string NumeroContrato { get; set; }
        }

        public class ConsultaSolicitanteInput
        {
            public string NoContrato { get; set; }
            public int IdArea { get; set; }
            public int IdEstatus { get; set; }
            public DateTime FechaRegistroInicio { get; set; }
            public DateTime FechaRegistroFin { get; set; }
        }

        public class ConsultaSolicitanteOutput
        {
            public List<SolicitanteDTO> Solicitante { get; set; }

            public ConsultaSolicitanteOutput()
            {
                Solicitante = new List<SolicitanteDTO>();
            }

            public class SolicitanteDTO
            {
                public string NoContrato { get; set; }
                public string Solicitante { get; set; }
                public string Area { get; set; }
                public string Estatus { get; set; }
                public string DetalleEstatus { get; set; }
                public DateTime? FechaSolicitud { get; set; }
            }
        }

        public class ConsultaAbogadoInput
        {
            public string NoContrato { get; set; }
            public int IdArea { get; set; }
            public int IdEstatus { get; set; }
            public DateTime FechaRegistroInicio { get; set; }
            public DateTime FechaRegistroFin { get; set; }
        }

        public class ConsultaAbogadoOutput
        {
            public List<AbogadoDTO> Abogado { get; set; }

            public ConsultaAbogadoOutput()
            {
                Abogado = new List<AbogadoDTO>();
            }

            public class AbogadoDTO
            {
                public string NoContrato { get; set; }
                public string Solicitante { get; set; }
                public string Area { get; set; }
                public string Estatus { get; set; }
                public string DetalleEstatus { get; set; }
                public DateTime? FechaSolicitud { get; set; }
            }
        }

        public class DetalleContratoInput
        {
            public string NoFolio { get; set; }
        }

        public class DetalleContratoOutput
        {
            public string NumeroContrato { get; set; }
            public bool? NuevaSolicitud { get; set; }
            public bool? PerteneceContrato { get; set; }
            public string CorreoSolicitante { get; set; }
            public string NumeroExtension { get; set; }
            public bool? AgenteSeguros { get; set; }
            public string NombreAgente { get; set; }
            public string ClaveAgente { get; set; }
            public string Rfc { get; set; }
            public string Calle { get; set; }
            public string NoExterior { get; set; }
            public string NoInterior { get; set; }
            public string Colonia { get; set; }
            public string Municipio { get; set; }
            public string Estado { get; set; }
            public string Poblacion { get; set; }
            public string TipoCedula { get; set; }
            public string Cedula { get; set; }
            public DateTime? FechaVtoCedula { get; set; }
            public string ClaveAgenteB { get; set; }
            public string NombreAgenteB { get; set; }
            public string Cp { get; set; }
            public int? IdRegExp { get; set; }
            public DateTime? FechaCreacion { get; set; }
            public int? IdArea { get; set; }
            public int? IdEstatus { get; set; }
            public object DefinicionContratos { get; set; }
            public object Persona { get; set; }
            public object Documentos { get; set; }
        }

        public class ComentarioContratoInput
        {
            public string NoFolio { get; set; }
        }

        public class ComentarioContratoOutput
        {
            public int? IdDemandaComentario { get; set; }
            public string NumeroContrato { get; set; }
            public DateTime? FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Comentario { get; set; }
        }

        public class GuardarComentarioContratoInput
        {
            public string NoFolio { get; set; }
            public DateTime FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Comentario { get; set; }
        }

        public class GuardarComentarioContratoOutput
        {
            public string Mensaje { get; set; }
        }

        public class GuardarTrackingContratoInput
        {
            public string NoFolio { get; set; }
            public DateTime FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Movimiento { get; set; }
        }

        public class ObtenerTrackingContratoInput
        {
            public string NoFolio { get; set; }
        }

        public class ObtenerTrackingContratoOutput
        {
            public List<TrackingDto> Trackings { get; set; }

            public ObtenerTrackingContratoOutput()
            {
                Trackings = new List<TrackingDto>();
            }

            public class TrackingDto
            {
                public int? IdDemandaTracking { get; set; }
                public string NumeroContrato { get; set; }
                public DateTime? FechaRegistro { get; set; }
                public string UsuarioRegistro { get; set; }
                public string Movimiento { get; set; }
            }
        }

        public class ReporteGeneralContratoInput
        {
            public int? IdTipoReporte { get; set; }
            public string NoFolio { get; set; }
            public int? IdArea { get; set; }
            public int? IdEstatus { get; set; }
            public int? IdPersona { get; set; }
            public string Rfc { get; set; }
            public DateTime? FechaRegistroInicio { get; set; }
            public DateTime? FechaRegistroFin { get; set; }
        }

        public class ReporteGeneralContratoOutput
        {
            public List<ReporteGeneralDto> Reportes { get; set; }

            public ReporteGeneralContratoOutput()
            {
                Reportes = new List<ReporteGeneralDto>();
            }

            public class ReporteGeneralDto
            {
                public int? ExpedienteProvieneDe { get; set; }
                public string Estatus { get; set; }
                public string DetalleEstatus { get; set; }
                public string Solicitante { get; set; }
                public string CorreoSolicitante { get; set; }
                public string NumeroExtencion { get; set; }
                public string Area { get; set; }
                public string NombreAgente { get; set; }
                public string ClaveAgente { get; set; }
                public string TipoPersona { get; set; }
                public string Rfc { get; set; }
                public string TipoContrato { get; set; }
                public string EspecifiqueCual { get; set; }
                public string RevisionProveedorEsteno { get; set; }
                public string Descripcion { get; set; }                
            }
        }

        public class ReportePrestadoresInput
        {
            public int? IdTipoReporte { get; set; }
            public string NoFolio { get; set; }
            public int? IdArea { get; set; }
            public int? IdEstatus { get; set; }
            public int? IdPersona { get; set; }
            public string Rfc { get; set; }
            public DateTime? FechaRegistroInicio { get; set; }
            public DateTime? FechaRegistroFin { get; set; }
        }

        public class ReportePrestadoresOutput
        {
            public List<ReportePrestadoresDto> Prestadores { get; set; }

            public ReportePrestadoresOutput()
            {
                Prestadores = new List<ReportePrestadoresDto>();
            }

            public class ReportePrestadoresDto
            {
                public string TipoServiciosTerceros { get; set; }
                public DateTime? FechaAcuerdo { get; set; }
                public DateTime? FechaInicio { get; set; }
                public DateTime? FechaBaja { get; set; }
                public string DescripcionOtrosServiciosTerceros { get; set; }
                public string OficioAutoridadOtrosServiciosTerceros { get; set; }
                public DateTime? FechaAutoridadOtrosServiciosTerceros { get; set; }
                public string NumeroContrato { get; set; }
                public string TipoPersona { get; set; }
                public string PrimerNombre { get; set; }
                public string SegundoNombre { get; set; }                
                public string APaterno { get; set; }
                public string AMaterno { get; set; }
                public string Curp { get; set; }
                public string RazonSocial { get; set; }
                public string Calle { get; set; }
                public string Pais { get; set; }
                public string CalleNumero { get; set; }
                public string Colonia { get; set; }
                public string EntidadFederativa { get; set; }
                public string Municipio { get; set; }
                public string Cp { get; set; }
                public string Telefono { get; set; }
                public string Correo { get; set; }
            }
        }
    }
}
