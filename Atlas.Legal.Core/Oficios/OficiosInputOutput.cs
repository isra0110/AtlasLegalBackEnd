using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Legal
{
    public class RegistraOficioInput
    {
        public int? IdTipoOficio { get; set; }
        public DateTime? FechaEmisionOficio { get; set; }
        public string DependenciaQueEnvia { get; set; }
        public string Asunto { get; set; }
        public string Expediente { get; set; }
        public string Oficio { get; set; }
        public DateTime? FechaNotificacionSegurosAtlas { get; set; }
        public int? HojasOficio { get; set; }
        public int? IdAreaInvolucrada { get; set; }
        public string NumeroOficio { get; set; }
        public int? IdRegExp { get; set; }
        public ICollection<DocumentoModel> Documentos { get; set; }
    }

    public class RegistraOficioOutput : BaseOutput
    {
        public int? IdOficio { get; set; }
        public string NumeroOficio { get; set; }
        public int? IdRegExp { get; set; }
    }

    public class RegistraOficioDocumentoInput
    {        
        public int? IdDocumento { get; set; }
        public int? IdOficio { get; set; }
        public string Nombre { get; set; }
        public string Objeto { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Contenido { get; set; }
        public string UsuarioRegistro { get; set; }
        public string SubTipoDocumento { get; set; }
        public string TipoMime { get; set; }
        public string TipoDocumento { get; set; }
        public bool Borrar { get; set; }
    }

    public class ObtenerObjectIdOficiosInput
    {
        public int IdDocumento { get; set; }
    }

    public class ObtenerIdRegExpOficiosOutput
    {
        public int? IdRegExp { get; set; }
        public string NumOficio { get; set; }
    }

    public class ObtenerDocumentoOficiosInput
    {
        public int IdDocumento { get; set; }
    }
    
    public class ObtenerDocumentoOficiosOutput
    {
        public int IdDocumento { get; set; }
        public int IdOficio { get; set; }
        public string Nombre { get; set; }
        public string ObjectId { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string TipoMime { get; set; }
        public string UsuarioRegistro { get; set; }
        public string TipoDocumento { get; set; }
        public string SubTipoDocumento { get; set; }
    }

    public class GuardaActualizaDocumentoOficiosOutput
    {
        public string Mensaje { get; set; }
    }
    
    public class RegistraOficioDocumentoOutput : BaseOutput
    {
        public int? IdOficios { get; set; }
        public int? IdDocumento { get; set; }
    }

    public class AsignarAbogadoOficiosInput
    {
        public List<AsignacionModel> AsignarAbogado { get; set; }

        public class AsignacionModel
        {
            public int? IdDemanda { get; set; }
            public string NumeroOficio { get; set; }
            public bool? EsInterno { get; set; }
            public int? IdProveedor { get; set; }
            public int? IdAbogadoInterno { get; set; }
            public DateTime? FechaCreacion { get; set; }
            public DetalleOficioNotificacion DetalleOficio { get; set; }
        }
    }

    public class AsignarAbogadoOficiosOutput : BaseOutput
    {
        public int? IdDemanda { get; set; }
    }

    public class ConsultaOficioInput
    {
        public int? IdTipoOficio { get; set; }
        public int? IdAbogadoInterno { get; set; }
        public int? IdProveedor { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
    }

    public class ConsultaOficioOutput : BaseOutput
    {
        public List<DetalleModel> Detalles { get; set; }

        public ConsultaOficioOutput()
        {
            Detalles = new List<DetalleModel>();
        }

        public class DetalleModel
        {
            public int? IdOficio { get; set; }
            public string NumOficio { get; set; }
            public int? IdTipoOficio { get; set; }
            public DateTime? FechaEmision { get; set; }
            public string DependenciaQueEnvia { get; set; }
            public string Asunto { get; set; }
            public string Expediente { get; set; }
            public string Oficio { get; set; }
            public DateTime? FechaNotificacionSeguroAtlas { get; set; }
            public int? HojasOficio { get; set; }
            public int? IdAreaInvolucrada { get; set; }
            public object Asignacion { get; set; }
            //public string TipoAsignacion { get; set; }
            //public string Proveedor { get; set; }
            //public string AbogadoInterno { get; set; }
        }
    }

    public class GuardaTrackingOficioInput
    {
        public string NumDemanda { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Movimiento { get; set; }
    }

    public class ObtenerTrackingsOficioInput
    {
        public string NumOficio { get; set; }
    }

    public class GuardaComentarioOficiosInput
    {
        public string NumOficio { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Comentario { get; set; }
    }

    public class GuardaComentarioOficiosOutput
    {
        public string Mensaje { get; set; }
    }

    public class ObtenerComentariosOficiosInput
    {
        public string NumOficio { get; set; }
    }

    public class ObtenerComentariosOficiosOutput
    {
        public List<ComentarioDto> Comentarios { get; set; }

        public ObtenerComentariosOficiosOutput()
        {
            Comentarios = new List<ComentarioDto>();
        }

        public class ComentarioDto
        {
            public int IdDemandaComentario { get; set; }
            public string NumOficio { get; set; }
            public DateTime FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Comentario { get; set; }
        }
    }

    public class ObtenerTrackingsOficioOutput
    {
        public List<TrackingDto> Trackings { get; set; }

        public ObtenerTrackingsOficioOutput()
        {
            Trackings = new List<TrackingDto>();
        }

        public class TrackingDto
        {
            public int IdDemandaTracking { get; set; }
            public string NumOficio { get; set; }
            public DateTime FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Movimiento { get; set; }
        }
    }

    public class DetalleOficioInput
    {
        public string NumeroOficio { get; set; }
    }

    public class DetalleOficioOutput : BaseOutput
    {
        public string NumeroOficio { get; set; }
        public object VerDetalle { get; set; }
        public object Asignacion { get; set; }
    }

    public class ConcluirOficioInput
    {
        public string NumeroOficio { get; set; }
        public bool Concluir { get; set; }
        public bool Pendiente { get; set; }
    }

    public class ConcluirOficioOutput : BaseOutput
    {
        public string NumeroOficio { get; set; }
    }

    public class ReportesOficiosInput
    {
        public int? IdTipoOficio { get; set; }
        public int? IdAbogadoInterno { get; set; }
        public int? IdProveedor { get; set; }
        public DateTime? FechaRegistroInicial { get; set; }
        public DateTime? FechaRegistroFinal { get; set; }
    }

    public class DetalleOficioNotificacion
    {
        public string NumeroOficio { get; set; }
        public string TipoOficio { get; set; }
        public DateTime FechaEmision { get; set; }
        public string DependenciaQueEnvia { get; set; }
        public string Asunto { get; set; }
        public string Expediente { get; set; }
        public string Oficio { get; set; }
        public DateTime FechaNotificacionSeguroAtlas { get; set; }
        public string HojasOficio { get; set; }
        public string AreaInvolucrada { get; set; }
    }

    [AutoMapFrom(typeof(ConsultaOficioOutput.DetalleModel))]
    public class PlantillaOficio
    {
        public int? IdOficio { get; set; }
        public string NumOficio { get; set; }
        public int? IdTipoOficio { get; set; }
        public DateTime? FechaEmision { get; set; }
        public string DependenciaQueEnvia { get; set; }
        public string Asunto { get; set; }
        public string Expediente { get; set; }
        public string Oficio { get; set; }
        public DateTime? FechaNotificacionSeguroAtlas { get; set; }
        public int? HojasOficio { get; set; }
        public int? IdAreaInvolucrada { get; set; }
        //public object Asignacion { get; set; }
        public string AppUrl { get; set; }
        //public string TipoAsignacion { get; set; }
        //public string Proveedor { get; set; }
        //public string AbogadoInterno { get; set; }
    }
}
