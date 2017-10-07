using AccountAndJwt.Models.Database;
using AccountAndJwt.Models.Service;
using AutoMapper;
using System.Linq;

namespace AccountAndJwt.Middleware.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserDb>()
                .ForMember(from => from.Roles, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(p => p.Role)));
        }
    }
}