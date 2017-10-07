using AccountAndJwt.Contracts.Models;
using AccountAndJwt.Models.Service;
using AccountAndJwt.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AccountAndJwt.Controllers
{
    /// <summary>
    /// Simple values controller
    /// </summary>
    //[Authorize]
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
        /// Retrieves all values
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Values take it</response>
        /// <response code="500">Oops! Can't get values right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(ValueAm[]), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult GetAll()
        {
            return Ok(_mapper.Map<ValueAm[]>(_valueService.GetAll()));
        }

        /// <summary>
        /// Retrieve specific value by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Value founded</response>
        /// <response code="400">Value has missing/invalid values</response>
        /// <response code="500">Oops! Can't get your value right now</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ValueAm), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult Get(Int32 id)
        {
            try
            {
                return Ok(_mapper.Map<ValueAm>(_valueService.Get(id)));
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add new value
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Value created</response>
        /// <response code="500">Oops! Can't create your value right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(String), 201)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult Post([FromBody]String value)
        {
            if (String.IsNullOrEmpty(value))
                return BadRequest($"{nameof(value)} is not presented in the request body");

            var valueDto = _valueService.Add(value);
            _logger.LogInformation($"New value created, value id: {valueDto.Id}");

            return CreatedAtAction(nameof(Get), "Values", new { id = valueDto.Id }, value);
        }

        /// <summary>
        /// Change value with desired id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Value changed</response>
        /// <response code="400">Value has missing/invalid values</response>
        /// <response code="500">Oops! Can't update your value right now</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(String), 400)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult Put([FromBody]ValueAm value)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Please provide valid data.");

                _valueService.Update(_mapper.Map<ValueDto>(value));
                _logger.LogInformation($"Value with id: {value.Id} updated");

                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete value with desired id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Value deleted</response>
        /// <response code="400">Value has missing/invalid values</response>
        /// <response code="500">Oops! Can't delete your value right now</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(String), 400)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult Delete(Int32 id)
        {
            try
            {
                _valueService.Delete(id);
                _logger.LogInformation($"Value deleted, value id: {id}");

                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
