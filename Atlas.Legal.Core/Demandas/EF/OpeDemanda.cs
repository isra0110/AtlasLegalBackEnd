using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal
{
    [Table("OpeDemanda")]
    public partial class OpeDemanda : Entity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column("IdDemanda")]
        public override int Id { get; set; }

        public string Folio { get; set; }

        public int? IdExpedienteWebContent { get; set; }

    }
}
