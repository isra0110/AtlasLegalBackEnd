using Atlas.Legal.Configuration;
using Atlas.Legal.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Atlas.Legal.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class LegalDbContextFactory : IDesignTimeDbContextFactory<LegalDbContext>
    {
        public LegalDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LegalDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(LegalConsts.ConnectionStringName)
            );

            return new LegalDbContext(builder.Options);
        }
    }
}