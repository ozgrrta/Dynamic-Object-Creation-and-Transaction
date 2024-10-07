using DynamicObjectCreation.Application.Common.Dtos;
using DynamicObjectCreation.Domain.Enums;
using MediatR;

namespace DynamicObjectCreation.Application.Methods.Commands.CreateValidationRule
{
	public class CreateValidationRuleCommand : IRequest<DefaultResponse>
	{
		public string ObjectType { get; set; }
		public string PropertyName { get; set; }
		public ValidationRuleType RuleType { get; set; }
		public string RuleValue { get; set; }
		public string? ParentPropertyName { get; set; }
		public string? ChildObjectType { get; set; }
	}
}
