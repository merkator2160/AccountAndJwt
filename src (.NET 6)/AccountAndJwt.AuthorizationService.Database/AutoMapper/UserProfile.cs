using AccountAndJwt.AuthorizationService.Database.Models;
using AccountAndJwt.AuthorizationService.Database.Models.Storage;
using AutoMapper;
using X.PagedList;

namespace AccountAndJwt.AuthorizationService.Database.AutoMapper
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