using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace AccountAndJwt.Api.Database.Interfaces
{
	public interface IRepository<TEntity> where TEntity : class
	{
		TEntity Get(Object id);
		TEntity[] GetAll();
		EntityEntry<TEntity> Add(TEntity item);
		void AddRange(IEnumerable<TEntity> items);
		void Update(TEntity item);
		void Remove(TEntity item);
		void RemoveRange(IEnumerable<TEntity> items);
	}
}
