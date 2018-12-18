using AccountAndJwt.Api.Database.Interfaces;
using AccountAndJwt.Api.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Api.Database.Mappings
{
	internal class UserMap : IEntityMap<UserDb>
	{
		public void Configure(EntityTypeBuilder<UserDb> entityBuilder)
		{
			entityBuilder.ToTable("Users");
			entityBuilder.HasKey(p => p.Id);
		}
	}
}