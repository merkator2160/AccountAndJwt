using AccountAndJwt.Database.Models;
using AccountAndJwt.Database.Models.Storage;

namespace AccountAndJwt.Database.Interfaces
{
    public interface IValueRepository : IRepository<ValueDb>
    {
        Task<ValueDb> GetByValueAsync(Int32 value);
        Task<PagedValueDb> GetPagedAsync(Int32 pageSize, Int32 pageNumber);
    }
}