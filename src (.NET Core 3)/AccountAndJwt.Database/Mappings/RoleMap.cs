using AccountAndJwt.Common.Interfaces;
using AccountAndJwt.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Database.Mappings
{
	internal class RoleMap : IEntityMap<RoleDb>
	{
		public void Configure(EntityTypeBuilder<RoleDb> entityBuilder)
		{
			entityBuilder
				.ToTable("Roles")
				.HasKey(p => p.Id);

			entityBuilder.Property(e => e.Id).ValueGeneratedNever();
		}
	}
}