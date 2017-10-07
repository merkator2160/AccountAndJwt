using AccountAndJwt.Database.Interfaces;

namespace AccountAndJwt.Database
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;


        public UnitOfWork(DataContext context, IValueRepository valueRepository, IUserRepository userRepository, ITokenRepository tokenRepository)
        {
            _context = context;
            Values = valueRepository;
            Users = userRepository;
            Tokens = tokenRepository;
        }


        // IUnitOfWork ////////////////////////////////////////////////////////////////////////////
        public IValueRepository Values { get; }
        public IUserRepository Users { get; }
        public ITokenRepository Tokens { get; }


        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
