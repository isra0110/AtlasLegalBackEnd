using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal
{
    [Table("OpeDictamenes")]
    public partial class Dictamen : Entity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column("IdDictamen")]
        public override int Id { get; set; }

        public int IdSiniestro { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaDictamen { get; set; }

        public double? AvaluoDIctamen { get; set; }

        [StringLength(250)]
        public string AutoridadConoce { get; set; }

        [StringLength(100)]
        public string TipoVehiculoAsegurado { get; set; }

        public double? EstimadoDaniosAjusteAsegurado { get; set; }

        [StringLength(100)]
        public string TipoVehiculoTercero { get; set; }

        public double? EstimadoDaniosAjusteTercero { get; set; }

        [StringLength(250)]
        public string SeguroContratadoTercero { get; set; }

        [StringLength(15)]
        public string TipoPersonaTercero { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public virtual Siniestro Siniestro { get; set; }
    }
}