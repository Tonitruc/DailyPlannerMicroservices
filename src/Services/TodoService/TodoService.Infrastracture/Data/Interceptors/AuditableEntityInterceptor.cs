using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TodoService.Application.Contracts.User;
using TodoService.Domain.Abstractions;

namespace TodoService.Infrastracture.Data.Interceptors;

public class AuditableEntityInterceptor(TimeProvider timeProvider,
    IUserClaimsService userClaimsService)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? dbContext)
    {
        if (dbContext is null)
            return;

        var entities = dbContext.ChangeTracker.Entries<IEntity>();

        foreach(var entity in entities)
        {
            if(entity.State == EntityState.Modified || entity.State == EntityState.Added || entity.HasChangedOwnedEntities())
            {
                if(entity.State == EntityState.Added)
                {
                    entity.Entity.CreatedBy = userClaimsService.GetUserEmail();
                    entity.Entity.LastModified = timeProvider.GetUtcNow().UtcDateTime;
                }

                entity.Entity.CreatedBy = userClaimsService.GetUserEmail();
                entity.Entity.Created = timeProvider.GetUtcNow().UtcDateTime;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
