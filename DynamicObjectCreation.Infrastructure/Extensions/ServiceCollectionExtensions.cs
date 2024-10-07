using DynamicObjectCreation.Infrastructure.Interfaces;
using DynamicObjectCreation.Infrastructure.Persistence;
using DynamicObjectCreation.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Text.Json;

namespace DynamicObjectCreation.Infrastructure.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<AppDbContext>((services, options) =>
			{
				var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"))
					.ConfigureJsonOptions(new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
					})
					.EnableDynamicJson();
				options.UseNpgsql(dataSourceBuilder.Build(), npgsqlOptions =>
				{
					npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
				});
			});

			services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

			services.AddMemoryCache();
			services.AddScoped<ICacheService, CacheService>();

			return services;
		}
	}
}
