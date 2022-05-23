using AccountAndJwt.Database.Models;
using AccountAndJwt.Database.Models.Storage;
using AutoMapper;
using X.PagedList;

namespace AccountAndJwt.Database.AutoMapper
{
    public class ValueProfile : Profile
    {
        public ValueProfile()
        {
            CreateMap<IPagedList<ValueDb>, PagedValueDb>()
                .ForMember(p => p.Values, opt => opt.MapFrom(p => p.ToArray()));
        }
    }
}