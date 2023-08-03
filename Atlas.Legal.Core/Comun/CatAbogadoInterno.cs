using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal.Comun
{
    [Table("CatAbogadoInterno")]
    public partial class CatAbogadoInterno : Entity<int>
    {
        [Key, Column("Id")]
        public override int Id { get; set; }

        public string Nombre { get; set; }

        public string Correo { get; set; }
    }
}
