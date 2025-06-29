using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using TodoService.Domain.Models;

namespace TodoService.Infrastracture.Data;

public static class DatabaseInitializer
{
    public static void AddSeedingAsync(this DbContextOptionsBuilder builder, IServiceProvider provider)
    {
        builder.UseAsyncSeeding(async (context, _, ct) =>
        {
            var initializer = provider.GetRequiredService<ApplicationDatabaseInitializer>();

            await initializer.SeedAsync();
        });
    }

    public static async Task InitializeDatabaseAsync(this WebApplication applications)
    {
        using var scope = applications.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseInitializer>();

        await initializer.InitializeAsync();
    }
}

public class ApplicationDatabaseInitializer(ApplicationDbContext context)
{
    public async Task InitializeAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"---> Exception: App init InitializeAsync() - {ex.Message}"); //TODO Logger
            throw;
        }
    }


    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"---> Exception: App init SeedAsync() - {ex.Message}"); //TODO Logger
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if(!context.Todos.Any())
        {
            await context.Todos.AddRangeAsync([
                //new Todo { Description = "Test", Title = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddHours(1) }
                ]);

            await context.SaveChangesAsync();
        }
    }
}
