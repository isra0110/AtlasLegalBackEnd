using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal.Comun
{
    [Table("OpeNotificacion")]
    public partial class OpeNotificacion : Entity<int>
    {
        [Key, Column("IdNotificacion")]
        public override  int Id { get; set; }

        public int IdPlantillaCorreo { get; set; }

        public int IdEntidad { get; set; }

        public bool EstaEnviada { get; set; }

        public DateTime? FechaEnvio { get; set; }

        public DateTime? FechaRegistro { get; set; }

        [ForeignKey("IdPlantillaCorreo")]
        public virtual CatPlantillaCorreo PlantillaCorreo { get; set; }

    }
}
