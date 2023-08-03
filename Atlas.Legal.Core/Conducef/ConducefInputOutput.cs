using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Legal
{
    public class ConsultaConducefInput
    {
        public int? IdExpedienteProvieneDe { get; set; }
        public int? IdDependencia { get; set; }
        public int? IdNaturalezaActor { get; set; }
        public int? IdArea { get; set; }
        public int? IdRamoBusqueda { get; set; }
        public int? IdDepartamento1 { get; set; }
        public int? IdDepartamento2 { get; set; }
        public DateTime? Solantec { get; set; }
        public DateTime? FechaTermino { get; set; }
        public DateTime? FechaRecepcion { get; set; }
    }

    public class ConsultaConducefOutput
    {
        public List<ConducefDto> Conducef { get; set; }

        public ConsultaConducefOutput()
        {
            Conducef = new List<ConducefDto>();
        }

        public class ConducefDto
        {            
            public string NumeroConducef { get; set; }
            public string NumeroExpediente { get; set; }
            public DateTime? FechaTermino { get; set; }
            public string Dependencia { get; set; }
            public string Partes { get; set; }
            public string NaturalezaActor { get; set; }
            public string Poliza { get; set; }
            public string Siniestro { get; set; }
            public string RamoBusqueda { get; set; }
            public string Area { get; set; }
            public string MontoReclamado { get; set; }
            public string Comentario { get; set; }
            public string Caracteristicas { get; set; }
            public string Particularidad { get; set; }
            public string UltimoMovimiento { get; set; }
            public string ProductoServicio { get; set; }
            public DateTime? Solantec { get; set; }
            public bool? Reversagie { get; set; }
            public bool? Pori { get; set; }
            public DateTime? FechaRespuestaArea { get; set; }
            public DateTime? FechaResolucionConclusion { get; set; }
            public string EstadoConcluidoPendiente { get; set; }
            public DateTime? FechaNotificacionUsuario { get; set; }
            public string ImporteQueSeRestituyoAlUsuario { get; set; }
            public int? ClaveEntidadFederativa { get; set; }
        }        
    }

    public class DetalleConducefInput
    {
        public string NumeroConducef { get; set; }
    }

    public class DetalleConducefOutput
    {
        public int? IdConducef { get; set; }
        public int? IdExpedienteProvieneDe { get; set; }
        public string NumeroExpediente { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public int? TrimestreInforma { get; set; }
        public DateTime? FechaTermino { get; set; }
        public bool? CuentaConSiniestro { get; set; }
        public object Siniestro { get; set; }
        public int? IdDependencia { get; set; }
        public string CP { get; set; }
        public int? IdEntidad { get; set; }
        public int? ClaveEntidadFederativa { get; set; }
        public string Partes { get; set; }
        public int? IdNaturalezaActor { get; set; }
        public string Ramo { get; set; }
        public int? IdArea { get; set; }
        public string Caracteristicas { get; set; }
        public int? IdDepartamento1 { get; set; }
        public int? IdDepartamento2 { get; set; }
        public string Particularidad { get; set; }
        public string MontoReclamado { get; set; }
        public bool? SinCuantificar { get; set; }
        public int? IdDescripcionDelMedioRecepcion { get; set; }
        public int? ClaveMedioRecepcion { get; set; }
        public DateTime? Solantec { get; set; }
        public DateTime? FechaRespuestaArea { get; set; }
        public DateTime? FechaResolucionConclusion { get; set; }
        public int? IdEstadoConcluidoPendiente { get; set; }
        public string DescripcionEstadoConcluidoPendiente { get; set; }
        public DateTime? FechaNotificacionAlUsuario { get; set; }
        public string UltimoMovimiento { get; set; }
        public string Ubicacion { get; set; }
        public bool? Revesagie { get; set; }
        public bool? Pori { get; set; }
        public string ImporteQueSeRestituyoAlUsuario { get; set; }
        public DateTime? FechaEnQueSeRestituyoAlUsuario { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string EspecifiqueCual { get; set; }
        public string TipoMoneda { get; set; }
        public int? IdProductoServicio { get; set; }
        public int? IdCausaConducef { get; set; }
        public int? NumeroReclamaciones { get; set; }
        public bool? EsConcluido { get; set; }
        public object Documentos { get; set; }
        public object Observaciones { get; set; }
        public object Asignacion { get; set; }
    }


    public class ConducefReporteGeneralInput
    {
        public int? IdRamoBusqueda { get; set; }
        public int? IdArea { get; set; }
        public int? IdDepartamento1 { get; set; }
        public int? IdDepartamento2 { get; set; }
        public DateTime? FechaReporteInicial { get; set; }
        public DateTime? FechaReporteFinal { get; set; }
    }

    public class ConducefReporteGeneralOutput
    {
        public List<ReporteGeneralDto> Reportes { get; set; }

        public ConducefReporteGeneralOutput()
        {
            Reportes = new List<ReporteGeneralDto>();
        }

        public class ReporteGeneralDto
        {
            public string NumeroExpediente { get; set; }
            public string Poliza { get; set; }
            public string Siniestro { get; set; }
            public string NombreAsegurado { get; set; }
            public string ClaveAgente { get; set; }
            public string NombreAgente { get; set; }
            public DateTime? FechaOcurrido { get; set; }
            public string Dependencia { get; set; }
            public string CP { get; set; }
            public string Entidad { get; set; }
            public int? ClaveEntidadFederativa { get; set; }
            public string Partes { get; set; }
            public string NaturalezaActor { get; set; }
            public string Ramo { get; set; }
            public string Area { get; set; }
            public string Caracteristicas { get; set; }
            public string Departamento1 { get; set; }
            public string Departamento2 { get; set; }
            public string Particularidad { get; set; }
            public string MontoReclamado { get; set; }
            public string DescripcionMonedaRecepcion { get; set; }
            public string ClaveMedioRecepcion { get; set; }
            public DateTime? Solantec { get; set; }
            public DateTime? FechaRespuestaArea { get; set; }
            public DateTime? FechaResolucionConclusion { get; set; }
            public string UltimoMovimiento { get; set; }
            public string Ubicacion { get; set; }
            public string ImporteQueSeRestituyoAlUsuario { get; set; }
            public DateTime? FechaEnQueSeRestituyoAlUsuario { get; set; }
            public string TipoMoneda { get; set; }
            public string Comentario { get; set; }
            public string MRDolares { get; set; }
            public DateTime? FechaTermino { get; set; }
        }
    }

    public class ConducefReporteTrimestralInput
    {
        public int? IdRamoBusqueda { get; set; }
        public int? IdArea { get; set; }
        public DateTime? FechaReporteTrimestralInicial { get; set; }
        public DateTime? FechaReporteTrimestralFinal { get; set; }
    }

    public class ConducefReporteTrimestralOutput
    {
        public List<ReporteDto> Reportes { get; set; }

        public ConducefReporteTrimestralOutput()
        {
            Reportes = new List<ReporteDto>();
        }

        public class ReporteDto
        {
            public string Folio { get; set; }
            public DateTime? FechaRecepcion { get; set; }
            public string CP { get; set; }
            public string Entidad { get; set; }
            public string RamoBusqueda { get; set; }
            public string MontoReclamado { get; set; }
            public int? ClaveMedioRecepcion { get; set; }
            public DateTime? FechaResolucionConclusion { get; set; }
            public string EstadoConcluidoPendiente { get; set; }
            public DateTime? FechaNotificacionUsuario { get; set; }
            public bool? Reversagie { get; set; }
            public bool? Pori { get; set; }
            public string ImporteRestituyoUsuario { get; set; }
            public DateTime? FechaRestituyoUsuario { get; set; }
            public string DenominacionRazonSocial { get; set; }
            public string Sector { get; set; }
            public DateTime? TrimestreAInformar { get; set; }
            public string ProductoServicio { get; set; }
            public int? NumeroReclamaciones { get; set; }
            public string CausaConducef { get; set; }
        }
    }

    public class GuardarTrackingConducefInput
    {
        public string NumeroConducef { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Movimiento { get; set; }
    }

    public class GuardarSiniestroConducefInput
    {
        public int? IdSiniestro { get; set; }
        public int IdRamoBusqueda { get; set; }
        public string Poliza { get; set; }
        public string Siniestro { get; set; }
        public string NombreAsegurado { get; set; }
        public string ClaveAgente { get; set; }
        public string NombreAgente { get; set; }
        public DateTime FechaOcurrido { get; set; }
        public string HonorariosPagados { get; set; }
        public DateTime FechaHonorariosPagados { get; set; }
        public string OtrosGastos { get; set; }
        public string NumeroConducef { get; set; }
    }

    public class GuardarSiniestroConducefOutput
    {
        public string Mensaje { get; set; }
    }

    public class GuardarObservacionConducefInput
    {
        public int? IdObservacion { get; set; }
        public string RealizadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Comentario { get; set; }
        public string NumeroConducef { get; set; }
    }

    public class GuardarObservacionConducefOutput
    {
        public string Mensaje { get; set; }
    }

    public class GuardarComentarioConducefInput
    {
        public string NumeroConducef { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Comentario { get; set; }
    }

    public class GuardarComentarioConducefOutput
    {
        public string Mensaje { get; set; }
    }

    public class GuardarDocumentoConducefInput
    {
        public int? IdDocumento { get; set; }
        public int? IdConducef { get; set; }        
        public string Nombre { get; set; }
        public string ObjectId { get; set; }
        public string Contenido { get; set; }
        public DateTime? FechaRegistro { get; set; }        
        public string UsuarioRegistro { get; set; }
        public string SubTipoDocumento { get; set; }
        public string TipoMime { get; set; }
        public string TipoDocumento { get; set; }
        public bool Borrar { get; set; }
    }

    public class GuardarDocumentoConducefOutput
    {
        public string Mensaje { get; set; }
    }

    public class RegistroConducefInput
    {
        public int? IdConducef { get; set; }
        public int IdExpedienteProvieneDe { get; set; }
        public string NumeroExpediente { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public int TrimestreInforma { get; set; }
        public DateTime FechaTermino { get; set; }
        public bool CuentaConSiniestro { get; set; }
        public int IdDependencia { get; set; }
        public string CP { get; set; }
        public int IdEntidad { get; set; }
        public int ClaveEntidadFederativa { get; set; }
        public string Partes { get; set; }
        public int IdNaturalezaActor { get; set; }
        public string Ramo { get; set; }
        public int IdArea { get; set; }
        public string Caracteristicas { get; set; }
        public int IdDepartamento1 { get; set; }
        public int IdDepartamento2 { get; set; }
        public string Particularidad { get; set; }
        public string MontoReclamado { get; set; }
        public bool SinCuantificar { get; set; }
        public int IdDescripcionMedioRecepcion { get; set; }
        public int ClaveMedioRecepcion { get; set; }
        public DateTime Solantec { get; set; }
        public DateTime FechaRespuestaArea { get; set; }
        public DateTime FechaResolucionConclusion { get; set; }
        public int IdEstadoConcluidoPendiente { get; set; }
        public string DescripcionEstadoConcluidoPendiente { get; set; }
        public DateTime FechaNotificacionUsuario { get; set; }
        public string UltimoMovimiento { get; set; }
        public string Ubicacion { get; set; }
        public bool Reversagie { get; set; }
        public bool Pori { get; set; }
        public string ImporteRestituyoUsuario { get; set; }
        public DateTime FechaRestituyoUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string EspecifiqueCual { get; set; }
        public int IdTipoMoneda { get; set; }
        public string NumeroConducef { get; set; }
        public int NumeroReclamaciones { get; set; }
        public int IdProductoServicio { get; set; }
        public int IdCausaConducef { get; set; }
        public int? IdRegExp { get; set; }

        public List<GuardarSiniestroConducefInput> Siniestros { get; set; }
        public List<GuardarDocumentoConducefInput> Documentos { get; set; }
        public List<GuardarObservacionConducefInput> Observaciones { get; set; }        
    }

    public class RegistroConducefOutput
    {
        public string Mensaje { get; set; }
        public int Id { get; set; }
        public string NumeroConducef { get; set; }
        public int IdRegExp { get; set; }
    }

    public class ConcluirRegistroConducefInput
    {
        public string NumeroConducef { get; set; }
        public bool Concluir { get; set; }
    }

    public class ConcluirRegistroConducefOutput
    {
        public string Mensaje { get; set; }
    }

    public class AsignarAbogadoConducefInput
    {
        public int IdAbogado { get; set; }
        public string NumeroConducef { get; set; }
        public bool EsInterno { get; set; }
        public int IdProveedor { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class AsignarAbogadoConducefOutput
    {
        public string Mensaje { get; set; }
    }

    public class ObtenerObjectIdConducefInput
    {
        public int IdDocumento { get; set; }
    }

    public class ObtenerIdRegExpConducefOutput
    {
        public int? IdRegExp { get; set; }
        public string NumeroConducef { get; set; }
    }

    public class ObtenerDocumentoConducefInput
    {
        public int IdDocumento { get; set; }
    }

    public class ObtenerDocumentoConducefOutput
    {
        public int IdDocumento { get; set; }
        public int IdConducef { get; set; }
        public string Nombre { get; set; }
        public string ObjectId { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string TipoMime { get; set; }
        public string UsuarioRegistro { get; set; }
        public string TipoDocumento { get; set; }
        public string SubTipoDocumento { get; set; }
    }

    public class ObtenerTrackingsConducefInput
    {
        public string NumeroConducef { get; set; }
    }

    public class ObtenerComentariosConducefInput
    {
        public string NumeroConducef { get; set; }
    }

    public class ObtenerObservacionesConducefInput
    {
        public string NumeroConducef { get; set; }
    }


    public class ObtenerTrackingsConducefOutput
    {
        public List<TrackingDto> Trackings { get; set; }

        public ObtenerTrackingsConducefOutput()
        {
            Trackings = new List<TrackingDto>();
        }

        public class TrackingDto
        {
            public int? IdTracking { get; set; }
            public string NumeroConducef { get; set; }
            public DateTime? FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Movimiento { get; set; }
        }
    }


    public class ObtenerComentariosConducefOutput
    {
        public List<ComentarioDto> Comentarios { get; set; }

        public ObtenerComentariosConducefOutput()
        {
            Comentarios = new List<ComentarioDto>();
        }

        public class ComentarioDto
        {
            public int? IdComentario { get; set; }
            public string NumeroConducef { get; set; }
            public DateTime? FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Comentario { get; set; }
        }
    }

    public class ObtenerObservacionesConducefOutput
    {
        public List<ObservacionDto> Observaciones { get; set; }

        public ObtenerObservacionesConducefOutput()
        {
            Observaciones = new List<ObservacionDto>();
        }

        public class ObservacionDto
        {
            public int? IdObservacion { get; set; }
            public string RealizadoPor { get; set; }
            public DateTime? FechaCreacion { get; set; }
            public string Comentario { get; set; }
            public string NumeroConducef { get; set; }
        }
    }

    public class BuscaSiniestroSISAInput
    {
        public int? IdSiniestro { get; set; }
        public int IdRamoBusqueda { get; set; }
        public string Siniestro { get; set; }
        public string NumeroConducef { get; set; }
    }

    public class SiniestrosSISAInput
    {
        public List<SiniestroSISA> siniestrosSISA { get; set; }
    }

    public class SiniestroSISA
    {
        public int IdRamoBusqueda { get; set; }
        public string Siniestro { get; set; }
        public string Poliza { get; set; }
        public string NombreAsegurado { get; set; }
        public string ClaveAgente { get; set; }
        public string NombreAgente { get; set; }
        public string FechaOcurrido { get; set; }
        public string HonorariosPagados { get; set; }
        public string FechaPagoHonorarios { get; set; }
        public string OtrosGastos { get; set; }
        public string NumeroConducef { get; set; }
    }

    public class DatosCondusefNotificacion
    {
        public string NumeroExpediente { get; set; }
        public DateTime FechaTermino { get; set; }
        public string Dependencia { get; set; }
        public string Partes { get; set; }
        public string Siniestro { get; set; }
        public string RamoBusqueda { get; set; }
        public DateTime Solantec { get; set; }
        public int DiasTermino { get; set; }
    }
}

