namespace AccountAndJwt.Database.Interfaces
{
    public interface IUnitOfWork
    {
        IValueRepository Values { get; }
        IUserRepository Users { get; }
        ITokenRepository Tokens { get; }

        void Commit();
    }
}