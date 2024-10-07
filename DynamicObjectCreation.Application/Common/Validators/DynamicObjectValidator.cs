using FluentValidation;
using DynamicObjectCreation.Domain.Enums;
using System.Text.Json;
using DynamicObjectCreation.Domain.Entities;
using JsonSerializer = System.Text.Json.JsonSerializer;
using DynamicObjectCreation.Application.Extensions;
using System.Data;
using DynamicObjectCreation.Infrastructure.Interfaces;

namespace DynamicObjectCreation.Application.Common.Validators
{
	public class DynamicObjectValidator : AbstractValidator<DynamicObject>
	{
		private readonly ICacheService _cacheService;

		public DynamicObjectValidator(ICacheService cacheService)
		{
			_cacheService = cacheService;

			RuleFor(dynamicObject => dynamicObject)
				.MustAsync(async (dynamicObject, obj, context, cancellation) =>
				{
					var rules = await _cacheService.GetValidationRulesFromCacheAsync();

					JsonDocument document = JsonDocument.Parse(JsonSerializer.Serialize(dynamicObject));
					JsonElement dynamicObjectElement = document.RootElement;

					var validationRules = new List<ValidationRule>();

					var rulesToAdd = rules.Where(r => r.ObjectType == dynamicObject.ObjectType).ToList();

					while (rulesToAdd.Count() > 0)
					{
						validationRules.AddRange(rulesToAdd);

						rulesToAdd = rules.Where(r => validationRules.Any(v => v.ChildObjectType == r.ObjectType)).ToList();
						rulesToAdd = rulesToAdd.Where(r => !validationRules.Any(v => v.ObjectType == r.ObjectType)).ToList();
					}

					validationRules.RemoveAll(r => validationRules.Exists(v => v.ObjectType == r.ObjectType &&
							v.PropertyName == r.PropertyName &&
							string.IsNullOrEmpty(r.ParentPropertyName) &&
							!string.IsNullOrEmpty(v.ParentPropertyName) &&
							r.RuleType == ValidationRuleType.Required));

					foreach (var rule in validationRules)
					{
						if (!string.IsNullOrEmpty(rule.ChildObjectType))
						{
							continue;
						}

						string errorMessage = string.Empty;
						JsonElement element = new();

						var parentPropertyNames = validationRules.Where(r => r.ChildObjectType == rule.ObjectType).Select(r => r.PropertyName).ToList();

						string propertyName = rule.ParentPropertyName ?? rule.PropertyName;

						foreach (var parentPropertyName in parentPropertyNames)
						{
							rule.ParentPropertyName = parentPropertyName;

							propertyName = rule.ParentPropertyName ?? rule.PropertyName;

							dynamicObjectElement.TryFindValueByCaseIgnoredKey(propertyName, out element);

							if (!ValidateProperty(element, rule, out errorMessage))
							{
								context.AddFailure(rule.PropertyName, errorMessage);
								return false;
							}
						}

						dynamicObjectElement.TryFindValueByCaseIgnoredKey(propertyName, out element);

						if (!ValidateProperty(element, rule, out errorMessage))
						{
							context.AddFailure(propertyName, errorMessage);
							return false;
						}
					}
					return true;
				});
		}

		private bool ValidateProperty(JsonElement jsonElement, ValidationRule rule, out string errorMessage)
		{
			if (!string.IsNullOrEmpty(rule.ParentPropertyName) && jsonElement.ValueKind == JsonValueKind.Array)
			{
				foreach (JsonElement itemElement in jsonElement.EnumerateArray())
				{
					if (!ValidateProperty(itemElement, rule, out errorMessage))
					{
						return false;
					}
				}
			}
			else if (!string.IsNullOrEmpty(rule.ParentPropertyName) && jsonElement.ValueKind == JsonValueKind.Object)
			{
				jsonElement.TryFindValueByCaseIgnoredKey(rule.PropertyName, out JsonElement element);

				if (!ApplyValidationRule(element, rule, out errorMessage))
				{
					return false;
				}
			}
			else
			{
				if (!ApplyValidationRule(jsonElement, rule, out errorMessage))
				{
					return false;
				}
			}

			errorMessage = default;
			return true;
		}

		private bool ApplyValidationRule(JsonElement element, ValidationRule rule, out string errorMessage)
		{
			if (rule.RuleType == ValidationRuleType.Required || element.ValueKind == JsonValueKind.Null || element.ValueKind == JsonValueKind.Undefined)
			{
				if ((element.ValueKind == JsonValueKind.Null || element.ValueKind == JsonValueKind.Undefined))
				{
					errorMessage = $"{rule.PropertyName} is required.";
					return false;
				}
			}
			else if (rule.RuleType == ValidationRuleType.MinCount)
			{
				if (element.ValueKind != JsonValueKind.Array || element.EnumerateArray().Count() < int.Parse(rule.RuleValue))
				{
					errorMessage = $"{rule.PropertyName} must have at least {rule.RuleValue} items.";
					return false;
				}
			}
			else if (rule.RuleType == ValidationRuleType.MinValue)
			{
				if (element.ValueKind != JsonValueKind.Number || element.GetInt32() < int.Parse(rule.RuleValue))
				{
					errorMessage = $"{rule.PropertyName} must be at least {rule.RuleValue}.";
					return false;
				}
			}

			errorMessage = default;
			return true;
		}
	}
}
