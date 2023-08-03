using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal
{
    [Table("OpeSiniestrosDeducible")]
    public partial class Deducible : Entity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column("IdDeducible")]
        public override int Id { get; set; }

        public int IdSiniestro { get; set; }

        public int? AnioPagado { get; set; }

        public int? AnioSiniestro { get; set; }

        [StringLength(250)]
        public string Sucursal { get; set; }

        [StringLength(20)]
        public string NumeroReporte { get; set; }

        [StringLength(20)]
        public string NumeroSiniestro { get; set; }

        [StringLength(250)]
        public string Localidad { get; set; }

        public double? Mondo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaSolicitud { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaPago { get; set; }

        public short? IdEstatus { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public int? FaseProcesal { get; set; }

        [StringLength(500)]
        public string observacion { get; set; }

        public int? incidencias { get; set; }

        [ForeignKey("IdSiniestro")]
        public virtual Siniestro Siniestro { get; set; }
    }
}