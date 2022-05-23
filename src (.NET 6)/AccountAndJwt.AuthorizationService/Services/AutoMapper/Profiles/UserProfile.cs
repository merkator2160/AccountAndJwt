using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AccountAndJwt.Database.Models;
using AccountAndJwt.Database.Models.Storage;
using AutoMapper;

namespace AccountAndJwt.AuthorizationService.Services.AutoMapper.Profiles
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDb, UserAm>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(p => p.Role)));

            CreateMap<RegisterUserRequestAm, UserDb>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<RoleDb, RoleAm>()
                .ReverseMap()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            CreateMap<PagedUserDb, PagedUserResponseAm>();
        }
    }
}