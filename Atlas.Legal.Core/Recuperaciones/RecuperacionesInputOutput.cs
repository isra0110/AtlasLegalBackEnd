using System;
using System.Collections.Generic;

namespace Atlas.Legal
{
    public class BuscarSiniestroRecuperacionesInput
    {
        public int? IdTipoRecuperacion { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
        public string NumeroSiniestro { get; set; }
    }

    public class BuscarSiniestroRecuperacionesOutput : BaseOutput
    {
        public List<DetalleModel> ConsultaSiniestro { get; set; }

        public BuscarSiniestroRecuperacionesOutput()
        {
            ConsultaSiniestro = new List<DetalleModel>();
        }

        public class DetalleModel
        {
            public string NumeroSiniestro { get; set; }
            public string OrdenTrabajo { get; set; }
            public DateTime? FechaOcurrido { get; set; }
            public string Analista { get; set; }
            public string Moneda { get; set; }
            public string NombreAsegurado { get; set; }
            public string Estimacion { get; set; }
            public string Ajustador { get; set; }
            public string Estado { get; set; }
            public string Gastos { get; set; }
            public string PagoSiniestro { get; set; }
            public DateTime? FechaPagoSiniestro { get; set; }
            public bool? AperturaRechazar { get; set; }
            public string VerDetalleBase64 { get; set; }
        }
    }

    public class RegistroDemandaRecuperacionesInput
    {
        public int? IdTipoRecuperacion { get; set; }
        public SiniestroSeleccionadoModel SiniestroSeleccionado { get; set; }
        public string Subrogado { get; set; }
        public DateTime? FechaRecepcionExpediente { get; set; }
        public string Subrrogacion { get; set; }
        public int? Preinscripcion { get; set; }
        public string Antiguedad { get; set; }
        public string Pendiente { get; set; }
        public int? TiempoTurno { get; set; }
        public string Gastos { get; set; }
        public string Recordatorio { get; set; }
        public int? IdCausa { get; set; }
        public int? IdTerceroInvolucrado { get; set; }
        public int? IdEstatus { get; set; }
        public string Observaciones { get; set; }
        public bool? EsDemanda { get; set; }
        public DateTime? FechaPagoSiniestro { get; set; }
        public string PagoSiniestro { get; set; }
        public string Actualizacion { get; set; }
        public string Comentarios { get; set; }
        public string IdAbogado { get; set; }
        public List<DocumentoModel> Documentos { get; set; }

        public string NumDemanda { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? IdRegExp { get; set; }

        public class SiniestroSeleccionadoModel
        {
            public int Id { get; set; }
            public string Siniestro { get; set; }
            public string OrdenTrabajo { get; set; }
            public string NombreAsegurado { get; set; }
            public DateTime? FechaOcurrido { get; set; }
            public string Analista { get; set; }
            public string Moneda { get; set; }
            public int? IdEstado { get; set; }
            public string Recuperacion { get; set; }
            public string Poliza { get; set; }
            public string Vehiculo { get; set; }
            public int? IdCausab { get; set; }
            public string Tercero { get; set; }
            public DateTime? FechaIngreso { get; set; }
            public string Honorarios { get; set; }
        }
    }

    public class ObtenerDocumentoRecuperacionesInput
    {
        public int IdDocumento { get; set; }
    }

    public class ObtenerDocumentoRecuperacionesOutput
    {
        public int IdDocumento { get; set; }
        public int IdDemanda { get; set; }
        public string Nombre { get; set; }
        public string ObjectId { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string TipoMime { get; set; }
        public string UsuarioRegistro { get; set; }
        public string TipoDocumento { get; set; }
        public string SubTipoDocumento { get; set; }
    }

    public class RegistroDemandaRecuperacionesOutput : BaseOutput
    {
        public int? Id { get; set; }
        public int? IdRegExp { get; set; }
        public string NumeroDemanda { get; set; }
    }

    public class ObtenerObjectIdRecuperacionesInput
    {
        public int IdDocumento { get; set; }
    }

    public class ObtenerIdRegExpRecuperacionesOutput
    {
        public int? IdRegExp { get; set; }
        public string NumDemanda { get; set; }
    }

    public class RegistroDemandaRecuperacionesDocumentoInput
    {
        public int? IdDocumento { get; set; }
        public int? IdDemanda { get; set; }
        public string Nombre { get; set; }
        public string Objeto { get; set; }
        public string Contenido { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string SubTipoDocumento { get; set; }
        public string TipoMime { get; set; }
        public string TipoDocumento { get; set; }
        public bool? Borrar { get; set; }

        //Datos Para notificacion
        public string NumeroSiniestro { get; set; }
        public string Asegurado { get; set; }
        public string Ramo { get; set; }
    }

    public class RegistroDocumentoRecuperacionesOutput
    {
        public string Mensaje { get; set; }
    }

    public class RegistroDemandaRecuperacionesDocumentoOutput : BaseOutput
    {
        public int? IdDocumento { get; set; }
    }

    public class AsignarAbogadoRecuperacionesInput
    {
        public List<AsignacionModel> AsignarAbogado { get; set; }

        public class AsignacionModel
        {
            public int? IdDemanda { get; set; }
            public string NumeroDemanda { get; set; }
            public bool? TipoAsignacion { get; set; }
            public int? IdProveedor { get; set; }
            public string Recuperacion { get; set; }
            public int? IdTurnadoDespacho { get; set; }
            public DateTime? FechaIngreso { get; set; }
            public DateTime? FechaCreacion { get; set; }
            
