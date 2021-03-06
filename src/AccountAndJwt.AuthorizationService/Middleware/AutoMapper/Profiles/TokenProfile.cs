﻿using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.Contracts.Models;
using AutoMapper;

namespace AccountAndJwt.AuthorizationService.Middleware.AutoMapper.Profiles
{
	internal class TokenProfile : Profile
	{
		public TokenProfile()
		{
			CreateMap<CreateAccessTokenByRefreshTokenDto, RefreshTokenResponseAm>();
			CreateMap<CreateAccessTokenByCredentialsDto, AuthorizeResponseAm>();
		}
	}
}