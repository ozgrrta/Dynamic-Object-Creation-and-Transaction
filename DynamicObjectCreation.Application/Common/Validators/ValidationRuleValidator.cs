using DynamicObjectCreation.Domain.Entities;
using DynamicObjectCreation.Domain.Enums;
using FluentValidation;

namespace DynamicObjectCreation.Application.Common.Validators
{
	public class ValidationRuleValidator : AbstractValidator<ValidationRule>
	{
		public ValidationRuleValidator()
		{
			RuleFor(v => v.ObjectType)
				.NotEmpty().WithMessage("ObjectType is required.")
				.MaximumLength(50).WithMessage("ObjectType can be at most 50 characters long.");

			RuleFor(v => v.PropertyName)
			.NotEmpty().WithMessage("PropertyName is required.")
			.MaximumLength(50).WithMessage("PropertyName can be at most 50 characters long.");

			RuleFor(v => v.ParentPropertyName)
				.MaximumLength(50).When(v => !string.IsNullOrEmpty(v.ParentPropertyName))
				.WithMessage("ParentPropertyName can be at most 50 characters long.");

			RuleFor(v => v)
				.Must((rule, obj, context) =>
				{
					if (!BeValidRuleValue(rule, out string errorMessage))
					{
						context.AddFailure(errorMessage);
						return false;
					};
					return true;
				});
		}

		private bool BeValidRuleValue(ValidationRule rule, out string errorMessage)
		{
			errorMessage = rule.RuleType switch
			{
				ValidationRuleType.Required => !bool.TryParse(rule.RuleValue, out _) ? $"RuleValue must be a boolean value as string ('true'/'false') for '{ValidationRuleType.Required}' type validation rule." : string.Empty,
				ValidationRuleType.MinCount => !int.TryParse(rule.RuleValue, out _) ? $"RuleValue must be an integer value as string for '{ValidationRuleType.MinCount}' type validation rule." : string.Empty,
				ValidationRuleType.MinValue => !int.TryParse(rule.RuleValue, out _) ? $"RuleValue must be an integer value as string for '{ValidationRuleType.MinValue}' type validation rule." : string.Empty,
				_ => "Invalid RuleType specified."
			};

			return string.IsNullOrEmpty(errorMessage);
		}
	}
}
