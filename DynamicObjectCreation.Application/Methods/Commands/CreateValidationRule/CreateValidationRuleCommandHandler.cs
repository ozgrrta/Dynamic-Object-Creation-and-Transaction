using DynamicObjectCreation.Application.Common.Dtos;
using DynamicObjectCreation.Domain.Entities;
using DynamicObjectCreation.Domain.Enums;
using DynamicObjectCreation.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DynamicObjectCreation.Application.Methods.Commands.CreateValidationRule
{
	public class CreateValidationRuleCommandHandler : IRequestHandler<CreateValidationRuleCommand, DefaultResponse>
	{
		private readonly IAppDbContext _context;
		private readonly ICacheService _cacheService;

		public CreateValidationRuleCommandHandler(IAppDbContext context, ICacheService cacheService)
		{
			_context = context;
			_cacheService = cacheService;
		}

		public async Task<DefaultResponse> Handle(CreateValidationRuleCommand request, CancellationToken cancellationToken)
		{
			return await CreateValidationRule(request, cancellationToken).ConfigureAwait(false);
		}

		private async Task<DefaultResponse> CreateValidationRule(CreateValidationRuleCommand request, CancellationToken cancellationToken)
		{
			var validationRule = await _context.ValidationRules
				.FirstOrDefaultAsync(v => v.RuleType == request.RuleType &&
					string.Equals(v.ObjectType, request.ObjectType, StringComparison.OrdinalIgnoreCase) &&
					string.Equals(v.PropertyName, request.PropertyName, StringComparison.OrdinalIgnoreCase) &&
					string.Equals(v.ParentPropertyName, request.ParentPropertyName, StringComparison.OrdinalIgnoreCase) &&
					string.Equals(v.ChildObjectType, request.ChildObjectType, StringComparison.OrdinalIgnoreCase));

			if (validationRule is not null)
			{
				return DefaultResponse.Failure($"The validation rule for the object type provided ({request.ObjectType}) is already available on the database with the following id: {validationRule.Id}", HttpStatusCode.BadRequest);
			}

			var isParentOrChildExist = !string.IsNullOrEmpty(request.ParentPropertyName) || !string.IsNullOrEmpty(request.ChildObjectType);

			DateTime operationDate = DateTime.UtcNow;

			validationRule = new ValidationRule()
			{
				Id = Guid.NewGuid(),
				ObjectType = request.ObjectType,
				PropertyName = request.PropertyName,
				RuleType = isParentOrChildExist ? ValidationRuleType.Required : request.RuleType,
				RuleValue = isParentOrChildExist ? "true" : request.RuleValue,
				ParentPropertyName = request.ParentPropertyName,
				ChildObjectType = request.ChildObjectType,
				CreatedAt = operationDate,
				IsActive = true,
				IsDeleted = false,
			};

			await _context.ValidationRules.AddAsync(validationRule, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			await _cacheService.LoadValidationRulesToCacheAsync(cancellationToken);

			return DefaultResponse.Successful($"The validation rule with the id ({validationRule.Id}) is successfully created");
		}
	}
}
