using AccountAndJwt.Api.Database.Models;
using AccountAndJwt.Api.Services.Models;
using AccountAndJwt.Contracts.Models;
using AutoMapper;
using System.Linq;

namespace AccountAndJwt.Api.Middleware.AutoMapper.Profiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<UserDto, UserDb>()
				.ForMember(from => from.UserRoles, opt => opt.Ignore())
				.ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
				.ReverseMap()
				.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(p => p.Role.RoleName)));
			CreateMap<UserDto, UserAm>()
				.ReverseMap();
			CreateMap<RegisterUserAm, UserDto>()
				.ForMember(from => from.Id, opt => opt.Ignore())
				.ForMember(from => from.Roles, opt => opt.Ignore());
		}
	}
}