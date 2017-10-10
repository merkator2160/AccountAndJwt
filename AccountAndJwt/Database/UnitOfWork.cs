using AccountAndJwt.Database.Interfaces;

namespace AccountAndJwt.Database
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;


        public UnitOfWork(DataContext context, IValueRepository valueRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _context = context;
            Values = valueRepository;
            Users = userRepository;
            Roles = roleRepository;
        }


        // IUnitOfWork ////////////////////////////////////////////////////////////////////////////
        public IValueRepository Values { get; }
        public IUserRepository Users { get; }
        public IRoleRepository Roles { get; }


        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
