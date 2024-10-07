using DynamicObjectCreation.Domain.Entities;
using DynamicObjectCreation.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DynamicObjectCreation.Infrastructure.Services
{
	public class CacheService : ICacheService
	{
		private readonly IMemoryCache _cache;
		private readonly IAppDbContext _context;

		public CacheService(IMemoryCache cache, IAppDbContext context)
		{
			_cache = cache;
			_context = context;
		}

		public async Task<List<ValidationRule>> GetValidationRulesFromCacheAsync()
		{
			if (!_cache.TryGetValue("ValidationRules", out List<ValidationRule> validationRules))
			{
				validationRules = await _context.ValidationRules.Where(r => r.IsActive && !r.IsDeleted).AsNoTracking().ToListAsync();
				_cache.Set("ValidationRules", validationRules, TimeSpan.FromHours(1));
			}

			return validationRules;
		}

		public async Task LoadValidationRulesToCacheAsync(CancellationToken cancellationToken = default)
		{
			var validationRules = await _context.ValidationRules.Where(r => r.IsActive && !r.IsDeleted).AsNoTracking().ToListAsync();
			_cache.Set("ValidationRules", validationRules, TimeSpan.FromHours(1));
		}

		public async Task UpdateValidationRuleInCacheAsync(Guid ruleId)
		{
			var rule = await _context.ValidationRules.FindAsync(ruleId);
			if (rule != null)
			{
				var currentRules = _cache.Get<List<ValidationRule>>("ValidationRules");
				var existingRule = currentRules.FirstOrDefault(r => r.Id == ruleId);
				if (existingRule != null)
				{
					currentRules.Remove(existingRule);
				}
				currentRules.Add(rule);
				_cache.Set("ValidationRules", currentRules, TimeSpan.FromHours(1));
			}
		}
	}
}
