using AccountAndJwt.Contracts.Models;
using AccountAndJwt.Controllers;
using AccountAndJwt.Middleware.AutoMapper;
using AccountAndJwt.Models.Service;
using AccountAndJwt.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using Xunit;

namespace AccountAndJwt.Tests.Controllers
{
    public class ValuesControllerTests
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ValuesController> _loggerService;


        public ValuesControllerTests()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(typeof(AutoMapperMiddleware).GetTypeInfo().Assembly);     // Dynamically load all configurations
            }).CreateMapper();
            _loggerService = Mock.Of<ILogger<ValuesController>>();
        }


        // TESTS //////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public void GetAll_Values_Test()
        {
            var valueService = Mock.Of<IValueService>(a => a.GetAll() == new[]
            {
                new ValueDto()
                {
                    Id = 1,
                    Value = "value1"
                },
                new ValueDto()
                {
                    Id = 2,
                    Value = "value2"
                }
            });

            var controller = new ValuesController(_mapper, valueService, _loggerService);

            var result = Assert.IsType<OkObjectResult>(controller.GetAll());
            Assert.Equal(200, result.StatusCode);

            var data = Assert.IsAssignableFrom<ValueAm[]>(result.Value);
            Assert.NotNull(data);
            Assert.Equal(2, data.Length);
        }

        [Fact]
        public void GetAll_ReturnNull_Test()
        {
            var valueService = Mock.Of<IValueService>(a => a.GetAll() == null);
            var controller = new ValuesController(_mapper, valueService, _loggerService);

            var result = Assert.IsType<OkObjectResult>(controller.GetAll());
            Assert.Equal(200, result.StatusCode);
            Assert.Null(result.Value);
        }

        [Fact]
        public void Get_Value_Test()
        {
            var valueService = Mock.Of<IValueService>(a => a.Get(3) == new ValueDto()
            {
                Id = 3,
                Value = "value3"
            });
            var controller = new ValuesController(_mapper, valueService, _loggerService);

            var result = Assert.IsType<OkObjectResult>(controller.Get(3));
            Assert.Equal(200, result.StatusCode);
            var data = Assert.IsAssignableFrom<ValueAm>(result.Value);
            Assert.NotNull(data);
            Assert.Equal(3, data.Id);
            Assert.Equal("value3", data.Value);
        }

        [Fact]
        public void Get_ReturnNull_Test()
        {
            var valueService = Mock.Of<IValueService>(a => a.Get(3) == null);
            var controller = new ValuesController(_mapper, valueService, _loggerService);

            var result = Assert.IsType<OkObjectResult>(controller.Get(3));
            Assert.Equal(200, result.StatusCode);
            Assert.Null(result.Value);
        }
    }
}