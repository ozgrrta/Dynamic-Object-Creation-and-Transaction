using DynamicObjectCreation.Application.Methods.Commands.CreateDynamicObject;
using DynamicObjectCreation.Application.Methods.Commands.CreateValidationRule;
using DynamicObjectCreation.Application.Methods.Commands.UpdateDynamicObject;
using DynamicObjectCreation.Application.Methods.Commands.UpdateValidationRule;
using DynamicObjectCreation.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Text.Json;
using ValidationException = FluentValidation.ValidationException;

namespace DynamicObjectCreation.Application.Common.Behaviors
{
	public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly IEnumerable<IValidator<DynamicObject>> _dynamicObjectValidators;
		private readonly IEnumerable<IValidator<ValidationRule>> _validationRuleValidators;
		public ValidationBehavior(IEnumerable<IValidator<DynamicObject>> dynamicObjectValidators, IEnumerable<IValidator<ValidationRule>> validationRuleValidators)
		{
			_dynamicObjectValidators = dynamicObjectValidators;
			_validationRuleValidators = validationRuleValidators;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			ValidationResult[] validationResults = default;
			if (request is CreateDynamicObjectCommand || request is UpdateDynamicObjectCommand)
			{
				var dynamicObject = JsonSerializer.Deserialize<DynamicObject>(JsonSerializer.Serialize(request));
				var context = new ValidationContext<DynamicObject>(dynamicObject);
				validationResults = await Task.WhenAll(_dynamicObjectValidators.Select(v => v.ValidateAsync(context, cancellationToken)));
			}
			else if (request is CreateValidationRuleCommand || request is UpdateValidationRuleCommand)
			{
				var validationRule = JsonSerializer.Deserialize<ValidationRule>(JsonSerializer.Serialize(request));
				var context = new ValidationContext<ValidationRule>(validationRule);
				validationResults = await Task.WhenAll(_validationRuleValidators.Select(v => v.ValidateAsync(context, cancellationToken)));
			}

			var failures = validationResults?.SelectMany(r => r.Errors).Where(f => f != null).ToList();

			if (failures?.Count > 0)
			{
				throw new ValidationException(failures);
			}

			return await next();
		}
	}
}
