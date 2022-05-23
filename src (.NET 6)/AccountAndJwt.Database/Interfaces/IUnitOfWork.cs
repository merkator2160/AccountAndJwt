namespace AccountAndJwt.Database.Interfaces
{
    public interface IUnitOfWork
    {
        IValueRepository Values { get; }
        IUserRepository Users { get; }

        Int32 Commit();
        Task<Int32> CommitAsync();
    }
}