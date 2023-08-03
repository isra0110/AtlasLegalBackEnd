using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace Atlas.Legal
{
    public class SiniestroSeleccionadoDto : Entity
    {
        public string RamoBusqueda { get; set; }
        public string Poliza { get; set; }
        public string Siniestro { get; set; }
        public string NombreAsegurado { get; set; }
        public string TipoMoneda { get; set; } //No esta en sp
        public string ReservaCondusefPesos { get; set; }
        public string ReservaCondusefDolares { get; set; }
        public string ReservaPesos { get; set; }
        public string ReservaDolares { get; set; }
        public string OtrosGastos { get; set; }
        public string HonorariosPagados { get; set; } // No esta en sp
        public string FechaPagoHonorarios { get; set; }
        public string Condena { get; set; }
        public string FechaPago { get; set; }
        public string HonorarioTabulador { get; set; }
        public string HonorarioAutorizado { get; set; }
        public string ReaseguroFacultativo { get; set; }
        public string PorcentajeParticipacionAtlas { get; set; }
        public string Coaseguro { get; set; }
        public string ColocacionEnContratos { get; set; }
        public string RetencionAtlas { get; set; }
        public string SumaAseguradaPesos { get; set; }
        public string SumaAseguradaDolares { get; set; }
        public string TotalSumaAseguradaMonedaNac { get; set; }
        public bool Resultado { get; set; }
    }

    public class ConsultaDemandaDto
    {
        public int? IdAsigna { get; set; }
        public int IdDemanda { get; set; }
        public string Actor { get; set; }
        public string NumeroSiniestro { get; set; }
        public string Ramo { get; set; }
        public string EtapaProcesal { get; set; }
        public string Ubicacion { get; set; }
        public string Juzgado { get; set; }
        public string AbogadoDesignadoAtlas { get; set; }
        public string AbogadoDesignadoInvolucrado { get; set; }
        public object Asignacion { get; set; }
        public string TipoJuicio { get; set; }
        public string Materia { get; set; }
        public string Expediente { get; set; }
        public string Codemandados { get; set; }
        public int? TerminoContestacion { get; set; }
        public string AutoridadJudicial { get; set; }
        public string MotivoDemanda { get; set; }
        public bool? JuicioRelevante { get; set; }
        public string NombreJuzgado { get; set; }
        public string Monto { get; set; }
        public string Reclamo { get; set; }
        public string MontoReclamadoSinDeterminar { get; set; }
    }

    public class ConsultaTrackingDto : Entity
    {
        public string IdDemanda { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Movimiento { get; set; }
    }

    public class ConsultarComentarioDto : Entity
    {
        public string IdDemanda { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Comentario { get; set; }
    }

    public class DetalleDemandaDto
    {
        public object Detalle { get; set; }
    }

    public class ConsultaReporteDto : Entity
    {
        public string Actor { get; set; }
        public string Poliza { get; set; }
        public string Siniestro { get; set; }
        public string RamoBusqueda { get; set; }
        public string Juzgado { get; set; }
        public string TipoJuicio { get; set; }
        public string EspecifiqueCual { get; set; }
        public string ExpedienteJuicio { get; set; }
        public string AbogadoDesignadoAtlas { get; set; }
        public string AbogadoDesignadoInvolucrado { get; set; }
        public string MontoReclamado { get; set; }
        public string TipoMoneda { get; set; }
        public string ReservaConducefPesos { get; set; }
        public string ReservaConducefDolares { get; set; }
        public string ReservaPesos { get; set; }
        public string ReservaDolares { get; set; }
        public string OtrosHonorarios { get; set; }
        public string HonorarioTabulador { get; set; }
        public string HonorarioAutorizado { get; set; }
        public string HonorariosPagados { get; set; }
        public string Condena { get; set; }
        public DateTime Inicio { get; set; }
        public string LugarJuicio { get; set; }
        public DateTime FechaPagoHonorarios { get; set; }
        public DateTime FechaCondena { get; set; }
        public string Comentario { get; set; }
        public object Etapa { get; set; }
    }

    public class ReporteRelevanteDto
    {
        public string Asegurado { get; set; }
        public string Poliza { get; set; }
        public string Siniestro { get; set; }
        public DateTime Fecha { get; set; }
        public string Plaza { get; set; }
        public string Abogado { get; set; }
        public string EtapaProcesal { get; set; }
        public string SumaAseguradaPesos { get; set; }
        public string SumaAseguradaDolares { get; set; }
        public string TotalSumaAseguradaMonedaNac { get; set; }
        public string MontoReclamado { get; set; }
        public string TipoMoneda { get; set; }
        public string ReservaConducefPesos { get; set; }
        public string ReservaConducefDolares { get; set; }
        public string ReservaPesos { get; set; }
        public string ReservaDolares { get; set; }
        public string ReaseguroFacultativo { get; set; }
        public string PorcentajeParticipacionAtlas { get; set; }
        public string Coaseguro { get; set; }
        public string ColocacionEnContratos { get; set; }
        public string RetencionAtlas { get; set; }
        public string Comentario { get; set; }
    }

    public class ReporteEnContraAseguradoDto
    {
        public string Asegurado { get; set; }
        public string Poliza { get; set; }
        public string Siniestro { get; set; }
        public DateTime Fecha { get; set; }
        public string Plaza { get; set; }
        public string Abogado { get; set; }
        public string EtapaProcesal { get; set; }
        public string MontoReclamado { get; set; }
        public string TipoMoneda { get; set; }
        public string ReservaConducefPesos { get; set; }
        public string ReservaConducefDolares { get; set; }
        public string ReservaPesos { get; set; }
        public string ReservaDolares { get; set; }
        public string Comentario { get; set; }
    }

    public class ReporteConcluidosDto
    {
        public string Asegurado { get; set; }
        public string Poliza { get; set; }
        public string Siniestro { get; set; }
        public string Ramo { get; set; }
        public bool? Resultado { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Plaza { get; set; }
        public string TipoJuicio { get; set; }
        public string Juzgado { get; set; }
        public string Abogado { get; set; }
        public string MontoReclamado { get; set; }
        public string TipoMoneda { get; set; }
        public string ReservaConducefPesos { get; set; }
        public string ReservaConducefDolares { get; set; }
        public string ReservaPesos { get; set; }
        public string ReservaDolares { get; set; }
        public string HonorariosPagados { get; set; }
        public string HonorarioAutorizado { get; set; }
        public string HonorarioTabulador { get; set; }
        public string PagoCondena { get; set; }
        public DateTime FechaPagoCondena { get; set; }
        public string ResumenDemanda { get; set; }
        public string Comentario { get; set; }
    }
}
