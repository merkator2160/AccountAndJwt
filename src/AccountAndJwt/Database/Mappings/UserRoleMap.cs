using AccountAndJwt.Api.Database.Interfaces;
using AccountAndJwt.Api.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Api.Database.Mappings
{
	internal class UserRoleMap : IEntityMap<UserRoleDb>
	{
		public void Configure(EntityTypeBuilder<UserRoleDb> entityBuilder)
		{
			entityBuilder.ToTable("UserRoles");
			entityBuilder.HasKey(p => new { p.RoleId, p.UserId });

			entityBuilder
				.HasOne(p => p.Role)
				.WithMany(p => p.UserRoles)
				.HasForeignKey(p => p.RoleId);

			entityBuilder
				.HasOne(p => p.User)
				.WithMany(p => p.UserRoles)
				.HasForeignKey(p => p.UserId);
		}
	}
}