using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models;
using AccountAndJwt.Database.Models.Storage;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace AccountAndJwt.Database.Repositories
{
    internal class ValueRepository : EfRepositoryBase<ValueDb, DataContext>, IValueRepository
    {
        private readonly IMapper _mapper;


        public ValueRepository(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }


        // IValueRepository ///////////////////////////////////////////////////////////////////////
        public Task<ValueDb> GetByValueAsync(Int32 value)
        {
            return Context.Values.FirstOrDefaultAsync(p => p.Value == value);
        }
        public async Task<PagedValueDb> GetPagedAsync(Int32 pageSize, Int32 pageNumber)
        {
            return _mapper.Map<PagedValueDb>(await Context.Values.ToPagedListAsync(pageNumber, pageSize));
        }
    }
}