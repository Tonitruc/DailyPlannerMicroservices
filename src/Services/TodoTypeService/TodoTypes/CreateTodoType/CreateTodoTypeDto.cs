using AutoMapper;
using TodoTypeService.Models;

namespace TodoTypeService.TodoTypes.CreateTodoType;

public class CreateTodoTypeDto
{
    public string Name { get; set; } = string.Empty;


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateTodoTypeDto, TodoType>();
        }
    }
}
