using AccountAndJwt.Contracts.Models;
using AccountAndJwt.Models.Service;
using AutoMapper;

namespace AccountAndJwt.Middleware.AutoMapper.Profiles
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