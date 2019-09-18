using AccountAndJwt.Common.Interfaces;
using AccountAndJwt.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Database.Mappings
{
	internal class UserMap : IEntityMap<UserDb>
	{
		public void Configure(EntityTypeBuilder<UserDb> entityBuilder)
		{
			entityBuilder
				.ToTable("Users")
				.HasKey(p => p.Id);
		}
	}
}