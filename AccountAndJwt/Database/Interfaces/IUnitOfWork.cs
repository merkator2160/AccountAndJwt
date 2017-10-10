namespace AccountAndJwt.Database.Interfaces
{
    public interface IUnitOfWork
    {
        IValueRepository Values { get; }
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }

        void Commit();
    }
}