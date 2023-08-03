using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal.Comun
{
    [Table("CatPlantillaCorreo")]
    public partial class CatPlantillaCorreo : Entity<int>
    {
        [Key, Column("IdPlantillaCorreo")]
        public override int  Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(300)]
        public string Asunto { get; set; }

        [Required]
        public string Plantilla { get; set; }

        [StringLength(1000)]
        public string Destinatarios { get; set; }

        public bool? EsAlerta { get; set; }
    }
}
