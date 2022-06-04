using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Ui.Models.ViewModels;
using AutoMapper;

namespace AccountAndJwt.Ui.Services.AutoMapper.Profiles
{
    internal class ValueProfile : Profile
    {
        public ValueProfile()
        {
            CreateMap<ValueAm, GridValueVm>();
            CreateMap<GridValueVm, UpdateValueRequestAm>();
            CreateMap<GridValueVm, AddValueRequestAm>();
        }
    }
}