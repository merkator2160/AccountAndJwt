using AccountAndJwt.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public virtual TEntity[] GetAll()
        {
            return _context.Set<TEntity>().ToArray();
        }
        public virtual EntityEntry<TEntity> Add(TEntity item)
        {
            return _context.Set<TEntity>().Add(item);
        }
        public virtual void AddRange(IEnumerable<TEntity> items)
        {
            _context.Set<TEntity>().AddRange(items);
        }
        public virtual void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
        public virtual void Delete(Object id)
        {
            var item = _context.Set<TEntity>().Find(id);
            if (item != null)
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