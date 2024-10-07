using DynamicObjectCreation.Domain.Entities;
using DynamicObjectCreation.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using System.Text.Json;

namespace DynamicObjectCreation.Infrastructure.Persistence
{
	public class AppDbContext : DbContext, IAppDbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options) { }


		public DbSet<DynamicObject> DynamicObjects { get; set; }
		public DbSet<ValidationRule> ValidationRules { get; set; }



		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
			return await base.SaveChangesAsync(cancellationToken);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseEntity).Assembly);

			modelBuilder.HasDbFunction(typeof(AppDbContext).GetMethod(nameof(JsonbKeyValueExists), new[] { typeof(JsonDocument), typeof(string), typeof(string) }))
				.HasName("jsonb_key_value_exists")
				.HasParameter("jsonbColumn").Metadata.TypeMapping = new NpgsqlJsonTypeMapping("jsonb", typeof(JsonDocument));

			modelBuilder
				.HasDbFunction(typeof(AppDbContext).GetMethod(nameof(JsonbDeepSearch), new[] { typeof(JsonDocument), typeof(string), typeof(string), typeof(bool) }))
				.HasName("jsonb_deep_search")
				.HasParameter("jsonbColumn").Metadata.TypeMapping = new NpgsqlJsonTypeMapping("jsonb", typeof(JsonDocument));
		}

		[DbFunction("jsonb_deep_search")]
		public static bool JsonbDeepSearch(JsonDocument jsonbColumn, string key, string value, bool isStringValue)
		{
			throw new NotImplementedException();
		}

		[DbFunction("jsonb_key_value_exists")]
		public static bool JsonbKeyValueExists(JsonDocument jsonbColumn, string key, string value)
		{
			throw new NotSupportedException("This method is for use with SQL translation only.");
		}
	}
}
