using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.Contracts.Models;
using AccountAndJwt.Database.Models.Storage;
using AutoMapper;

namespace AccountAndJwt.AuthorizationService.Middleware.AutoMapper.Profiles
{
	internal class ValueProfile : Profile
	{
		public ValueProfile()
		{
			CreateMap<AddValueAm, ValueDto>()
				.ForMember(dest => dest.Id, opt => opt.Ignore());
			CreateMap<ValueAm, ValueDto>()
				.ReverseMap();
			CreateMap<ValueDto, ValueDb>()
				.ReverseMap();
		}
	}
}