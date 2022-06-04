using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Ui.Models.ViewModels;
using AutoMapper;

namespace AccountAndJwt.Ui.Services.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAm, GridUserVm>();
        }
    }
}