using AccountAndJwt.Api.Services.Models;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Services.Interfaces
{
	public interface IValueService
	{
		Task<ValueDto[]> GetAllAsync();
		Task<ValueDto> GetAsync(Int32 id);
		Task<ValueDto> AddAsync(String value);
		Task UpdateAsync(ValueDto value);
		Task DeleteAsync(Int32 id);
	}
}