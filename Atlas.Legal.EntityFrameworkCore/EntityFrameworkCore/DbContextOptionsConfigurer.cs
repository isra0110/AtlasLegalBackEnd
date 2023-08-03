using Microsoft.EntityFrameworkCore;

namespace Atlas.Legal.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<LegalDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for LegalDbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }
    }
}
