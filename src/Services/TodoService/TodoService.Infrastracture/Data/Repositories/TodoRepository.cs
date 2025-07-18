﻿using Microsoft.EntityFrameworkCore;
using TodoService.Domain.Contracts.Repositories;
using TodoService.Domain.Models;

namespace TodoService.Infrastracture.Data.Repositories;

public class TodoRepository(ApplicationDbContext context) : ITodoRepository
{
    public async Task<IEnumerable<Todo>> GetAllTodosAsync(CancellationToken cancellationToken = default)
    {
        return await context.Todos
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Todo?> GetTodoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Todos
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Todo> CreateTodoAsync(Todo todo, CancellationToken cancellationToken = default)
    {
        var entry = await context.Todos
            .AddAsync(todo, cancellationToken);

        return entry.Entity;
    }

    public bool DeleteTodo(Todo todo)
    {
        context.Remove(todo);
        return true;
    }
}
