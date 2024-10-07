using DynamicObjectCreation.Domain.Enums;

namespace DynamicObjectCreation.Domain.Entities
{
	public class ValidationRule : BaseEntity
	{
		public string ObjectType { get; set; }
		public string PropertyName { get; set; }
		public ValidationRuleType RuleType { get; set; }
		public string RuleValue { get; set; }
		public string? ParentPropertyName { get; set; }
		public string? ChildObjectType { get; set; }
	}
}
