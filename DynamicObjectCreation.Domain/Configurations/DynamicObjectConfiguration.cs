using DynamicObjectCreation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicObjectCreation.Domain.Configurations
{
	public class DynamicObjectConfiguration : IEntityTypeConfiguration<DynamicObject>
	{
		public void Configure(EntityTypeBuilder<DynamicObject> builder)
		{
			builder.HasIndex(e => e.ObjectType);
			builder.HasIndex(e => e.CreatedAt);

			builder.HasIndex(e => e.Data)
				.HasMethod("GIN")
				.HasDatabaseName("idx_data_gin");

		}
	}
}
