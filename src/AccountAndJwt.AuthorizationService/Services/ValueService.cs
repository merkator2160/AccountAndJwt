using AccountAndJwt.AuthorizationService.Services.Exceptions;
using AccountAndJwt.AuthorizationService.Services.Interfaces;
using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models.Storage;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Services
{
	internal sealed class ValueService : IValueService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;


		public ValueService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}


		// IValueService //////////////////////////////////////////////////////////////////////////
		public async Task<ValueDto[]> GetAllAsync()
		{
			return _mapper.Map<ValueDto[]>(await _unitOfWork.Values.GetAllAsync());
		}
		public async Task<ValueDto> GetAsync(Int32 id)
		{
			var requestedValue = await _unitOfWork.Values.GetAsync(id);
			if(requestedValue == null)
				throw new ValueNotFoundException($"Value with desired \"{nameof(id)}\" was not found.");

			return _mapper.Map<ValueDto>(requestedValue);
		}
		public async Task<ValueDto> AddAsync(String value)
		{
			var valueDb = new ValueDb()
			{
				Value = value
			};
			await _unitOfWork.Values.AddAsync(valueDb);
			await _unitOfWork.CommitAsync();

			return _mapper.Map<ValueDto>(valueDb);
		}
		public async Task UpdateAsync(ValueDto value)
		{
			var requestedValue = await _unitOfWork.Values.GetAsync(value.Id);
			if(requestedValue == null)
				throw new ValueNotFoundException($"Value with desired \"{nameof(value.Id)}\" was not found.");

			_mapper.Map(value, requestedValue);
			_unitOfWork.Values.Update(requestedValue);
			await _unitOfWork.CommitAsync();
		}
		public async Task DeleteAsync(Int32 id)
		{
			var requestedValue = await _unitOfWork.Values.GetAsync(id);
			if(requestedValue == null)
				throw new ValueNotFoundException($"Value with desired \"{nameof(id)}\" was not found.");

			_unitOfWork.Values.Remove(requestedValue);
			await _unitOfWork.CommitAsync();
		}
	}
}