using AutoMapper;
using TodoTypeService.Models;

namespace TodoTypeService.TodoTypes.Commons;

public class TodoTypeBriefDto()
{
    public string Name { get; set; } = string.Empty;


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TodoType, TodoTypeBriefDto>();
        }
    }
}