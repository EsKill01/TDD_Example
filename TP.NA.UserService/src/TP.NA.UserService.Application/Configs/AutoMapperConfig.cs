using AutoMapper;
using TP.NA.UserService.Application.Commands.User.Request;
using TP.NA.UserService.Application.Models;
using TP.NA.UserService.Domain.Entities;

namespace TP.NA.UserService.Application.Configs
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UserEntity, CreateUserRequest>().ReverseMap();
            CreateMap<UserEntity, UserModel>().ReverseMap();
        }
    }
}