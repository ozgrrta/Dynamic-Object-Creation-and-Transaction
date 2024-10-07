using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DynamicObjectCreation.Domain.Entities;

namespace DynamicObjectCreation.Domain.Configurations
{
	public class ValidationRuleConfiguration : IEntityTypeConfiguration<ValidationRule>
	{
		public void Configure(EntityTypeBuilder<ValidationRule> builder)
		{
			builder.HasIndex(e => e.ObjectType);

			builder.Property(e => e.ObjectType)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(e => e.PropertyName)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(e => e.RuleType)
				.IsRequired();

			builder.Property(e => e.ParentPropertyName)
				.IsRequired(false)
				.HasMaxLength(50);

			builder.Property(e => e.ChildObjectType)
				.IsRequired(false)
				.HasMaxLength(50);
		}
	}
}
