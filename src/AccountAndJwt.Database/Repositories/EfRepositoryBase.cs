using AccountAndJwt.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAndJwt.Database.Repositories
{
	internal abstract class EfRepositoryBase<TEntity, TContext> : IRepository<TEntity> where TEntity : class where TContext : DataContext
	{
		private readonly DataContext _context;


		protected EfRepositoryBase(TContext context)
		{
			_context = context;
		}


		// PROPERTIES /////////////////////////////////////////////////////////////////////////////
		protected TContext Context => _context as TContext;


		// IRepository<TEntity> //////////////////////////////////////////////////////////////////
		public virtual TEntity Get(Object id)
		{
			return _context.Set<TEntity>().Find(id);
		}
		public virtual Task<TEntity> GetAsync(Object id)
		{
			return _context.Set<TEntity>().FindAsync(id);
		}
		public virtual TEntity[] GetAll()
		{
			return _context.Set<TEntity>().ToArray();
		}
		public virtual Task<TEntity[]> GetAllAsync()
		{
			return _context.Set<TEntity>().ToArrayAsync();
		}
		public virtual TEntity[] GetChunked(Int32 offset, Int32 amount)
		{
			return _context.Set<TEntity>().Skip(offset).Take(amount).ToArray();
		}
		public virtual Task<TEntity[]> GetChunkedAsync(Int32 offset, Int32 amount)
		{
			return _context.Set<TEntity>().Skip(offset).Take(amount).ToArrayAsync();
		}
		public virtual Int64 GetQuantity()
		{
			return Context.Set<TEntity>().LongCount();
		}
		public virtual Task<Int64> GetQuantityAsync()
		{
			return Context.Set<TEntity>().LongCountAsync();
		}
		public virtual EntityEntry<TEntity> Add(TEntity item)
		{
			return _context.Set<TEntity>().Add(item);
		}
		public virtual Task<EntityEntry<TEntity>> AddAsync(TEntity item)
		{
			return _context.Set<TEntity>().AddAsync(item);
		}
		public virtual void AddRange(IEnumerable<TEntity> items)
		{
			_context.Set<TEntity>().AddRange(items);
		}
		public virtual Task AddRangeAsync(IEnumerable<TEntity> items)
		{
			return _context.Set<TEntity>().AddRangeAsync(items);
		}
		public virtual void Update(TEntity item)
		{
			_context.Entry(item).State = EntityState.Modified;
		}
		public virtual void Delete(Object id)
		{
			var item = _context.Set<TEntity>().Find(id);
			if(item != null)
				_context.Set<TEntity>().Remove(item);
		}
		public virtual async Task DeleteAsync(Object id)
		{
			var item = await _context.Set<TEntity>().FindAsync(id);
			if(item != null)
				_context.Set<TEntity>().Remove(item);
		}
		public virtual void Remove(TEntity item)
		{
			_context.Set<TEntity>().Remove(item);
		}
		public virtual void RemoveRange(IEnumerable<TEntity> items)
		{
			_context.Set<TEntity>().RemoveRange(items);
		}
	}
}