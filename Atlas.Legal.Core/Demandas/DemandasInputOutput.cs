using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Legal
{
    public class GuardarActualizarRegistroDemandaInput
    {
        public string IdDemanda { get; set; }
        public int IdTipoMateria { get; set; }
        public string EspecifiqueMateria { get; set; }
        public string Actor { get; set; }
        public int IdNotificacionCalidad { get; set; }
        public string ExpedienteJuicio { get; set; }
        public string Coodemandados { get; set; }
        public int TerminoContestacion { get; set; }
        public DateTime FechaVencimientoContestacion { get; set; }
        public string Juzgado { get; set; }
        public int IdTipoJuicio { get; set; }
        public string EspecifiqueCual { get; set; }
        public string MotivoDemanda { get; set; }
        public bool? JuicioRelevante { get; set; }
        public string NombreJuzgado { get; set; }
        public int IdUbicacionJuicio { get; set; }
        public string MontoReclamado { get; set; }
        public string MontoReclamadoSinDeterminar { get; set; }
        public string AutoridadJudicial { get; set; }
        public SiniestroSeleccionadoDto SiniestroSeleccionado { get; set; }
        public bool ContestacionDeDemanda { get; set; }
        public int? IdRegExp { get; set; }

    }

    public class GuardarActualizarRegistroDemandaOutput
    {
        public string NumDemanda { get; set; }
        public string Mensaje { get; set; }
    }

    public class AsignarActualizarAbogadoInput
    {
        public int? IdAsigna { get; set; }
        public string IdDemanda { get; set; }
        public bool? NotificacionEmplazamiento { get; set; }
        public DateTime? FechaEmplazamiento { get; set; }
        public int? IdAbogadoDesignadoInvolucrado { get; set; }
        public int? IdAbogadoDesignadoAtlas { get; set; }
        public int? IdTipoInvolucrado { get; set; }
        public DatosRegistroNotificacion DetalleRegistro { get; set; }
    }

    public class AsignarActualizarAbogadoOutput
    {
        public string Mensaje { get; set; }
    }

    public class EliminarAsignacionInput
    {
        public string IdDemanda { get; set; }
        public int IdAsigna { get; set; }
    }

    public class EliminarAsignacionOutput
    {
        
        public string Mensaje { get; set; }
    }

    public class ConsultarRegistroDemandaInput
    {
        public string NumeroSiniestro { get; set; }
        public string ExpedienteJuicio { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
    }

    public class ConsultaRegistroDemandaOutput
    {
        public List<ConsultaDemandaDto> ConsultaDemanda { get; set; }

        public ConsultaRegistroDemandaOutput()
        {
            ConsultaDemanda = new List<ConsultaDemandaDto>();
        }
    }

    public class GuardarComentarioInput
    {
        public string IdDemanda { get; set; }        
        public string Comentario { get; set; }
    }

    public class GuardarComentarioOutput
    {
        public string Mensaje { get; set; }
    }

    public class ConsultaDemandaTrackingInput
    {
        public string IdDemanda { get; set; }
    }

    public class ConsultaDemandaTrackingOutput
    {
        public List<ConsultaTrackingDto> Trackings { get; set; }

        public ConsultaDemandaTrackingOutput()
        {
            Trackings = new List<ConsultaTrackingDto>();
        }
    }

    public class ConsultaDemandaComentarioInput
    {
        public string IdDemanda { get; set; }
    }

    public class ConsultaDemandaComentarioOutput
    {
        public List<ConsultarComentarioDto> Comentarios { get; set; }

        public ConsultaDemandaComentarioOutput()
        {
            Comentarios = new List<ConsultarComentarioDto>();
        }
    }
    
    public class ObtenerDetalleDemandaInput
    {
        public string IdDemanda { get; set; }
        public string NumeroSiniestro { get; set; }
    }

    public class ObtenerDetalleDemandaOutput
    {
        public object Detalle { get; set; }

        //public DetalleDemandaDto Detalle { get; set; }

        //public ObtenerDetalleDemandaOutput()
        //{
        //    Detalle = new DetalleDemandaDto();
        //}
    }

    public class ConsultaReporteJuiciosInput
    {
        public int? IdTipoReporte { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
        public int? IdProveedor { get; set; }
    }

    public class ConsultaReporteJuiciosOutput
    {
        public List<ConsultaReporteDto> Reporte { get; set; }

        public ConsultaReporteJuiciosOutput()
        {
            Reporte = new List<ConsultaReporteDto>();
        }

    }

    public class ConsultaReporteJuiciosRelevantesInput
    {
        public int? IdTipoReporte { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
        public int? IdProveedor { get; set; }
    }

    public class ConsultaReporteJuiciosRelevantesOutput
    {
        public List<ReporteRelevanteDto> Reporte { get; set; }

        public ConsultaReporteJuiciosRelevantesOutput()
        {
            Reporte = new List<ReporteRelevanteDto>();
        }
    }

    public class ConsultaReporteJuiciosEnContraAseguradosInput
    {
        public int? IdTipoReporte { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
        public int? IdProveedor { get; set; }
    }

    public class ConsultaReporteJuiciosEnContraAseguradosOutput
    {
        public List<ReporteEnContraAseguradoDto> Reporte { get; set; }

        public ConsultaReporteJuiciosEnContraAseguradosOutput()
        {
            Reporte = new List<ReporteEnContraAseguradoDto>();
        }
    }

    public class ConsultaReporteConcluidosInput
    {
        public int? IdTipoReporte { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
        public int? IdProveedor { get; set; }
    }

    public class ConsultaReporteConcluidosOutput
    {
        public List<ReporteConcluidosDto> Reporte { get; set; }

        public ConsultaReporteConcluidosOutput()
        {
            Reporte = new List<ReporteConcluidosDto>();
        }
    }

    public class AgregarActualizarDocumentoLitigioInput
    {
        public int? IdDocumento { get; set; }
        public int IdDemanda { get; set; }
        public string Nombre { get; set; }
        public string TipoMime { get; set; }
        public string Contenido { get; set; }
        public string TipoDocumento { get; set; }
        public string SubTipoDocumento { get; set; }
        public bool SentenciaIncumplimiento { get; set; }
        public string SubNombre { get; set; }
        public bool Borrar { get; set; }

        public string ObjectId { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DatosRegistroNotificacion DetalleRegistro { get; set; }
    }

    public class AgregarActualizarDocumentoLitigioOutput
    {
        public string Mensaje { get; set; }
    }
    

    public class ObtenerRegExpInput
    {        
        public int IdDemanda { get; set; }
    }

    public class ObtenerRegExpOutput
    {
        public int IdRegExp { get; set; }
        public string NumDemanda { get; set; }
    }

    public class ObtenerObjectIdInput
    {
        public int IdDocumento { get; set; }
    }

    public class ObtenerObjectIdOutput
    {
        public string ObjectId { get; set; }
    }

    public class ObtenerDocumentoLitigioInput
    {
        public int IdDocumento { get; set; }
    }

    public class ObtenerDocumentoLitigioOuput
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

    public class ConcluirDemandaLitigioInput
    {
        public string NumeroDemanda { get; set; }
        public bool Concluir { get; set; }
    }

    public class ConcluirDemandaLitigioOutput
    {
        public string Mensaje { get; set; }
    }

    public class DatosRegistroNotificacion
    {
        public string Materia { get; set; }
        public string EspecifiqueMateria { get; set; }
        public string Actor { get; set; }
        public string NotificacionCalidad { get; set; }
        public string ExpedienteJuicio { get; set; }
        public string Codemandados { get; set; }
        public int TerminosContestacion { get; set; }
        public DateTime FechaVencimientoContestacion { get; set; }
        public string Autoridad { get; set; }
        public string TipoJuicio { get; set; }
        public string MotivoDemanda { get; set; }
    }
}

