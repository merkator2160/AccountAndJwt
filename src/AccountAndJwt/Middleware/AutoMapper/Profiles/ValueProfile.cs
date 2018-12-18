using AccountAndJwt.Api.Contracts.Models;
using AccountAndJwt.Api.Database.Models.Storage;
using AccountAndJwt.Api.Services.Models;
using AutoMapper;

namespace AccountAndJwt.Api.Middleware.AutoMapper.Profiles
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