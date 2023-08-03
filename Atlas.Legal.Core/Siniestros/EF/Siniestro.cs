using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal
{
    [Table("OpeSiniestros")]
    public partial class Siniestro : Entity<int>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Siniestro()
        {
            Dictamen = new HashSet<Dictamen>();
            Recuperacion = new HashSet<Recuperacion>();
            Deducible = new HashSet<Deducible>();
            PagoTercero = new HashSet<PagoTercero>();
            VehiculosDetenidos = new HashSet<VehiculoDetenido>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column("IdSiniestro")]
        public override int Id { get; set; }

        [StringLength(20)]
        public string NumeroReporte { get; set; }

        [StringLength(20)]
        public string NumeroSiniestro { get; set; }

        [StringLength(500)]
        public string Proveedor { get; set; }

        [StringLength(20)]
        public string MesTurnado { get; set; }

        [StringLength(250)]
        public string SucursalSegurosAtlas { get; set; }

        public int? NumeroReporteProveedorLegal { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaReporte { get; set; }

        public short? IdAbogadoAsignado { get; set; }

        [StringLength(250)]
        public string Coordinador { get; set; }

        [StringLength(250)]
        public string Gerencia { get; set; }

        [StringLength(250)]
        public string Abogado { get; set; }

        [StringLength(20)]
        public string NumeroSiniestroAtlas { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaSiniestro { get; set; }

        public TimeSpan? HoraSiniestro { get; set; }

        [StringLength(250)]
        public string Ajustador { get; set; }

        [StringLength(250)]
        public string Asegurado { get; set; }

        public short? IdEstatus { get; set; }

        public short? IdDelito { get; set; }

        public short? IdCausa { get; set; }

        public short? IdResponsabilidadAjuste { get; set; }

        public short? IdPagoRecuperacion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaAcreditamientoPropiedad { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaLiberacionVehiculo { get; set; }

        public int? DiasLiberacion { get; set; }

        public short? IdAutoridadConoce { get; set; }

        public short? IdEtapaProceso { get; set; }

        [StringLength(20)]
        public string MesRecuperacion { get; set; }

        public short? idTipoRecuperacion { get; set; }

        public double? Monto { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaRecuperacion { get; set; }

        public short? IdSubEstatus { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [StringLength(50)]
        public string usuario { get; set; }

        public TimeSpan? tiempoestatus { get; set; }

        public int? consecutivo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaAcceso { get; set; }

        public TimeSpan? horaAcceso { get; set; }

        [StringLength(50)]
        public string delito { get; set; }

        [StringLength(50)]
        public string tipoDeLesionados { get; set; }

        public int? noFallecidos { get; set; }

        [StringLength(500)]
        public string quienFallece { get; set; }

        [StringLength(50)]
        public string parteAccidente { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaDeConclusion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaAcreditacion { get; set; }

        public int? idmodificacionintegracion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaTurnado { get; set; }

        public short? mesTurn { get; set; }

        public short? idEstado { get; set; }

        public short? idAgravante { get; set; }

        public short? idResponsabilidadDictamen { get; set; }

        public double? avaluoOficial { get; set; }

        public int? idRegExp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dictamen> Dictamen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recuperacion> Recuperacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deducible> Deducible { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PagoTercero> PagoTercero { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehiculoDetenido> VehiculosDetenidos { get; set; }
    }
}