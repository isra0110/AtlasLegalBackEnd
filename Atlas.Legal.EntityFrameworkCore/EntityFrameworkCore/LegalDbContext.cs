using Abp.EntityFrameworkCore;
using Atlas.Legal;
using Atlas.Legal.Autorizacion;
using Atlas.Legal.Comun;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Legal.EntityFrameworkCore
{
    public class LegalDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...
        public virtual DbSet<OpeDemanda> OpeDemanda { get; set; }

        public virtual DbSet<Dictamen> Dictamenes { get; set; }
        public virtual DbSet<Recuperacion> Recuperaciones { get; set; }
        public virtual DbSet<Siniestro> Siniestros { get; set; }
        public virtual DbSet<Deducible> Deducibles { get; set; }
        public virtual DbSet<PagoTercero> PagosTerceros { get; set; }
        public virtual DbSet<VehiculoDetenido> VehiculosDetenidos { get; set; }
        public virtual DbSet<OpeUsuario> OpeUsuario { get; set; }


        public virtual DbSet<CatAbogadoInterno> CatAbogadoInterno { get; set; }
        public virtual DbSet<CatPlantillaCorreo> CatPlantillaCorreo { get; set; }
        public virtual DbSet<CatProveedor> CatProveedor { get; set; }
        public virtual DbSet<CatParametro> CatParametro { get; set; }
        public virtual DbSet<OpeNotificacion> OpeNotificacion { get; set; }

        public LegalDbContext(DbContextOptions<LegalDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
