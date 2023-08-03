using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Atlas.Legal.Comun
{
    [Table("CatProveedor")]
    public partial class CatProveedor : Entity<int>
    {
        [Key, Column("Id")]
        public override int Id { get; set; }

        public int CVEProveedor { get; set; }

        [Required]
        [StringLength(20)]
        public string OficinaCoordinadora { get; set; }

        [Required]
        [StringLength(20)]
        public string OficinaServicio { get; set; }

        public int CVETipoProveedor { get; set; }

        public int NumeroProveedor { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreComercial { get; set; }

        [StringLength(100)]
        public string UbicCalle { get; set; }

        [StringLength(20)]
        public string UbicNumExt { get; set; }

        [StringLength(20)]
        public string UbicNumInt { get; set; }

        [StringLength(50)]
        public string UbicColonia { get; set; }

        [Required]
        [StringLength(50)]
        public string UbicDelegacionMunicipio { get; set; }

        public int UbicCVEDelegMuni { get; set; }

        public int UbicCP { get; set; }

        public int UbicCVEEstado { get; set; }

        public int UbicCVEPais { get; set; }

        public int CVETipoTel { get; set; }

        public int? LadaTel1 { get; set; }

        [StringLength(20)]
        public string Tel1 { get; set; }

        [StringLength(5)]
        public string ExtTel1 { get; set; }

        public int CVETipoTel2 { get; set; }

        public int? LadaTel2 { get; set; }

        [StringLength(20)]
        public string Tel2 { get; set; }

        [StringLength(5)]
        public string ExtTel2 { get; set; }

        public int CVETipoTel3 { get; set; }

        public int? LadaTel3 { get; set; }

        [StringLength(20)]
        public string Tel3 { get; set; }

        [StringLength(5)]
        public string ExtTel3 { get; set; }

        public int CVEEstatus { get; set; }

        public DateTime FechaEstatus { get; set; }

        public int UbicLatitud { get; set; }

        public int UbicLongitud { get; set; }

        [StringLength(100)]
        public string Correo { get; set; }
    }
}
