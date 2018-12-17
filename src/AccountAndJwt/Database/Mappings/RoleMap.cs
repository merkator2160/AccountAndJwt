using AccountAndJwt.Api.Database.Interfaces;
using AccountAndJwt.Api.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Api.Database.Mappings
{
	internal class RoleMap : IEntityMap<RoleDb>
	{
		public void Configure(EntityTypeBuilder<RoleDb> entityBuilder)
		{
			entityBuilder.ToTable("UserRoles");
			entityBuilder.HasKey(p => p.Id);
		}
	}
}