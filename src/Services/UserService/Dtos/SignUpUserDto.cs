using AutoMapper;
using UserService.Models;

namespace UserService.Dtos;

public class SignUpUserDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SignUpUserDto, User>();
        }
    }
}
