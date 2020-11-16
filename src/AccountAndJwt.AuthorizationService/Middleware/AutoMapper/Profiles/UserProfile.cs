using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.Contracts.Models;
using AccountAndJwt.Database.Models.Storage;
using AutoMapper;
using System.Linq;

namespace AccountAndJwt.AuthorizationService.Middleware.AutoMapper.Profiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<UserDto, UserAm>()
				.ForMember(from => from.PasswordHash, opt => opt.Ignore())
				.ReverseMap();

			CreateMap<UserDto, UserDb>()
				.ForMember(from => from.UserRoles, opt => opt.Ignore())
				.ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
				.ForMember(from => from.PasswordHash, opt => opt.Ignore())
				.ReverseMap()
				.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(p => p.Role.Name)));

			CreateMap<RegisterUserAm, UserDto>()
				.ForMember(from => from.Id, opt => opt.Ignore())
				.ForMember(from => from.Roles, opt => opt.Ignore());
		}
	}
}