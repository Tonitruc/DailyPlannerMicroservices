using UserService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace UserService.Data;

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

    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseInitializer>();

        await initializer.InitializeAsync();
    }
}

public class ApplicationDatabaseInitializer
{
    private readonly ApplicationDbContext _context;

    private readonly UserManager<User> userManager;

    private readonly RoleManager<IdentityRole> roleManager;


    public ApplicationDatabaseInitializer(ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        this.userManager = userManager;
        this.roleManager = roleManager;
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
        if(!_context.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        if (!_context.Users.Any())
        {
            var newUser = new User { Email = "gromahacer@gmail.com", UserName = "Roman" };
            string password = "1234";

            var result = await userManager.CreateAsync(newUser, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, "Admin");
            }
        }
    }
}
