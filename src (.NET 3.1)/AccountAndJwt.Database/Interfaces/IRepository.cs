using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountAndJwt.Database.Interfaces
{
	public interface IRepository<TEntity> where TEntity : class
	{
		TEntity Get(Object id);
		Task<TEntity> GetAsync(Object id);
		TEntity[] GetAll();
		Task<TEntity[]> GetAllAsync();
		TEntity[] GetChunked(Int32 offset, Int32 amount);
		Task<TEntity[]> GetChunkedAsync(Int32 offset, Int32 amount);
		Int64 GetQuantity();
		Task<Int64> GetQuantityAsync();
		EntityEntry<TEntity> Add(TEntity item);
		Task<EntityEntry<TEntity>> AddAsync(TEntity item);
		void AddRange(IEnumerable<TEntity> items);
		Task AddRangeAsync(IEnumerable<TEntity> items);
		void Update(TEntity item);
		void Delete(Object id);
		Task DeleteAsync(Object id);
		void Remove(TEntity item);
		void RemoveRange(IEnumerable<TEntity> items);
	}
}
