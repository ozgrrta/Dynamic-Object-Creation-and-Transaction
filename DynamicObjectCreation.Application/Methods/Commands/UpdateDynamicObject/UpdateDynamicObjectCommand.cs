using DynamicObjectCreation.Application.Common.Dtos;
using MediatR;
using System.Text.Json;

namespace DynamicObjectCreation.Application.Methods.Commands.UpdateDynamicObject
{
	public class UpdateDynamicObjectCommand : IRequest<DefaultResponse>
	{
		public Guid DynamicObjectId { get; set; }
		public string ObjectType { get; set; }
		public JsonDocument Data { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
	}
}
