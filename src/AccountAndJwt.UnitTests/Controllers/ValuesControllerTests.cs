using AccountAndJwt.AuthorizationService.Controllers;
using AccountAndJwt.AuthorizationService.Services.Interfaces;
using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Contracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace AccountAndJwt.UnitTests.Controllers
{
	// I left this file here as a mock usage sample. It's actually doing nothing. It seems testing controllers like an isolated units not good idea.
	public class ValuesControllerTests
	{
		private readonly IMapper _mapper;


		public ValuesControllerTests()
		{
			_mapper = new MapperConfiguration(cfg =>
			{
				cfg.AddMaps(typeof(AutoMapperModule).GetTypeInfo().Assembly);
			}).CreateMapper();
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
					Value = 1,
					Commentary = "value1"
				},
				new ValueDto()
				{
					Id = 2,
					Value = 2,
					Commentary = "value2"
				}
			}));

			var controller = new ValueController(_mapper, valueService);

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
			var controller = new ValueController(_mapper, valueService);

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
				Value = 3,
				Commentary = "value3"
			}));
			var controller = new ValueController(_mapper, valueService);
			var result = Assert.IsType<OkObjectResult>(controller.Get(3));
			Assert.Equal(200, result.StatusCode);

			var data = Assert.IsAssignableFrom<ValueAm>(result);
			Assert.NotNull(data);
			Assert.Equal(3, data.Id);
			Assert.Equal(3, data.Value);
			Assert.Equal("value3", data.Commentary);
		}

		//[Fact]
		public void GetReturnNullTest()
		{
			var valueService = Mock.Of<IValueService>(a => a.GetAsync(3) == Task.FromResult(default(ValueDto)));
			var controller = new ValueController(_mapper, valueService);

			var result = Assert.IsType<OkObjectResult>(controller.Get(3));
			Assert.Equal(200, result.StatusCode);
			Assert.Null(result.Value);
		}
	}
}