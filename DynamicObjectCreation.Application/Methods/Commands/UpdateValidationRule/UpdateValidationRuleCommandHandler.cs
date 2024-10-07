using DynamicObjectCreation.Application.Common.Dtos;
using DynamicObjectCreation.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DynamicObjectCreation.Application.Methods.Commands.UpdateValidationRule
{
	public class UpdateValidationRuleCommandHandler : IRequestHandler<UpdateValidationRuleCommand, DefaultResponse>
	{
		private readonly IAppDbContext _context;
		private readonly ICacheService _cacheService;

		public UpdateValidationRuleCommandHandler(IAppDbContext context, ICacheService cacheService)
		{
			_context = context;
			_cacheService = cacheService;
		}

		public async Task<DefaultResponse> Handle(UpdateValidationRuleCommand request, CancellationToken cancellationToken)
		{
			return await UpdateValidationRule(request, cancellationToken).ConfigureAwait(false);
		}

		private async Task<DefaultResponse> UpdateValidationRule(UpdateValidationRuleCommand request, CancellationToken cancellationToken)
		{
			var validationRule = await _context.ValidationRules.FirstOrDefaultAsync(o => o.Id == request.ValidationRuleId, cancellationToken);

			if (validationRule is null)
			{
				return DefaultResponse.Failure("The validation rule with the id provided could not be found!", HttpStatusCode.NotFound);
			}

			var operationDate = DateTime.UtcNow;

			validationRule.RuleType = request.RuleType;
			validationRule.RuleValue = request.RuleValue;
			validationRule.ChildObjectType = request.ParentPropertyName;
			validationRule.ChildObjectType = request.ChildObjectType;
			validationRule.IsActive = request.IsActive;
			validationRule.IsDeleted = request.IsDeleted;
			validationRule.LastUpdatedAt = operationDate;

			_context.ValidationRules.Update(validationRule);
			await _context.SaveChangesAsync(cancellationToken);

			return DefaultResponse.Successful();
		}
	}
}
