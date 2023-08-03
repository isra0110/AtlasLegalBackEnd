using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using static Atlas.Legal.FraudesInputOutput.ConsultaFraudesOutput;

namespace Atlas.Legal
{
    public class FraudesInputOutput
    {
        public class RegistroFraudesOutput
        {
            public int IdFraude { get; set; }
            public string NumeroFraude { get; set; }
            public int IdRegExp { get; set; }
            public string Mensaje { get; set; }
            public int ClaveMensaje { get; set; }
        }

        public class RegistroFraudesInput
        {
            public int? IdProcedimientoEnContraDe { get; set; }
            public int? IdMultipleViasSolucionUno { get; set; }
            public int? IdMultipleViasSolucionDos { get; set; }
            public int? IdMultipleViasSolucionTres { get; set; }
            public DateTime? FechaTurnado { get; set; }
            public int? Antiguedad { get; set; }
            public string UsuarioRegistro { get; set; }
            public DateTime FechaRegistro { get; set; }
            public string MontoInicialARecuperar { get; set; }
            public string MontoPropuestoAuditoria { get; set; }
            public string MontoFinalARecuperar { get; set; }
            public string MontoDeterminadoAutoridad { get; set; }
            public string MontoNnegociado { get; set; }
            public int? IdAreaQueIdentifica { get; set; }
            public string HonorarioPagado { get; set; }
            public DateTime? FechaPago { get; set; }
            public DateTime? FechaCreacion { get; set; }
            public string NumeroFraude { get; set; }
            public bool? investigacionActiva { get; set; }
            public bool? JudicializarLaCDI { get; set; }
            public int? IdRegExp { get; set; }
            public List<GuardarCausaProblemaFraudesInput> CausaProblema { get; set; }
            public List<GuardarDocumentoFraudesInput> Documentos { get; set; }
        }

        public class GuardarCausaProblemaFraudesInput
        {
            public int? Id { get; set; }
            public string NumeroFraude { get; set; }
            public string CausaProblema { get; set; }
            public string RealizadoPor { get; set; }
            public DateTime FechaRegistro { get; set; }
        }

        public class GuardarCausaProblemaFraudesOutput
        {
            public int Id { get; set; }
            public string FolioFraude { get; set; }
            public int CodigoMensaje { get; set; }
            public string Mensaje { get; set; }
        }

        public class GuardarAsignacionFraudesInput
        {
            public int? IdAbogado { get; set; }
            public string NumeroFraude { get; set; }
            public bool EsInterno { get; set; }
            public int IdProveedor { get; set; }
            public DateTime FechaCreacion { get; set; }            
        }

        public class GuardarAsignacionFraudesOutput
        {
            public int id { get; set; }
            public string FolioFraudes { get; set; }
            public int CodigoMensaje { get; set; }
            public string Mensaje { get; set; }
        }

        public class GuardarDocumentoFraudesInput
        {
            public int? IdDocumento { get; set; }
            public int? IdFraudes { get; set; }
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

        public class ConsultaFraudesOutput
        {
            public List<FraudesDto> Fraudes { get; set; }

            public ConsultaFraudesOutput()
            {
                Fraudes = new List<FraudesDto>();
            }

            public class FraudesDto
            {
                public string VerDetalle { get; set; }
                public string NoDeFolio { get; set; }
                public string ProcedimientoEnContraDe { get; set; }
                public string MultiplesViasSolucion { get; set; }
                public DateTime? FechaTurnado { get; set; }
                public int? Antiguedad { get; set; }
                public string MontoInicialARecuperarse { get; set; }
                public string MontoPresupuestoAuditoria { get; set; }
                public string MontoFinalARecuperar { get; set; }
                public string MontoDeterminadoAutoridad { get; set; }
                public string MontoNegociado { get; set; }
                public string AreaQueIdentifica { get; set; }
                public bool? InvestigacionDelCampo { get; set; }
                public object Proveedor { get; set; }
            }
        }

        public class ConsultaFraudesInput
        {
            public string NumeroFraude { get; set; }

            public int? IdProcedimientoEnContraDe { get; set; }
            public string IdProveedor { get; set; }
            public DateTime? FechaRegistroInicial { get; set; }
            public DateTime? FechaRegistroFinal { get; set; }
        }

        public class DetalleFraudesInput
        {
            public string NumeroFraude { get; set; }
        }

        public class DetalleFraudesOutput
        {
            public int? IdFraudes { get; set; }
            public int? IdProcedimientoEnContraDe { get; set; }
            public int? IdMultpleViaSolucionUno { get; set; }
            public int? IdMultpleViaSolucionDos { get; set; }
            public int? IdMultpleViaSolucionTres { get; set; }
            public DateTime? FechaTurnado { get; set; }
            public int? Antiguedad { get; set; }
            public string UsuarioRegistro { get; set; }
            public DateTime? FechaRegistro { get; set; }
            public string MontoInicialARecuperarse { get; set; }
            public string MontoPresupuestoAuditoria { get; set; }
            public string MontoFinalARecuperar { get; set; }
            public string MontoDeterminadoAutoridad { get; set; }
            public string MontoNegociado { get; set; }
            public int? IdAreaQueIdentifica { get; set; }
            public string HonorarioPagado { get; set; }
            public DateTime? FechaPago { get; set; }
            public int? IdRegExp { get; set; }
            public bool? InvestigacionJuicioDeCampo { get; set; }
            public bool? JudicializarLaCDI { get; set; }
            public bool? Concluir { get; set; }
            public string FolioFraude { get; set; }
            public object CausaProblema { get; set; }
            public object Documentos { get; set; }
            public object Asignacion { get; set; }

        }