            //Datos Para notificacion
            public string NumeroSiniestro { get; set; }
            public string Asegurado { get; set; }
            public string Ramo { get; set; }
        }
    }

    public class AsignarAbogadoRecuperacionesOutput : BaseOutput
    {
        public int? Id { get; set; }
        public string NumeroDemanda { get; set; }
    }

    public class ConsultaRecuperacionInput
    {
        public int? IdTipoRecuperacion { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
        public string NumeroSiniestro { get; set; }
        public int? IdProveedor { get; set; }
        public string Asegurado { get; set; }
    }

    public class ConsultaRecuperacionOutput : BaseOutput
    {
        public List<DetalleModel> Detalles { get; set; }

        public ConsultaRecuperacionOutput()
        {
            Detalles = new List<DetalleModel>();
        }

        public class DetalleModel
        {
            public int? IdDemanda { get; set; }
            public string NumeroSiniestro { get; set; }
            public string Asegurado { get; set; }
            public DateTime? FechaOcurrido { get; set; }
            public int? Preinscripcion { get; set; }
            public string Abogado { get; set; }
            public string Recuperacion { get; set; }
            public DateTime? FechaRecuperacion { get; set; }
            public bool? TipoDemanda { get; set; }
            public string NumDemanda { get; set; }
        }
    }

    public class GuardaTrackingRecuperacionesInput
    {
        public string NumDemanda { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Movimiento { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class ObtenerTrackingsRecuperacionesOutput
    {
        public List<TrackingDto> Trackings { get; set; }

        public ObtenerTrackingsRecuperacionesOutput()
        {
            Trackings = new List<TrackingDto>();
        }

        public class TrackingDto
        {
            public int IdDemandaTracking { get; set; }
            public string NumDemanda { get; set; }
            public DateTime FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Movimiento { get; set; }
        }
    }

    public class ObtenerTrackingsRecuperacionesInput
    {
        public string NumDemanda { get; set; }
    }

    public class GuardarComentarioRecuperacionesInput
    {
        public string NumDemanda { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Comentario { get; set; }
    }

    public class GuardarComentarioRecuperacionesOutput
    {
        public string Mensaje { get; set; }
    }

    public class ObtenerComentariosRecuperacionesInput
    {
        public string NumDemanda { get; set; }
    }
    
    public class ObtenerComentariosRecuperacionesOutput
    {
        public List<ComentarioDto> Comentarios { get; set; }

        public ObtenerComentariosRecuperacionesOutput()
        {
            Comentarios = new List<ComentarioDto>();
        }

        public class ComentarioDto
        {
            public int IdDemandaComentario { get; set; }
            public string NumDemanda { get; set; }
            public DateTime FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Comentario { get; set; }
        }
    }

    public class DetalleRecuperacionInput
    {
        public string IdDemanda { get; set; }
    }

    public class DetalleRecuperacionOutput : BaseOutput
    {
        public List<DetalleModel> Detalles { get; set; }

        public DetalleRecuperacionOutput()
        {
            Detalles = new List<DetalleModel>();
        }

        public class DetalleModel
        {
            public int? IdDemanda { get; set; }
            public string TipoRecuperacion { get; set; }
            public Object SiniestroSeleccionado { get; set; }
            public string Subrogado { get; set; }
            public DateTime? FechaRecepcionExpediente { get; set; }
            public int? Preinscripcion { get; set; }
            public string Antiguedad { get; set; }
            public string Pendiente { get; set; }
            public int? TiempoTurno { get; set; }
            public string Gastos { get; set; }
            public string Recordatorio { get; set; }
            public string Causa { get; set; }
            public int? IdTerceroInvolucrado { get; set; }
            public string Estatus { get; set; }
            public int? EsDemanda { get; set; }
            public DateTime? FechaPagoSiniestro { get; set; }
            public string PagoSiniestro { get; set; }
            public string Actualizacion { get; set; }
            public string Observaciones { get; set; }
            public string Abogado { get; set; }
            public string Comentarios { get; set; }
            public object Documentos { get; set; }
            public object Asignacion { get; set; }
            public bool? EsConcluido { get; set; }
        }
    }

    public class ConcluirRecuperacionInput
    {
        public string IdDemanda { get; set; }
        public bool Concluir { get; set; }
    }

    public class ConcluirRecuperacionOutput : BaseOutput
    {
        public string IdDemanda { get; set; }
    }

    public class AperturarRechazarSiniestroRecuperacionesInput
    {
        public string NumeroSiniestro { get; set; }
        public bool AperturarRechazar { get; set; }
    }

    public class AperturarRechazarSiniestroRecuperacionesOut : BaseOutput
    {
        public string Siniestro { get; set; }
    }

    public class ReporteRecuperacionTransporteInput
    {
        public int? IdTipoRecuperacion { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
        public int? IdProveedor { get; set; }
    }

    public class ReporteRecuperacionAutosInput
    {
        public int? IdTipoRecuperacion { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
        public int? IdProveedor { get; set; }
    }

    public class ReporteRecuperacionDiversosInput
    {
        public int? IdTipoRecuperacion { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
        public int? IdProveedor { get; set; }
    }

}
