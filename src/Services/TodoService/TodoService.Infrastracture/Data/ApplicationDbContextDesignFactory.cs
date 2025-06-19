using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace TodoService.Infrastracture.Data;

public class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private const string BaseConfigurationPath = @"P:\VS\Projects\Everydaynik\src\Services\TodoService\TodoService.Presentation";


    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var path = Assembly.GetExecutingAssembly().Location;

        var fullPath = Path.Combine(path, BaseConfigurationPath);

        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder
            .SetBasePath(BaseConfigurationPath)
            .AddJsonFile("appsettings.Development.json");

        var configuration = configurationBuilder.Build();

        var dbContextBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(configuration.GetConnectionString("PostgresSQL"));

        return new ApplicationDbContext(dbContextBuilder.Options);
    }
}
