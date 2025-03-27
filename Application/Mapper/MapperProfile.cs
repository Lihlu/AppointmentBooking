using Domain.Models.Entities;
using AutoMapper;
using Application.Models;

namespace Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //Source => Destination
            CreateMap<UserDto, User>().ForMember(dest => dest.PasswordHash, input => input.MapFrom(i => i.Password))
                .ReverseMap();

            CreateMap<LoginUserDto, User>().ForMember(dest => dest.PasswordHash, input => input.MapFrom(i => i.Password)).ReverseMap();


        }
    }
}
