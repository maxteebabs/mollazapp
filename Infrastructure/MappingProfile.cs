using AutoMapper;
using WebApplication.Models;

namespace WebApplication.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, User>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                    Link.To(nameof(Controllers.UserController.GetUserById),
                        new {userId = src.Id})));

//                .ForMember(dest => dest.Location.City
//                    , opt => opt.MapFrom(src => src.City));
//                .ForMember(dest => dest.Location.Country
//                    , opt=> opt.MapFrom(src => src.Country));
        }
    }
}