using AutoMapper;
using UserService.Models;

namespace UserService.Dtos;

public class BriefUserDto
{
    public string? Email { get; set; } = string.Empty;

    public string? UserName { get; set; } = string.Empty;

    public string? Role {  get; set; } = string.Empty;


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, BriefUserDto>();
        }
    }
}
