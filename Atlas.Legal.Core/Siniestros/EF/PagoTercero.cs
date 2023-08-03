using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal
{
    [Table("OpeSiniestrosPagoTerceros")]
    public partial class PagoTercero : Entity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column("IdPagoTercero")]
        public override int Id { get; set; }

        public int IdSiniestro { get; set; }

        [StringLength(500)]
        public string Proveedor { get; set; }

        public int? Anio { get; set; }

        [StringLength(20)]
        public string NumeroReporte { get; set; }

        [StringLength(20)]
        public string NumeroSiniestro { get; set; }

        [StringLength(250)]
        public string Regional { get; set; }

        [StringLength(100)]
        public string Estado { get; set; }

        [StringLength(500)]
        public string Beneficiario { get; set; }

        public double? MontoPago { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [ForeignKey("IdSiniestro")]
        public virtual Siniestro Siniestro { get; set; }
    }
}
