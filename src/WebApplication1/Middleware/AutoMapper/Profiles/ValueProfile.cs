using AccountAndJwt.Api.Contracts.Models;
using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.Database.Models.Storage;
using AutoMapper;

namespace AccountAndJwt.AuthorizationService.Middleware.AutoMapper.Profiles
{
	public class ValueProfile : Profile
	{
		public ValueProfile()
		{
			CreateMap<ValueAm, ValueDto>()
				.ReverseMap();
			CreateMap<ValueDto, ValueDb>()
				.ReverseMap();
		}
	}
}