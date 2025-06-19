using TodoService.Domain.Contracts.Repositories;

namespace TodoService.Infrastracture.Data.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationDbContext _context;

    private readonly Lazy<ITodoRepository> _todoRepository;


    public RepositoryManager(ApplicationDbContext context)
    {
        _context = context;

        _todoRepository = new Lazy<ITodoRepository>(() => new TodoRepository(context));
    }


    public ITodoRepository Todos => _todoRepository.Value;


    public async Task<bool> SaveChangesAsync() => (await _context.SaveChangesAsync()) > 0;
}
