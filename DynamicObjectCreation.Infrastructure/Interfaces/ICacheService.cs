using DynamicObjectCreation.Domain.Entities;

namespace DynamicObjectCreation.Infrastructure.Interfaces
{
	public interface ICacheService
	{
		Task<List<ValidationRule>> GetValidationRulesFromCacheAsync();
		Task LoadValidationRulesToCacheAsync(CancellationToken cancellationToken = default);
		Task UpdateValidationRuleInCacheAsync(Guid ruleId);
	}
}
