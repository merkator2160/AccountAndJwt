using AccountAndJwt.Contracts.Models;
using AccountAndJwt.Models.Database;
using AccountAndJwt.Models.Service;
using AutoMapper;

namespace AccountAndJwt.Middleware.AutoMapper.Profiles
{
    public class ValueProfile : Profile
    {
        public ValueProfile()
        {
            CreateMap<ValueAm, ValueDto>();
            CreateMap<ValueDto, ValueDb>()
                .ReverseMap();
        }
    }
}