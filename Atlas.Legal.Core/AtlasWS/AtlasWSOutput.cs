using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Legal.AtlasWS
{
    public class AtlasWSOutput
    {
        public class NotificacionesInput
        {
            public int id_cloud { get; set; }
            public string numeroReporte { get; set; }
            public string para { get; set; }
            public string cc { get; set; }
            public string cco { get; set; }
            public string mensaje { get; set; }
            public string asunto { get; set; }
            public List<ArchivosInput> archivos { get; set; }
        }

        public class CorreoInput
        {
            public int id_cloud { get; set; }
            public string numeroReporte { get; set; }
            public string para { get; set; }
            public string cc { get; set; }
            public string cco { get; set; }
            public string mensaje { get; set; }
            public string asunto { get; set; }
            public string archivos { get; set; }
        }

        public class ArchivosInput
        {
            public string nombreArchivo { get; set; }
            public string extArchivo { get; set; }
            public string archivo { get; set; }
        }

        public class NotificacionesOutput
        {
            public NotificacionDto RespSalida { get; set; }

            public class NotificacionDto
            {
                public string estatus_resp { get; set; }
                public string desc_estatus_resp { get; set; }
                public string fecha_resp { get; set; }
                public string hora_resp { get; set; }
            }            
        }

        public class JuridicoInput
        {
            public string numeroReporte { get; set; }
        }

        public class JuridicoOutput
        {
            public SiniestralidadOutput Siniestralidad { get; set; }
            public List<RecuperacionesOutput> Recuperaciones { get; set; }
            public List<PagoTercerosOutput> PagoTerceros { get; set; }
            public DeducibleOutput Deducible { get; set; }
            public DictamenesFavorableOutput DictamenesFavorables { get; set; }
            public LiberacionVehiculosOutput LiberacionVehiculos { get; set; }
            public HomicidiosOutput HomicidiosYLesiones { get; set; }
            public VehiculosDetenidosOutput VehiculosDetenidos { get; set; }
            public List<DocumentosOutput> Documentos { get; set; }

        }

        public class SiniestralidadOutput
        {
            public int? Id { get; set; }
            public int? IdDelito { get; set; }
            public int? IdCausa { get; set; }
            public string MesRecuperacion { get; set; }
            public string NumeroReporteProveedorLegal { get; set; }
            public TipoRecuperacionDtc IdTipoRecuperacion { get; set; }
            public string MontoRecuperacion { get; set; }
            public string FechaRecuperacion { get; set; }
            public string NumeroSiniestroAtlas { get; set; }

            public class TipoRecuperacionDtc
            {
                public string TipoMovimiento { get; set; }
                public DateTime? FechaMovimiento { get; set; }
                public string ImporteMovimeinto { get; set; }
                public string PrestadorServicios { get; set; }
            }
        }

        public class RecuperacionesOutput
        {
            public string Regional { get; set; }
            public string DeducibleEstimado { get; set; }
            public string MontoRecuperado { get; set; }
            public string HonorarioComision { get; set; }
            public string Iva { get; set; }
            public string Total { get; set; }
            public string FechaRecuperacion { get; set; }
            public string DeducibleValuacion { get; set; }
            public string FolioFactura { get; set; }
        }

        public class PagoTercerosOutput
        {
            public string Regional { get; set; }
            public string Beneficiario { get; set; }
            public string MontoPago { get; set; }
            public string FechaComprobacion { get; set; }
            public string FechaPago { get; set; }
        }

        public class DeducibleOutput
        {
            public AnioPagadoDtc AnioPagado { get; set; }
            public string MontoPagado { get; set; }
            public int? IdEstatus { get; set; }
            public string FechaReembolso { get; set; }

            public class AnioPagadoDtc
            {
                public string Regional { get; set; }
                public string Beneficiario { get; set; }
                public string MontoPago { get; set; }
                public DateTime? FechaComprobacion { get; set; }
                public DateTime? FechaPago { get; set; }
            }
        }

        public class DictamenesFavorableOutput
        {
            public string FechaRecuperacion { get; set; }
            public string MontoDeducible { get; set; }
            public string FechaSolicitud { get; set; }
            public string FechaPagoDeducible { get; set; }
            public int? IdTipoPoliza { get; set; }
            public string EstimadoDaniosAjusteAsegurado { get; set; }
            public string EstimadoDaniosAjusteTercero { get; set; }
        }

        public class LiberacionVehiculosOutput
        {
            public string OficinaRegional { get; set; }
            public string EstimacionDanos { get; set; }
        }

        public class HomicidiosOutput
        {
            public string FechaEmisionCheque { get; set; }
            public string MontoPagadoHomicidio { get; set; }
            public string MontoPagadoLesiones { get; set; }
        }

        public class VehiculosDetenidosOutput
        {
            public int? IdOficinaRegional { get; set; }
            public string FechaAcreditaPropiedad { get; set; }
        }

        public class DocumentosOutput
        {
            public string Nombre { get; set; }
            public string TipoMime { get; set; }
            public string Contenido { get; set; }
            public string TipoDocumento { get; set; }
            public string SubTipoDocumento { get; set; }
        }

        public class SiniestroAtlasInput
        {
            public int? idRamoBusqueda { get; set; }
            public string noSiniestro { get; set; }
        }

        public class SiniestroAtlasOutput
        {
            public List<SiniestroCondusef> BusquedaSiniestro { get; set; }
        }

        public class SiniestroCondusef
        {
            public string RamoBusqueda { get; set; }
            public string Siniestro { get; set; }
            public string Poliza { get; set; }
            public string NombreAsegurado { get; set; }
            public string ClaveAgente { get; set; }
            public string NombreAgente { get; set; }
            public string FechaOcurrido { get; set; }
            public string HonorariosPagados { get; set; }
            public string FechaPagoHonorarios { get; set; }
            public string OtrosGastos { get; set; }
        }

        public class LitigioAtlasInput
        {
            public string noSiniestro { get; set; }
        }

        public class LitigioAtlasOutput
        {
            public List<SiniestroLitigio> BusquedaSiniestro { get; set; }
        }

        public class SiniestroLitigio
        {
            public string RamoBusqueda { get; set; }
            public string Siniestro { get; set; }
            public string Poliza { get; set; }
            public string NombreAsegurado { get; set; }
            public string TipoMoneda { get; set; }
            public string ReservaCondusefPesos { get; set; }
            public string ReservaCondusefDolares { get; set; }
            public string ReservaPesos { get; set; }
            public string ReservaDolares { get; set; }
            public string OtrosGastos { get; set; }
            public string HonorariosPagados { get; set; }
            public string FechaPagoHonorarios { get; set; }
            public string Condena { get; set; }
            public string FechaPago { get; set; }
            public string ReaseguroFacultativo { get; set; }
            public string PorcentajeParticipacionAtlas { get; set; }
            public string Coaseguro { get; set; }
            public string ColocacionEnContratos { get; set; }
            public string RetencionAtlas { get; set; }
            public string SumaAseguradaPesos { get; set; }
            public string SumaAseguradaDolares { get; set; }
            public string TotalSumaAseguradaMonedaNac { get; set; }
            public string Resultado { get; set; }
        }

        public class RecuperacionesSiniestroInput
        {
            public int? idTipoRecuperacion { get; set; }
            public string fechaRegistroInicial { get; set; }
            public string fechaRegistroFinal { get; set; }
            public string numeroSiniestro { get; set; }
        }

        public class RecuperacionesSiniestroOutput
        {
            public List<Recuperaciones> ConsultaSiniestro { get; set; }
        }

        public class Recuperaciones
        {
            public string NumeroSiniestro { get; set; }
            public string OrdenTrabajo { get; set; }
            public string Asegurado { get; set; }
            public string FechaOcurrido { get; set; }
            public string Analista { get; set; }
            public string Moneda { get; set; }
            public string Estado { get; set; }
            public string Recuperacion { get; set; }
            public string Poliza { get; set; }
            public string Vehiculo { get; set; }
            public string Causa { get; set; }
            public string Tercero { get; set; }
            public string FechaIngreso { get; set; }
            public string Honorarios { get; set; }
        }

        public class SiniestroMailInput
        {
            public string NumeroReporte { get; set; }
            public string Cuenta { get; set; }
            public string Asegurado { get; set; }
            public string Siniestro { get; set; }
            public string Entidad { get; set; }
            public string Municipio { get; set; }
            public DateTime FechaApertura { get; set; }
            public string Abogado { get; set; }
            public int PersonasLesionadas { get; set; }
            public int Homicidios { get; set; }
            public List<ArchivosInput> archivos { get; set; }
        }

        

    }
}
