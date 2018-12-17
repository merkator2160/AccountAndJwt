using AccountAndJwt.Api.Database.Interfaces;
using AccountAndJwt.Api.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Api.Database.Mappings
{
	internal class ValueMap : IEntityMap<ValueDb>
	{
		public void Configure(EntityTypeBuilder<ValueDb> entityBuilder)
		{
			entityBuilder.ToTable("Values");
			entityBuilder.HasKey(p => p.Id);
		}
	}
}