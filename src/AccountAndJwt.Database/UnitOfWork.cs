using AccountAndJwt.Database.Interfaces;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Database
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly DataContext _context;


		public UnitOfWork(
			DataContext context,
			IValueRepository valueRepository,
			IUserRepository userRepository)
		{
			_context = context;
			Values = valueRepository;
			Users = userRepository;
		}


		// IUnitOfWork ////////////////////////////////////////////////////////////////////////////
		public IValueRepository Values { get; }
		public IUserRepository Users { get; }


		// IDisposable ////////////////////////////////////////////////////////////////////////////
		public void Dispose()
		{
			_context?.Dispose();
		}


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		public Int32 Commit()
		{
			return _context.SaveChanges();
		}
		public Task<Int32> CommitAsync()
		{
			return _context.SaveChangesAsync();
		}
	}
}