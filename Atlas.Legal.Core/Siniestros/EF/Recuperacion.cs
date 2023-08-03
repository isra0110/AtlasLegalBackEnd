using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal
{
    [Table("OpeRecuperaciones")]
    public partial class Recuperacion : Entity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column("IdRecuperacion")]
        public override int Id { get; set; }

        public int IdSiniestro { get; set; }

        [StringLength(250)]
        public string Regional { get; set; }

        public short? IdEstado { get; set; }

        [StringLength(20)]
        public string NumeroReporte { get; set; }

        [StringLength(20)]
        public string NumeroSiniestro { get; set; }

        public short? IdTipoRecuperacion { get; set; }

        public double? Deducible { get; set; }

        public double? MontoRecuperado { get; set; }

        public double? Comision { get; set; }

        public double? Iva { get; set; }

        public double? Total { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaRecuperacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [StringLength(500)]
        public string EspecificarRecuperacion { get; set; }

        public virtual Siniestro Siniestro { get; set; }
    }
}