using AccountAndJwt.Api.Database.Interfaces;
using AccountAndJwt.Api.Database.Models;
using AccountAndJwt.Api.Services.Exceptions;
using AccountAndJwt.Api.Services.Interfaces;
using AccountAndJwt.Api.Services.Models;
using AutoMapper;
using System;

namespace AccountAndJwt.Api.Services
{
	internal class ValueService : IValueService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;


		public ValueService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}


		// IValueService //////////////////////////////////////////////////////////////////////////
		public ValueDto[] GetAll()
		{
			return _mapper.Map<ValueDto[]>(_unitOfWork.Values.GetAll());
		}
		public ValueDto Get(Int32 id)
		{
			var requestedValue = _unitOfWork.Values.Get(id);
			if(requestedValue == null)
				throw new ValueNotFoundException($"Value with desired \"{nameof(id)}\" was not found.");

			return _mapper.Map<ValueDto>(requestedValue);
		}
		public ValueDto Add(String value)
		{
			var valueDb = new ValueDb()
			{
				Value = value
			};
			_unitOfWork.Values.Add(valueDb);
			_unitOfWork.Commit();

			return _mapper.Map<ValueDto>(valueDb);
		}
		public void Update(ValueDto value)
		{
			var requestedValue = _unitOfWork.Values.Get(value.Id);
			if(requestedValue == null)
				throw new ValueNotFoundException($"Value with desired \"{nameof(value.Id)}\" was not found.");

			_mapper.Map(value, requestedValue);
			_unitOfWork.Values.Update(requestedValue);
			_unitOfWork.Commit();
		}
		public void Delete(Int32 id)
		{
			var requestedValue = _unitOfWork.Values.Get(id);
			if(requestedValue == null)
				throw new ValueNotFoundException($"Value with desired \"{nameof(id)}\" was not found.");

			_unitOfWork.Values.Remove(requestedValue);
			_unitOfWork.Commit();
		}
	}
}