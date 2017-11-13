using AccountAndJwt.Contracts.Models;
using AccountAndJwt.Database.Models;
using AccountAndJwt.Services.Models;
using AutoMapper;

namespace AccountAndJwt.Middleware.AutoMapper.Profiles
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