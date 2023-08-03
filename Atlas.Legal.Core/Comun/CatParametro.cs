using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal.Comun
{
    [Table("CatParametro")]
    public partial class CatParametro : Entity<string>
    {
        [Key, Column("IdParametro")]
        public override string Id { get; set; }

        public string Valor { get; set; }
    }
}
