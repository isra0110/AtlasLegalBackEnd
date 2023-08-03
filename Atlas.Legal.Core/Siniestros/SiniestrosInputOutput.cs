using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Legal
{
    #region Proveedor
    public class SiniestroInput
    {
        public SiniestralidadDto Siniestralidad { get; set; }
        public DictamenesFavorablesDto DictamenesFavorables { get; set; }
        public RecuperacionesDto Recuperaciones { get; set; }
        public HomicidiosYLesionesDto HomicidiosYLesiones { get; set; }
        public VehiculosDetenidosDto VehiculosDetenidos { get; set; }
        public LiberacionVehiculosDto LiberacionVehiculos { get; set; }
        public BitacoraDto Bitacora { get; set; }

        public ICollection<GuardarDocumentosDto> Documentos { get; set; }

        public DemandaDto Demanda { get; set; }
        public int? IdRegExp { get; set; }
        public string ObjectId { get; set; }

    }

    public class SiniestroOutput
    {        
        public string Mensaje { get; set; }
        public int? IdSiniestro { get; set; }
        public bool Exito { get; set; }
    }
    #endregion

    #region Sisa

    public class SiniestroSisaInput
    {
        public SiniestralidadDto Siniestralidad { get; set; }
        public VehiculosDetenidosDto VehiculosDetenidos { get; set; }
        public PagoTercerosDto PagoTerceros { get; set; }
        public DeducibleDto Deducible { get; set; }
        public DictamenesFavorablesDto DictamenesFavorables { get; set; }
        public HomicidiosYLesionesDto HomicidiosYLesiones { get; set; }
        public LiberacionVehiculosDto LiberacionVehiculos { get; set; }
        public RecuperacionesDto Recuperaciones { get; set; }

        public ICollection<GuardarDocumentosDto> Documentos { get; set; }
        public int? IdRegExp { get; set; }
        public string ObjectId { get; set; }

    }

    public class SiniestroSisaOutput
    {
        public string Mensaje { get; set; }
        public int? IdSiniestro { get; set; }
        public bool Exito { get; set; }
    }

    public class SiniestroSISAInput
    {
        public string NumeroReporte { get; set; }
        public int? IdRegExp { get; set; }
        public string ObjectId { get; set; }
    }

    public class RecuperacionesInput
    {
        public string NumeroReporte { get; set; }
        public string Regional { get; set; }
        public double? DeducibleEstimado { get; set; }
        public double? MontoRecuperado { get; set; }
        public double? HonorarioComision { get; set; }
        public double? Iva { get; set; }
        public double? Total { get; set; }
        public string FechaRecuperacionR { get; set; }
        public double? DeducibleValuacion { get; set; }
        public string FolioFactura { get; set; }
        public string NumeroSiniestroAtlas { get; set; }
    }

    public class PagoTercerosInput
    {
        public string NumeroReporte { get; set; }
        public string Regional { get; set; }
        public string Beneficiario { get; set; }
        public string MontoPago { get; set; }
        public string FechaComprobacion { get; set; }
        public string FechaPago { get; set; }
        public string NumeroSiniestroAtlas { get; set; }
    }

    #endregion

    #region Prometeo

    public class SiniestroPrometeoInput
    {
        public SiniestralidadDto Siniestralidad { get; set; }
        public VehiculosDetenidosDto VehiculosDetenidos { get; set; }
        public PagoTercerosDto PagoTerceros { get; set; }
        public DeducibleDto Deducible { get; set; }
        public RecuperacionesDto Recuperaciones { get; set; }
        public DictamenesFavorablesDto DictamenesFavorables { get; set; }
        public LiberacionVehiculosDto LiberacionVehiculos { get; set; }
        public HomicidiosYLesionesDto HomicidiosYLesiones { get; set; }
        public ICollection<GuardarDocumentosDto> Documentos { get; set; }
        public int? IdRegExp { get; set; }
        public string ObjectId { get; set; }
    }

    public class SiniestroPrometeoOutput
    {
        public string Mensaje { get; set; }
        public int? IdSiniestro { get; set; }
        public bool Exito { get; set; }
    }

    #endregion        

    public class BusquedaSolicitudInput
    {
        public int? IdProveedor { get; set; }
        public string NoReporteProveedor { get; set; }
        public DateTime? FechaReporte { get; set; }
        public DateTime? FechaReporteFinal { get; set; }
        public Int32? IdEstatus { get; set; }
        public Int32? IdDelito { get; set; }
        public string NoSiniestroAtlas { get; set; }
        public int? IdPagoRecuperacion { get; set; }
    }

    public class BusquedaSolicitudOutput
    {
        public List<BusquedaSolicitudDto> BusquedaSolicitud { get; set; }

        public BusquedaSolicitudOutput()
        {
            BusquedaSolicitud = new List<BusquedaSolicitudDto>();
        }
    }

    public class DetalleSolicitudInput
    {
        public string NoSiniestroAtlas { get; set; }
        public string NumeroReporteProveedorLegal { get; set; }
    }

    public class DetalleSolicitudOutput
    {
        public List<DetalleSolicitudDto> DetalleSolicitud { get; set; }

        public DetalleSolicitudOutput()
        {
            DetalleSolicitud = new List<DetalleSolicitudDto>();
        }
    }



    public class GuardaBitacoraSolicitudInput
    {
        public string NoSiniestroAtlas { get; set; }
        public string NumeroReporteProveedorLegal { get; set; }
        public string Username { get; set; }
        public int IdModificacionIntegracion { get; set; }
    }

    public class GuardaBitacoraSolicitudOutput
    {
        public string Mensaje { get; set; }
    }

    public class ConsultaBitacoraInput
    {
        public string NoSiniestroAtlas { get; set; }
    }

    public class ConsultaBitacoraOutput
    {
        public List<ConsultaBitacoraDto> Bitacora { get; set; }

        public ConsultaBitacoraOutput()
        {
            Bitacora = new List<ConsultaBitacoraDto>();
        }

        public class ConsultaBitacoraDto
        {
            public string Usuario { get; set; }
            public int? IdSiniestro { get; set; }
            public string NumeroSiniestro { get; set; }
            public string NumeroReporte { get; set; }
            public string Asegurado { get; set; }
            public string ModificacionIntegracion { get; set; }
            public TimeSpan? TiempoEstatus { get; set; }
            public int? Consecutivo { get; set; }
            public DateTime? FechaHoraAcceso { get; set; }
        }
    }

    public class BusquedaReporteInput
    {
        public int? IdProveedor { get; set; }
        public DateTime? FechaReporteInicio { get; set; }
        public DateTime? FechaReporteFin { get; set; }
        public int? IdTipoReporte { get; set; }
    }

    public class ObtenerDocumentoSiniestroInput
    {
        public int IdDocumento { get; set; }
    }

    public class ObtenerDocumentoSiniestroOutput
    {
        public int IdDocumento { get; set; }
        public int IdSiniestro { get; set; }
        public string Nombre { get; set; }
        public string ObjectId { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string TipoMime { get; set; }
        public string UsuarioRegistro { get; set; }
        public string TipoDocumento { get; set; }
        public string SubTipoDocumento { get; set; }

    }

    public class AgregarActualizarDocumentoSiniestroInput
    {
        public int? IdDocumento { get; set; }
        public int IdSiniestro { get; set; }
        public string Nombre { get; set; }
        public string TipoMime { get; set; }
        public string Contenido { get; set; }
        public string TipoDocumento { get; set; }
        public string SubTipoDocumento { get; set; }

    }

    public class AgregarActualizarDocumentoSiniestroOutput
    {
        public string Mensaje { get; set; }
    }
    
    public class DatosNotificacionesInput
    {
        public string NumeroReporte { get; set; }
        public bool Notificacion { get; set; }
    }

    public class DatosNotificacionesOutput
    {
        public string NumeroSiniestro { get; set; }
        public string Entidad { get; set; }
        public string Municipio { get; set; }
        public DateTime FechaApertura { get; set; }
        public string Abogado { get; set; }
        public int? NumeroLesionados { get; set; }
        public int? NumeroFallecidos { get; set; }
        public bool? Notificacion { get; set;  }
        public string NumeroReporte { get; set; }
        public string Asegurado { get; set; }
        public object Documentos { get; set; }
    }

    public class DocumentoNotificaciones
    {
        public int IdDocumento { get; set; }
        public int IdSiniestro { get; set; }
        public string Nombre { get; set; }
        public string TipoMime { get; set; }
        public string Objeto { get; set; }
        public string TipoDocumento { get; set; }
        public string SubTipoDocumento { get; set; }

    }

    public class InformacionAsistenciaNotificacionInput
    {
        public int IdSiniestro { get; set; }
    }

    public class InformacionAsistenciaNotificacionOutput
    {
        public string Cuenta { get; set; }
        public string NumeroReporte { get; set; }
        public string NumeroSiniestroAtlas { get; set; }
        public string Entidad { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string Abogado { get; set; }
        public string Asegurado { get; set; }
        public int? PersonasLesionadas { get; set; }
        public int? NumeroFallecidos { get; set; }
        public int IdProveedor { get; set; }
    }

    public class DatosVehiculosDetenidosAlertaOutput
    {
        public string NumeroReporte { get; set; }
        public DateTime? FechaLiberacion { get; set; }
        public string NumeroSiniestroAtlas { get; set; }
        public int IdProveedor { get; set; }
        public int DiasVencimiento { get; set; }
    }

    public class BitacoraEventosInput
    {
        public string NumeroReporte { get; set; }
        public string JsonEntrada { get; set; }
        public string Respuesta { get; set; }
        public DateTime Fecha { get; set; }
        public int IdModulo { get; set; }
    }
}
