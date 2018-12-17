using AccountAndJwt.Api.Database.Models;
using System;

namespace AccountAndJwt.Api.Database.Interfaces
{
	public interface IUserRepository : IRepository<UserDb>
	{
		UserDb GetByLoginEager(String login);
		UserDb GetByRefreshTokenEager(String refreshToken);
		UserDb GetEager(Int32 id);
		UserDb[] GetAllEager();
		void AddRole(Int32 userId, Int32 roleId);
		void RemoveRole(Int32 userId, Int32 roleId);
	}
}