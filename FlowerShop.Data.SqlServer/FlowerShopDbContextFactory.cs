using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FlowerShop.Data.SqlServer
{
    public class FlowerShopDbContextFactory: IDesignTimeDbContextFactory<FlowerShopDbContext>
    {
        public FlowerShopDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return CreateDbContext(configuration);
        }

        public FlowerShopDbContext CreateDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<FlowerShopDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new FlowerShopDbContext(optionsBuilder.Options);
        }
    }
   
}
