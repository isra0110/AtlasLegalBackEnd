using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.Legal.Autorizacion
{
    [Table("OpeUsuario")]
    public class OpeUsuario : Entity<int>
    {
        [Key, Column("idUsuario")]
        public override int Id { get; set; }

        [StringLength(30)]
        public string nombre { get; set; }

        [StringLength(30)]
        public string apaterno { get; set; }

        [StringLength(30)]
        public string amaterno { get; set; }

        public int idrol { get; set; }

        public int idtipo { get; set; }

        [StringLength(20)]
        public string telefono { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(20)]
        public string usuario { get; set; }
        
        [StringLength(20)]
        public string contrasenia { get; set; }

        public bool interno { get; set; }

        public bool activo { get; set; }
    }
}
