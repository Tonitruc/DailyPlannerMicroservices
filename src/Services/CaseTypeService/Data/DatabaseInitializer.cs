using CaseTypeService.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseTypeService.Data;

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

    public static async Task InitializeAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseInitializer>();

        await initializer.InitializeAsync();
    }
}

public class ApplicationDatabaseInitializer
{
    private readonly ApplicationDbContext _context;


    public ApplicationDatabaseInitializer(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
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
        if (!_context.CaseTypes.Any())
        {
            await _context.CaseTypes.AddRangeAsync(
                new CaseType { Name = "Meeting" },
                new CaseType { Name = "Call" },
                new CaseType { Name = "Email" },
                new CaseType { Name = "Project Work" },
                new CaseType { Name = "Conference" },
                new CaseType { Name = "Lunch" },
                new CaseType { Name = "Workout" },
                new CaseType { Name = "Study" },
                new CaseType { Name = "Shopping" },
                new CaseType { Name = "Reminder" },
                new CaseType { Name = "Trip" },
                new CaseType { Name = "Deadline" },
                new CaseType { Name = "Reading" },
                new CaseType { Name = "Meditation" },
                new CaseType { Name = "Movie" },
                new CaseType { Name = "Task Specification" },
                new CaseType { Name = "Planning" },
                new CaseType { Name = "Household Chores" },
                new CaseType { Name = "Daily Summary" },
                new CaseType { Name = "Documentation" }
            );

            await _context.SaveChangesAsync();
        }
    }
}
