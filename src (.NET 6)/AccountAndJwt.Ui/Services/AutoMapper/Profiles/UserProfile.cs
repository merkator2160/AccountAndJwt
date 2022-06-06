using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Ui.Models.ViewModels;
using AutoMapper;

namespace AccountAndJwt.Ui.Services.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAm, GridUserVm>()
                .ForMember(dest => dest.RoleList, opt => opt.MapFrom(src => new List<RoleAm>(src.Roles)));

            CreateMap<GridUserVm, RegisterUserRequestAm>()
                .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.Password));

            CreateMap<GridUserVm, ChangeNameRequestAm>();
        }
    }
}