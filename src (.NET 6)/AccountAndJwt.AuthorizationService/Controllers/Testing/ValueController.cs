using AccountAndJwt.AuthorizationService.Database.Interfaces;
using AccountAndJwt.AuthorizationService.Database.Models.Storage;
using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Errors;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountAndJwt.AuthorizationService.Controllers.Testing
{
    /// <summary>
    /// RESTful values controller
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ValueController : ControllerBase
    {
        private const UInt16 _pageSizeLimit = 1000;

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public ValueController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        // ACTIONS ////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// [Auth] Retrieves some values, paged
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Values take it</response>
        /// <response code="401">Unauthorized without JWT token</response>
        /// <response code="460">Business logic validation exception</response>
        /// <response code="500">Server side error</response>
        [HttpGet("{pageSize}/{pageNumber}")]
        [ProducesResponseType(typeof(PagedValueResponseAm), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> GetPaged([FromRoute] Int32 pageSize, [FromRoute] Int32 pageNumber)
        {
            if (pageSize <= 0 || pageSize > _pageSizeLimit)
                return StatusCode(460, $"Page size min: 1, max: {_pageSizeLimit}");

            if (pageNumber <= 0 || pageNumber > UInt16.MaxValue)
                return StatusCode(460, $"Page number min: 1, max: {UInt16.MaxValue}");

            return Ok(_mapper.Map<PagedValueResponseAm>(await _unitOfWork.Values.GetPagedAsync(pageSize, pageNumber)));
        }

        /// <summary>
        /// [Auth] Retrieve specific value by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Value founded</response>
        /// <response code="401">Unauthorized without JWT token</response>
        /// <response code="460">Business logic validation exception</response>
        /// <response code="500">Server side error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ValueAm), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> Get([FromRoute] Int32 id)
        {
            var requestedValue = await _unitOfWork.Values.GetAsync(id);
            if (requestedValue == null)
                return StatusCode(460, $"Value with desired \"{nameof(id)}\" was not found!");

            return Ok(_mapper.Map<ValueAm>(requestedValue));
        }

        /// <summary>
        /// [Auth] Add new value
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="201">Value created</response>
        /// <response code="400">Value has missing/invalid values</response>
        /// <response code="401">Unauthorized without JWT token</response>
        /// <response code="460">Business logic validation exception</response>
        /// <response code="500">Server side error</response>
        [HttpPost]
        [ProducesResponseType(typeof(String), 201)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> Post([FromBody] AddValueRequestAm value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var storedValue = await _unitOfWork.Values.GetByValueAsync(value.Value);
            if (storedValue != null)
                return StatusCode(460, $"Value with the same value \"{nameof(value.Value)}\" is already exists!");

            var valueDb = _mapper.Map<ValueDb>(value);
            await _unitOfWork.Values.AddAsync(valueDb);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(Get), "Value", new { id = valueDb.Id }, value);
        }

        /// <summary>
        /// [Auth] Change value with desired id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Value changed</response>
        /// <response code="400">Value has missing/invalid values</response>
        /// <response code="401">Unauthorized without JWT token</response>
        /// <response code="415">Unsupported media type</response>
        /// <response code="460">Business logic validation exception</response>
        /// <response code="500">Server side error</response>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> Put([FromBody] UpdateValueRequestAm value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var requestedValue = await _unitOfWork.Values.GetAsync(value.Id);
            if (requestedValue == null)
                return StatusCode(460, $"Value with desired \"{nameof(value.Id)}\" was not found!");

            _mapper.Map(value, requestedValue);
            _unitOfWork.Values.Update(requestedValue);
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        /// <summary>
        /// [Auth] Delete value with desired id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Value deleted</response>
        /// <response code="401">Unauthorized without JWT token</response>
        /// <response code="460">Business logic validation exception</response>
        /// <response code="500">Server side error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> Delete([FromRoute] Int32 id)
        {
            var requestedValue = await _unitOfWork.Values.GetAsync(id);
            if (requestedValue == null)
                return StatusCode(460, $"Value with desired \"{nameof(id)}\" was not found!");

            _unitOfWork.Values.Remove(requestedValue);
            await _unitOfWork.CommitAsync();

            return Ok();
        }
    }
}
