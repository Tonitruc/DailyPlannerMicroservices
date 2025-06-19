using AutoMapper;
using TodoService.Domain.Models;

namespace TodoService.Application.Dtos;

public class TodoDto
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    //public TodoStatus Status { get; set; }


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Todo, TodoDto>();
        }
    }
}
