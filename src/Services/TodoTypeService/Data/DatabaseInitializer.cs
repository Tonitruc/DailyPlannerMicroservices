using TodoTypeService.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoTypeService.Data;

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
        if (!_context.TodoTypes.Any())
        {
            await _context.TodoTypes.AddRangeAsync(
                new TodoType { Name = "Meeting" },
                new TodoType { Name = "Call" },
                new TodoType { Name = "Email" },
                new TodoType { Name = "Project Work" },
                new TodoType { Name = "Conference" },
                new TodoType { Name = "Lunch" },
                new TodoType { Name = "Workout" },
                new TodoType { Name = "Study" },
                new TodoType { Name = "Shopping" },
                new TodoType { Name = "Reminder" },
                new TodoType { Name = "Trip" },
                new TodoType { Name = "Deadline" },
                new TodoType { Name = "Reading" },
                new TodoType { Name = "Meditation" },
                new TodoType { Name = "Movie" },
                new TodoType { Name = "Task Specification" },
                new TodoType { Name = "Planning" },
                new TodoType { Name = "Household Chores" },
                new TodoType { Name = "Daily Summary" },
                new TodoType { Name = "Documentation" }
            );

            await _context.SaveChangesAsync();
        }
    }
}
