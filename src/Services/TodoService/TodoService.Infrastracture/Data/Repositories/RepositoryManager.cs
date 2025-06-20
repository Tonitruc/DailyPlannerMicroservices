using TodoService.Domain.Contracts.Repositories;

namespace TodoService.Infrastracture.Data.Repositories;

public class RepositoryManager(ApplicationDbContext context) : IRepositoryManager
{
    private readonly Lazy<ITodoRepository> _todoRepository = new(
        () => new TodoRepository(context));

    public ITodoRepository Todos => _todoRepository.Value;


    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default) 
        => (await context.SaveChangesAsync(cancellationToken)) > 0;
}
