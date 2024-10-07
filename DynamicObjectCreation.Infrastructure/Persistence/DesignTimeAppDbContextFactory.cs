using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DynamicObjectCreation.Infrastructure.Persistence
{
	public class DesignTimeAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
	{
		public AppDbContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

			var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"))
				.ConfigureJsonOptions(new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
				})
			.EnableDynamicJson();

			optionsBuilder.UseNpgsql(dataSourceBuilder.Build(), npgsqlOptions =>
			{
				npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
			});

			return new AppDbContext(optionsBuilder.Options);
		}
	}
}