        public class ConcluirFraudeInput
        {
            public string NumeroFraude { get; set; }
            public bool Concluido { get; set; }
        }

        public class ConcluirFraudeOutput
        {
            public string FolioFraude { get; set; }
            public int CodigoMensaje { get; set; }
            public string Mensaje { get; set; }
        }

        public class GuardarTrackingFraudeInput
        {
            public string NumeroFraude { get; set; }
            public DateTime FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Movimiento { get; set; }
        }

        public class GuardarComentarioFraudeInput
        {
            public string NumeroFraude { get; set; }
            public DateTime FechaRegistro { get; set; }
            public string UsuarioRegistro { get; set; }
            public string Comentario { get; set; }
        }

        public class GuardarComentarioFraudeOutput
        {
            public string Mensaje { get; set; }
        }

        public class ObtenerTrackingFraudesInput
        {
            public string NumeroFraude { get; set; }
        }

        public class ObtenerTrackingFraudesOutput
        {
            public List<TrackingDto> Trackings { get; set; }

            public ObtenerTrackingFraudesOutput()
            {
                Trackings = new List<TrackingDto>();
            }

            public class TrackingDto
            {
                public int? IdDemandaTracking { get; set; }
                public string NumeroFraude { get; set; }
                public DateTime? FechaRegistro { get; set; }
                public string UsuarioRegistro { get; set; }
                public string Movimiento { get; set; }
            }
        }

        public class ConsultaReportesFraudesOutput
        {
            public List<ReportesDto> Fraudes { get; set; }

            public ConsultaReportesFraudesOutput()
            {
                Fraudes = new List<ReportesDto>();
            }

            public class ReportesDto
            {
                public string NoDeFolio { get; set; }
                public string ProcedimientoEnContraDe { get; set; }
                public string MultiplesViasSolucion { get; set; }
                public DateTime? FechaTurnado { get; set; }
                public int? Antiguedad { get; set; }
                public string MontoInicialARecuperarse { get; set; }
                public string MontoPresupuestoAuditoria { get; set; }
                public string MontoFinalARecuperar { get; set; }
                public string MontoDeterminadoAutoridad { get; set; }
                public string MontoNegociado { get; set; }
                public string AreaQueIdentifica { get; set; }
                public bool? InvestigacionDelCampo { get; set; }
                public object Proveedor { get; set; }
            }
        }
        public class ConsultaReportesFraudesInput
        {
            public string NumeroFraude { get; set; }

            public int? IdProcedimientoEnContraDe { get; set; }
            public string IdProveedor { get; set; }
            public DateTime? FechaRegistroInicial { get; set; }
            public DateTime? FechaRegistroFinal { get; set; }
        }

        public class ObtenerDocumentoFraudesInput
        {
            public int IdDocumento { get; set; }
        }

        public class ObtenerDocumentoFraudesOutput
        {
            public int IdDocumento { get; set; }
            public int IdFraude { get; set; }
            public string Nombre { get; set; }
            public string Objeto { get; set; }
            public string Contenido { get; set; }
            public DateTime FechaRegistro { get; set; }
            public string TipoMime { get; set; }
            public string UsuarioRegistro { get; set; }
            public string TipoDocumento { get; set; }
            public string SubTipoDocumento { get; set; }
        }

        public class GuardarDocumentoFraudeOutput
        {
            public string Mensaje { get; set; }
        }

        public class ObtenerObjectIdFraudeInput
        {
            public int IdDocumento { get; set; }
        }

        public class ObtenerIdRegExpFraudeOutput
        {
            public int? IdRegExp { get; set; }
            public string NumeroFraude { get; set; }
        }

        public class ObtenerComentariosFraudesOutput
        {
            public List<ComentarioDto> Comentarios { get; set; }

            public ObtenerComentariosFraudesOutput()
            {
                Comentarios = new List<ComentarioDto>();
            }

            public class ComentarioDto
            {
                public int? IdDemandaComentario { get; set; }
                public string NumeroFraude { get; set; }
                public DateTime? FechaRegistro { get; set; }
                public string UsuarioRegistro { get; set; }
                public string Comentario { get; set; }
            }
        }

        public class ObtenerComentariosFraudesInput
        {
            public string NumeroFraude { get; set; }
        }        
    }

    [AutoMapFrom(typeof(FraudesDto))]
    public class PlantillaFraudesModel
    {
        public string ProcedimientoEnContraDe { get; set; }
        public string MultiplesViasSolucion { get; set; }
        public DateTime FechaTurnado { get; set; }
        public int Antiguedad { get; set; }
        public string MontoInicialARecuperarse { get; set; }
        public string MontoPresupuestoAuditoria { get; set; }
        public string MontoFinalARecuperar { get; set; }
        public string MontoDeterminadoAutoridad { get; set; }
        public string MontoNegociado { get; set; }
        public string AreaQueIdentifica { get; set; }
        public string AppUrl { get; set; }
    }
}
