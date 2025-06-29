using AutoMapper;
using TodoService.Domain.Models;

namespace TodoService.Application.Dtos;

using AutoMapper;
using System;
using System.Globalization;

public class CreateTodoDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateTodoDto, Todo>()
                .ForMember(d => d.StartDate, opt => opt.MapFrom(s =>
                    string.IsNullOrEmpty(s.StartDate)
                        ? DateTime.Now
                        : DateTime.ParseExact(s.StartDate, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture)))
                .ForMember(d => d.EndDate, opt => opt.MapFrom(s =>
                    string.IsNullOrEmpty(s.EndDate)
                        ? (DateTime?)null
                        : DateTime.ParseExact(s.EndDate, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
        }
    }
}
