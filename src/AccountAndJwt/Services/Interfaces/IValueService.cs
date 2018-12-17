using AccountAndJwt.Api.Services.Models;
using System;

namespace AccountAndJwt.Api.Services.Interfaces
{
	public interface IValueService
	{
		ValueDto[] GetAll();
		void Delete(Int32 id);
		void Update(ValueDto value);
		ValueDto Add(String value);
		ValueDto Get(Int32 id);
	}
}