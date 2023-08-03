using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Atlas.Legal
{
    
    public class SiniestralidadDto
    {
        //Proveedor
        public int? Id { get; set; }
        public int? IdEstatus { get; set; }
        public DateTime? FechaDeAcreditamiendoDePropiedad { get; set; }
        public DateTime? FechaDeLiberacionVehiculo { get; set; }
        public int? DíasLiberacion { get; set; }
        public Int16? IdResponsabilidadDictamen { get; set; }
        public decimal? AvaluoOficial { get; set; }
        [StringLength(50,ErrorMessage = "El NumeroSiniestroAtlas debe tener una longitud máxima de 50 caracteres")]
        public string NumeroSiniestroAtlas { get; set; }
        public Int16? IdAutoridadQueConoce { get; set; }
        public Int16? IdEtapaDeProceso { get; set; }        
        public Int16? IdAgravante { get; set; }

        //Sisa
        public Int16? IdDelito { get; set; }
        public Int16? IdCausa { get; set; }
        public string MesRecuperacion { get; set; }
        public Int16? IdTipoRecuperacion { get; set; }
        public decimal? MontoRecuperacion { get; set; }
        public DateTime? FechaRecuperacion { get; set; }
        public DateTime? FechaTurnado { get; set; }

        //Prometeo
        public string IdSucursalSegurosAtlas { get; set; }
        public int? IdProveedor { get; set; }
        public int? IdMesTurnado { get; set; }
        public string NumeroReporteProveedorLegal { get; set; }
        public DateTime? FechaReporte { get; set; }
        public string? Abogado { get; set; }
        public DateTime? FechaSiniestro { get; set; }
        public string Ajustador { get; set; }
        public string Asegurado { get; set; }
        public Int16? IdResponsabilidadAjuste { get; set; }
        public Int16? IdEstado { get; set; }
        public Int16? IdPagoRecuperacion { get; set; }
        public string NumeroReporteCabina { get; set; }
    }

    public class DictamenesFavorablesDto
    {
        public DateTime? FechaDictamen { get; set; }
        public decimal? AvaluoDictamen { get; set; }
        public string AutoridadQueConoce { get; set; }
        public string EspecificarRecuperacion { get; set; }
        public string FaseProcesal { get; set; }        
        public string Coordinador { get; set; }
        public string Gerencia { get; set; }
        public string TipoRecuperacion { get; set; }
        
        public DateTime? FechaRecuperacion { get; set; }
        public decimal? MontoDeducible { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public DateTime? FechaPagoDeducible { get; set; }
        public Int16? IdTipoPoliza { get; set; }


        public string TipoVehiculoAsegurado { get; set; }
        public decimal? EstimadoDaniosAjusteAsegurado { get; set; }
        public string TipoVehiculoTercero { get; set; }
        public decimal? EstimadoDaniosAjusteTercero { get; set; }
        public Int16? IdSeguroContratado { get; set; }
        public Int16? IdVehiculoAsegurado { get; set; }
        public Int16? IdTipoPersona { get; set; }
    }

    public class RecuperacionesDto
    {
        public Int16? IdTipoDeRecuperacion { get; set; }
        public string Parcialidad { get; set; }
        public DateTime? FechaDeConvenio { get; set; }

        public string Regional { get; set; }
        public decimal? DeducibleEstimado { get; set; }
        public decimal? MontoRecuperado { get; set; }
        public decimal? HonorarioComision { get; set; }
        public decimal? Iva { get; set; }
        public decimal? Total { get; set; }
        public DateTime? FechaRecuperacion { get; set; }
        public decimal? DeducibleValuacion { get; set; }
        public string FolioFactura { get; set; }
    }

    public class HomicidiosYLesionesDto
    {
        public Int16? IdEstatus { get; set; }
        public Int16? IdEtapaProcesal { get; set; }
        public Int16? IdDelitoFinal { get; set; }
        public Int16? IdTipoDeLesionados { get; set; }        
        public Int16? IdQuienFallece { get; set; }        
        public DateTime? FechaDeConclusion { get; set; }

        public int? IdMesTurnado { get; set; }
        public Int16? IdCircunstancia { get; set; }
        public Int16? IdDeslindeResponsabilidad { get; set; }
        public DateTime? FechaEmisionCheque { get; set; }
        public decimal? MontoPagadoHomicidio { get; set; }
        public decimal? MontoPagadoLesiones { get; set; }

        
        public int? NumeroLesionados { get; set; }
        public int? NumeroFallecidos { get; set; }
        public Int16? IdParteAccidente { get; set; }
        public Int16? IdResponsabilidadDictamen { get; set; }
        public decimal? ReservaRCLesiones { get; set; }
        public decimal? ReservaHomicidios { get; set; }        

    }

    public class VehiculosDetenidosDto
    {        
        public string LiderACargo { get; set; }
        public DateTime? FechaDeLiberacion { get; set; }
        
        public Int16? IdOficinaRegional { get; set; }
        public DateTime? FechaAcreditaPropiedad { get; set; }

        public string Vehiculo { get; set; }
        public DateTime? FechaLiberacion { get; set; }

        public Int16? IdDelito { get; set; }
        public string NumeroReporteProveedorLegal { get; set; }
    }

    public class LiberacionVehiculosDto
    {
        public Int16? IdDelitoInicial { get; set; }
        public DateTime? FechaAcreditacion { get; set; }
                
        public string OficinaRegional { get; set; }
        public decimal? EstimacionDanos { get; set; }

        public string TipoVehiculo { get; set; }
    }

    public class PagoTercerosDto
    {
        public string Regional { get; set; }
        public string Beneficiario { get; set; }
        public decimal? MontoPago { get; set; }
        public DateTime? FechaComprobacion { get; set; }
        public DateTime? FechaPago { get; set; }

        //Prometeo
        public int? AnioSiniestro { get; set; }
        public string Reporte { get; set; }
        public int? Cobertura { get; set; }
        public string CoberturaTexto { get; set; }

    }

    public class DeducibleDto
    {
        public int? AnioPagado { get; set; }
        public decimal? MontoPagado { get; set; }
        public Int16? IdEstatus { get; set; }
        public DateTime? FechaReembolso { get; set; }


        public DateTime? FechaSolicitud { get; set; }
        public DateTime? FechaPago { get; set; }
        public int? AnioSiniestro { get; set; }
        public string Sucursal { get; set; }
        public string Localidad { get; set; }        
        
    }

    public class BitacoraDto
    {
        public string NoSiniestroAtlas { get; set; }
        public string NumeroReporteProveedorLegal { get; set; }
        public string Usuario { get; set; }
        public int IdModificacionIntegracion { get; set; }
    }


    public class GuardarDocumentosDto
    {
        public int IdTipoDocumental { get; set; }
        public string Nombre { get; set; }
        public string TipoMime { get; set; }
        public string Contenido { get; set; }        
        public string TipoDocumento { get; set; }
        public string SubTipoDocumento { get; set; }
    }    


    public class DemandaDto
    {
        public string folio { get; set; }
        public int NumeroX { get; set; }
    }
    

    public class BusquedaSolicitudDto
    {
        public string Proveedor { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string NumeroReporteProveedorLegal { get; set; }
        public DateTime? FechaReporte { get; set; }
        public string Abogado { get; set; }
        public string NumeroSiniestroAtlas { get; set; }
        public DateTime? FechaSiniestro { get; set; }
        public string Ajustador { get; set; }
        public string Asegurado { get; set; }
        public string Estatus { get; set; }
        public string Delito { get; set; }
        public string Causa { get; set; }
        public string ResponsabilidadAjuste { get; set; }
        public string PagoRecuperacion { get; set; }
        public DateTime? FechaAcreditamientoPropiedad { get; set; }
        public DateTime? FechaLiberacionVehiculo { get; set; }
        public int? DiasLiberacion { get; set; }
        public string ResponsabilidadDictamen { get; set; }
        public decimal? AvaluoOficial { get; set; }
        public string AutoridadConoce { get; set; }
        public string EtapaProceso { get; set; }
        public string MesRecuperacion { get; set; }
        public string TipoRecuperacion { get; set; }
        public decimal? Monto { get; set; }
        public DateTime? FechaRecuperacion { get; set; }
        public string SucursalSegurosAtlas { get; set; }
    }

    public class DetalleSolicitudDto
    {
        public object DictamenesFavorables { get; set; }
        public object Recuperaciones { get; set; }
        public object Deducibles { get; set; }
        public object VehiculosDetenidos { get; set; }
        public object PagoTerceros { get; set; }
        public object Documentos { get; set; }
    }
    
}
