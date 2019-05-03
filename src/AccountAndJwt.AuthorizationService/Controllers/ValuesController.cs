using AccountAndJwt.AuthorizationService.Services.Interfaces;
using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.Contracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Controllers
{
	/// <summary>
	/// Simple values controller
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	public class ValuesController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IValueService _valueService;
		private readonly ILogger<ValuesController> _logger;


		public ValuesController(IMapper mapper, IValueService valueService, ILogger<ValuesController> logger)
		{
			_mapper = mapper;
			_valueService = valueService;
			_logger = logger;
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// [Auth] Retrieves all values
		/// </summary>
		/// <remarks>Awesomeness!</remarks>
		/// <response code="200">Values take it</response>
		/// <response code="500">Oops! Can't get values right now</response>
		[HttpGet]
		[ProducesResponseType(typeof(ValueAm[]), 200)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> GetAll()
		{
			return Ok(_mapper.Map<ValueAm[]>(await _valueService.GetAllAsync()));
		}

		/// <summary>
		/// [Auth] Retrieve specific value by unique id
		/// </summary>
		/// <remarks>Awesomeness!</remarks>
		/// <response code="200">Value founded</response>
		/// <response code="400">Value has missing/invalid values</response>
		/// <response code="500">Oops! Can't get your value right now</response>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ValueAm), 200)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> Get(Int32 id)
		{
			return Ok(_mapper.Map<ValueAm>(await _valueService.GetAsync(id)));
		}

		/// <summary>
		/// [Auth] Add new value
		/// </summary>
		/// <remarks>Awesomeness!</remarks>
		/// <response code="200">Value created</response>
		/// <response code="500">Oops! Can't create your value right now</response>
		[HttpPost]
		[ProducesResponseType(typeof(String), 201)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> Post([FromBody]String value)
		{
			if(String.IsNullOrEmpty(value))
				return BadRequest($"{nameof(value)} is not presented in the request body");

			var valueDto = await _valueService.AddAsync(value);
			_logger.LogInformation($"New value created, value id: {valueDto.Id}");

			return CreatedAtAction(nameof(Get), "Values", new { id = valueDto.Id }, value);
		}

		/// <summary>
		/// [Auth] Change value with desired id
		/// </summary>
		/// <remarks>Awesomeness!</remarks>
		/// <response code="200">Value changed</response>
		/// <response code="400">Value has missing/invalid values</response>
		/// <response code="500">Oops! Can't update your value right now</response>
		[HttpPut]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> Put([FromBody]ValueAm value)
		{
			if(!ModelState.IsValid)
				return BadRequest("Please provide valid data.");

			await _valueService.UpdateAsync(_mapper.Map<ValueDto>(value));
			_logger.LogInformation($"Value with id: {value.Id} updated");

			return Ok();
		}

		/// <summary>
		/// [Auth] Delete value with desired id
		/// </summary>
		/// <remarks>Awesomeness!</remarks>
		/// <response code="200">Value deleted</response>
		/// <response code="400">Value has missing/invalid values</response>
		/// <response code="500">Oops! Can't delete your value right now</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> Delete(Int32 id)
		{
			await _valueService.DeleteAsync(id);
			_logger.LogInformation($"Value deleted, value id: {id}");

			return Ok();
		}
	}
}
