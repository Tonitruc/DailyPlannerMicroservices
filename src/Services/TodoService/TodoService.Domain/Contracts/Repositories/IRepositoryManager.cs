namespace TodoService.Domain.Contracts.Repositories;

public interface IRepositoryManager
{
    ITodoRepository Todos { get; }

    Task<bool> SaveChangesAsync();
}
