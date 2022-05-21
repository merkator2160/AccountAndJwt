using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Database.Models.Storage;
using AutoMapper;

namespace AccountAndJwt.AuthorizationService.Services.AutoMapper.Profiles
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserAm>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UserDto, UserDb>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(p => p.Role.Name)));

            CreateMap<RegisterUserAm, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<RoleDb, RoleAm>()
                .ReverseMap()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
        }
    }
}