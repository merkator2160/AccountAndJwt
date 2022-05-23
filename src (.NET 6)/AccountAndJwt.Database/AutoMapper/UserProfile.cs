using AccountAndJwt.Database.Models;
using AccountAndJwt.Database.Models.Storage;
using AutoMapper;
using X.PagedList;

namespace AccountAndJwt.Database.AutoMapper
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<IPagedList<UserDb>, PagedUserDb>()
                .ForMember(p => p.Users, opt => opt.MapFrom(p => p.ToArray()));
        }
    }
}