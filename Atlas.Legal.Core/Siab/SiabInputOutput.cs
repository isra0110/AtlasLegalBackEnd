using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Atlas.Legal
{
    public class GenerarRequerimientoSiabInput
    {
        public int RequerimientosDelDia { get; set; }
        public DateTime FechaElaboracionRequerimientos { get; set; }      
        public string UsuarioRegistro { get; set; }
    }

    public class GenerarRequerimientoSiabOutput
    {
        public string Mensaje { get; set; }
        public object Requerimientos { get; set; }
    }

    public class GuardaDocumentoSiabInput
    {
        public int? IdDocumento { get; set; }
        public int? IdRegistro { get; set; }
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

    public class GuardarTrackingSiabInput
    {
        public string NumeroSiab { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Movimiento { get; set; }
    }

    public class GuardarActualizarRegistroSiabInput
    {
        [Required]
        public string NumeroRegistro { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public DateTime? FechaTermino { get; set; }
        [Required]
        public string NombreAsegurado { get; set; }
        [Required]
        public string FechaNacimiento { get; set; }
        [Required]
        public string FechaDefuncion { get; set; }
        public bool Sexo { get; set; }
        [Required]
        public string FolioSio { get; set; }
        public int? IdIdentidad { get; set; }
        public string cp { get; set; }
        [Required]
        public string NombreBeneficiario { get; set; }
        public bool? RespuestaPrevia { get; set; }
        public int? IdRespuesta { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public int? IdRegExp { get; set; }
        [Required]
        public string Poliza { get; set; }
        public bool? Actualizar { get; set; }

        public List<GuardaDocumentoSiabInput> Documentos { get; set; }
    }

    public class GuardarActualizarRegistroSiabOutput
    {
        public string Mensaje { get; set; }
        public int? IdRegistro { get; set; }
        public int? IdRegExp { get; set; }
        public string NumeroRegistro { get; set; }
        public string Estatus { get; set; }
        public DateTime? FechaElaboracion { get; set; }
        public string ValorRespuesta { get; set; }
    }

    public class BusquedaRequerimientoSiabInput
    {
        public DateTime? FechaElaboracionInicio { get; set; }
        public DateTime? FechaElaboracionFin { get; set; }
        public string NumeroRegistro { get; set; }
    }

    public class BusquedaRequerimientoSiabOutput
    {
        public List<RequerimientoDto> Requerimientos { get; set; }

        public BusquedaRequerimientoSiabOutput()
        {
            Requerimientos = new List<RequerimientoDto>();
        }

        public class RequerimientoDto
        {
            public int? IdRegistro { get; set; }
            public string NumeroSiab { get; set; }
            public DateTime? FechaRegistro { get; set; }
            public string Estatus { get; set; }
            public DateTime? FechaElaboracion { get; set; }
            public int? IdRespuesta { get; set; }
            public string FolioSio { get; set; }
            public string NombreAsegurado { get; set; }
            public string FechaNacimiento { get; set; }
            public string FechaDefuncion { get; set; }
            public string NombreBeneficiario { get; set; }
            public string Poliza { get; set; }
            public string ValorRespuesta { get; set; }

            public DateTime? FechaRecepcion { get; set; }
            public DateTime? FechaRespuesta { get; set; }
            public bool? RespEmsVidaInd { get; set; }
            public DateTime? FechaTermino { get; set; }
            public string Entidad { get; set; }
            public bool? Sexo { get; set; }
            public string cp { get; set; }
            public bool? RespuestaSinVida { get; set; }
            public bool? RespEmsVidaCol { get; set; }
        }
    }

    public class GuardarSeguimientoRegistroSiabInput
    {
        public string NumeroRegistro { get; set; }
        public string cp { get; set; }
        public bool EditarRespuesta { get; set; }
        public int? IdRespuesta { get; set; }
        public bool RespuestaSinVida { get; set; }
        public string ComentarioRespSinVida { get; set; }
        public bool RespEmsVidaInd { get; set; }
        public bool RespEmsVidaCol { get; set; }
        public bool Concluir { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string ComentarioRespVidaInd { get; set; }
        public string ComentarioRespVidaCol { get; set; }

        public List<GuardaDocumentoSiabInput> Documentos { get; set; }
    }

    public class GuardarSeguimientoRegistroSiabOutput
    {
        public string NumeroRegistro { get; set; }
        public string Estatus { get; set; }
        public DateTime? FechaElaboracion { get; set; }
        public string ValorRespuesta { get; set; }
    }

    public class ObtenerIdRegExpSiabOutput
    {
        public int? IdRegExp { get; set; }
        public string NumeroRegistro { get; set; }
        public int? IdRegistro { get; set; }
    }

    public class ReporteRequerimientoSiabInput
    {
        public int? IdEstatus { get; set; }
        public int? IdRespuestaVida { get; set; }
        public DateTime? FechaElaboracionInicio { get; set; }
        public DateTime? FechaElaboracionFin { get; set; }
    }

    public class ReporteRequerimientoSiabOutput
    {
        public List<ReporteDto> Requerimientos { get; set; }

        public ReporteRequerimientoSiabOutput()
        {
            Requerimientos = new List<ReporteDto>();
        }

        public class ReporteDto
        {
            public string NumeroRegistro { get; set; }
            public DateTime? FechaRecepcion { get; set; }
            public DateTime? FechaTermino { get; set; }
            public string NombreAsegurado { get; set; }
            public string FechaNacimiento { get; set; }
            public string FechaDefuncion { get; set; }
            public string Sexo { get; set; }
            public string FolioSIO { get; set; }
            public string Entidad { get; set; }
            public string cp { get; set; }
            public string NombreBeneficiario { get; set; }
            public string RespuestaPrevia { get; set; }
            public int? IdRespuesta { get; set; }
            public DateTime? FechaRespuesta { get; set; }
            public bool? EditarRespuesta { get; set; }
            public string RespuestaSinVida { get; set; }
            public string ComentarioRespSinVida { get; set; }
            public string RespEmsVidaInd { get; set; }
            public string RespEmsVidaCol { get; set; }
            public string Estatus { get; set; }
            public string Poliza { get; set; }
            public string ValorRespuesta { get; set; }
        }        
    }

    public class ObtenerDetalleRequerimientoSiabInput
    {
        public string NumeroRegistro { get; set; }
    }

    public class ObtenerDetalleRequerimientoSiabOutput
    {
        public int? Id { get; set; }
        public string NumeroRegistro { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public DateTime? FechaTermino { get; set; }
        public string NombreAsegurado { get; set; }
        public string FechaNacimiento { get; set; }
        public string FechaDefuncion { get; set; }
        public bool? Sexo { get; set; }
        public string FolioSIO { get; set; }
        public int? IdEntidad { get; set; }
        public string cp { get; set; }
        public string NombreBeneficiario { get; set; }
        public bool? RespuestaPrevia { get; set; }
        public int? IdRespuesta { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public bool? Concluir { get; set; }
        public bool? EditarRespuesta { get; set; }
        public bool? RespuestaSinVida { get; set; }
        public string ComentarioRespuestaSinVida { get; set; }
        public bool? RespEmsVidaInd { get; set; }
        public string ComentarioRespVidaInd { get; set; }
        public bool? respEmsVidaCol { get; set; }
        public string ComentarioRespVidaCol { get; set; }
        public string Poliza { get; set; }
        public object Documentos { get; set; }
    }

    public class GuardarComentarioSiabInput
    {
        public string NumeroRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Comentario { get; set; }

    }

    public class GuardarComentarioSiabOutput
    {
        public string Mensaje { get; set; }
    }

    public class GuardaDocumentoSiabOutput
    {
        public string Mensaje { get; set; }
    }

    public class ObtenerObjectIdSiabInput
    {
        public int IdDocumento { get; set; }
    }

    public class ObtenerObjectIdSiabOutput
    {
        public string ObjectId { get; set; }
    }

    public class ObtenerIdRegExpSiabInput
    {
        public string NumeroRegistro { get; set; }
        public int? IdRegistro { get; set; }
    }

    public class ObtenerTrackingsSiabInput
    {
        public string NumeroRegistro { get; set; }
    }

    public class ObtenerComentariosSiabInput
    {
        public string NumeroRegistro { get; set; }
    }

    public class ObtenerTrackingsSiabOutput
    {
        public List<TrackingDto> Trackings { get; set; }

        public ObtenerTrackingsSiabOutput()
        {
            Trackings = new List<TrackingDto>();
        }

        public class TrackingDto
        {
            public int? Id { get; set; }
            public string NumeroRegistro { get; set; }
            public DateTime? FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Movimiento { get; set; }
        }
    }

    public class ObtenerComentariosSiabOutput
    {
        public List<ComentarioDto> Comentarios { get; set; }

        public ObtenerComentariosSiabOutput()
        {
            Comentarios = new List<ComentarioDto>();
        }

        public class ComentarioDto
        {
            public int? Id { get; set; }
            public string NumeroRegistro { get; set; }
            public DateTime? FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Comentario { get; set; }
        }
    }

    public class ObtenerDocumentoSiabInput
    {
        public int IdDocumento { get; set; }
    }

    public class ObtenerDocumentoSiabOutput
    {
        public int? IdDocumento { get; set; }
        public int? IdRegistro { get; set; }
        public string Nombre { get; set; }
        public string ObjectId { get; set; }
        public string Contenido { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string SubTipoDocumento { get; set; }
        public string TipoMime { get; set; }
        public string TipoDocumento { get; set; }
    }

    public class ValidacionSiabInput
    {
        public DateTime FechaRegistro { get; set; }
        public string Usuario { get; set; }
    }

    public class ValidacionSiabOutput
    {
        public int CodigoMensaje { get; set; }
        public string Mensaje { get; set; }
        public bool Validacion { get; set; }
    }

    public class DatosRequerimientoSiabNotificacion
    {
        public DateTime FechaRecepcion { get; set; }
        public int NumeroRequerimientos { get; set; }
        public DateTime FechaTermino { get; set; }
        public int DiasHabilesTermino { get; set; }
    }
}
