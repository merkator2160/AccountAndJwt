using AccountAndJwt.AuthorizationService.Database.Models;
using AccountAndJwt.AuthorizationService.Database.Models.Storage;

namespace AccountAndJwt.AuthorizationService.Database.Interfaces
{
    public interface IValueRepository : IRepository<ValueDb>
    {
        Task<ValueDb> GetByValueAsync(Int32 value);
        Task<PagedValueDb> GetPagedAsync(Int32 pageSize, Int32 pageNumber);
    }
}