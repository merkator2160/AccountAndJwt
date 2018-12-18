using AccountAndJwt.Api.Contracts.Models;
using AccountAndJwt.Api.Services.Models;
using AutoMapper;

namespace AccountAndJwt.Api.Middleware.AutoMapper.Profiles
{
	internal class TokenProfile : Profile
	{
		public TokenProfile()
		{
			CreateMap<CreateAccessTokenByRefreshTokenDto, RefreshTokenResponseAm>();
			CreateMap<CreateAccessTokenByCredentialsDto, AuthorizeByCredentialsResponseAm>();
		}
	}
}