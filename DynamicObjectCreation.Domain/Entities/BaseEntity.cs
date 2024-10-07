using System.ComponentModel.DataAnnotations;

namespace DynamicObjectCreation.Domain.Entities
{
	public class BaseEntity
	{
		[Key]
		public Guid Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? LastUpdatedAt { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }

	}
}
