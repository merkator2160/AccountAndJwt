using AccountAndJwt.Api.Contracts.Models;
using AccountAndJwt.AuthorizationService.Controllers;
using AccountAndJwt.AuthorizationService.Services.Interfaces;
using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.Common.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace AccountAndJwt.UnitTests.Controllers
{
	public class ValuesControllerTests
	{
		private readonly IMapper _mapper;
		private readonly ILogger<ValuesController> _loggerService;


		public ValuesControllerTests()
		{
			_mapper = new MapperConfiguration(cfg =>
			{
				cfg.AddMaps(typeof(AutoMapperModule).GetTypeInfo().Assembly);     // Dynamically load all configurations
			}).CreateMapper();
			_loggerService = Mock.Of<ILogger<ValuesController>>();
		}


		// TESTS //////////////////////////////////////////////////////////////////////////////////
		//[Fact]
		public void GetAllValuesTest()
		{
			var valueService = Mock.Of<IValueService>(a => a.GetAllAsync() == Task.FromResult(new[]
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
			}));

			var controller = new ValuesController(_mapper, valueService, _loggerService);

			var result = Assert.IsType<OkObjectResult>(controller.GetAll());
			Assert.Equal(200, result.StatusCode);

			var data = Assert.IsAssignableFrom<ValueAm[]>(result.Value);
			Assert.NotNull(data);
			Assert.Equal(2, data.Length);
		}

		//[Fact]
		public void GetAllReturnNullTest()
		{
			var valueService = Mock.Of<IValueService>(a => a.GetAllAsync() == Task.FromResult(default(ValueDto[])));
			var controller = new ValuesController(_mapper, valueService, _loggerService);

			var result = Assert.IsType<OkObjectResult>(controller.GetAll());
			Assert.Equal(200, result.StatusCode);
			Assert.Null(result.Value);
		}

		//[Fact]
		public void GetValueTest()
		{
			var valueService = Mock.Of<IValueService>(a => a.GetAsync(3) == Task.FromResult(new ValueDto()
			{
				Id = 3,
				Value = "value3"
			}));
			var controller = new ValuesController(_mapper, valueService, _loggerService);

			var result = Assert.IsType<OkObjectResult>(controller.Get(3));
			Assert.Equal(200, result.StatusCode);
			var data = Assert.IsAssignableFrom<ValueAm>(result.Value);
			Assert.NotNull(data);
			Assert.Equal(3, data.Id);
			Assert.Equal("value3", data.Value);
		}

		//[Fact]
		public void GetReturnNullTest()
		{
			var valueService = Mock.Of<IValueService>(a => a.GetAsync(3) == Task.FromResult(default(ValueDto)));
			var controller = new ValuesController(_mapper, valueService, _loggerService);

			var result = Assert.IsType<OkObjectResult>(controller.Get(3));
			Assert.Equal(200, result.StatusCode);
			Assert.Null(result.Value);
		}
	}
}