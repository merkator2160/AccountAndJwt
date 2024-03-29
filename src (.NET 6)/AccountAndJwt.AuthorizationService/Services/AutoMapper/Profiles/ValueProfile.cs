﻿using AccountAndJwt.AuthorizationService.Database.Models;
using AccountAndJwt.AuthorizationService.Database.Models.Storage;
using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AutoMapper;

namespace AccountAndJwt.AuthorizationService.Services.AutoMapper.Profiles
{
    internal class ValueProfile : Profile
    {
        public ValueProfile()
        {
            CreateMap<UpdateValueRequestAm, ValueDb>();
            CreateMap<AddValueRequestAm, ValueDb>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ValueDb, ValueAm>()
                .ReverseMap();

            CreateMap<PagedValueDb, PagedValueResponseAm>();
        }
    }
}