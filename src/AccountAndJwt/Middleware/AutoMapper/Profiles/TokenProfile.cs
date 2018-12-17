using AccountAndJwt.Api.Services.Models;
using AccountAndJwt.Contracts.Models;
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