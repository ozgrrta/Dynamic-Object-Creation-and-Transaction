using DynamicObjectCreation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DynamicObjectCreation.Infrastructure.Interfaces
{
	public interface IAppDbContext
	{
		DbSet<DynamicObject> DynamicObjects { get; set; }
		DbSet<ValidationRule> ValidationRules { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
