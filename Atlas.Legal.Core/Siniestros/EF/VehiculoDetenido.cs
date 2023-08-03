using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal
{
    [Table("OpeVehiculosDetenidos")]
    public partial class VehiculoDetenido : Entity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column("IdVehiculoDetenido")]
        public override int Id { get; set; }

        public int IdSiniestro { get; set; }

        [StringLength(250)]
        public string LiderACargo { get; set; }

        public short? IdOficinaRegional { get; set; }

        [StringLength(100)]
        public string Estado { get; set; }

        [StringLength(20)]
        public string NumeroReporte { get; set; }

        [StringLength(20)]
        public string NumeroSinietro { get; set; }

        public short? IdDelito { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaTurnado { get; set; }

        [StringLength(250)]
        public string Asegurado { get; set; }

        [StringLength(500)]
        public string Vehiculo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaAcreditaPropiedad { get; set; }

        public int? DiasDetenido { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaLiberacion { get; set; }

        [StringLength(100)]
        public string KPI { get; set; }

        public short? IdEstatusLiberado { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [ForeignKey("IdSiniestro")]
        public virtual Siniestro Siniestro { get; set; }
    }
}