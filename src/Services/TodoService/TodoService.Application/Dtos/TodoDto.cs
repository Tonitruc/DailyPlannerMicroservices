using TodoService.Domain.Models;
using TodoService.Domain.Enums;
using AutoMapper;

namespace TodoService.Application.Dtos;

public class TodoDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public TodoStatuses Status { get; set; }


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Todo, TodoDto>();
        }
    }
}
