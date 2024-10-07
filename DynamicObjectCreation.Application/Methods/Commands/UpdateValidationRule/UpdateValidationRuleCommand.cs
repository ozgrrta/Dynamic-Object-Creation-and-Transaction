using DynamicObjectCreation.Application.Common.Dtos;
using DynamicObjectCreation.Domain.Enums;
using MediatR;

namespace DynamicObjectCreation.Application.Methods.Commands.UpdateValidationRule
{
	public class UpdateValidationRuleCommand : IRequest<DefaultResponse>
	{
		public Guid ValidationRuleId { get; set; }
		public string PropertyName { get; set; }
		public ValidationRuleType RuleType { get; set; }
		public string RuleValue { get; set; }
		public string? ParentPropertyName { get; set; }
		public string? ChildObjectType { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
	}
}
