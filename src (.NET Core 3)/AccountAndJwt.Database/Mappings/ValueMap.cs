using AccountAndJwt.Common.Interfaces;
using AccountAndJwt.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Database.Mappings
{
	internal class ValueMap : IEntityMap<ValueDb>
	{
		public void Configure(EntityTypeBuilder<ValueDb> entityBuilder)
		{
			entityBuilder
				.ToTable("Values")
				.HasKey(p => p.Id);
		}
	}
}