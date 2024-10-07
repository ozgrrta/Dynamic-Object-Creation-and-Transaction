using System.Text.Json;

namespace DynamicObjectCreation.Domain.Entities
{
	public class DynamicObject : BaseEntity
	{
		public string ObjectType { get; set; }
		public JsonDocument Data { get; set; }
	}
}
