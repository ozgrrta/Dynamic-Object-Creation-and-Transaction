using DynamicObjectCreation.Application.Common.Dtos;
using MediatR;

namespace DynamicObjectCreation.Application.Methods.Commands.DeleteDynamicObject
{
	public class DeleteDynamicObjectCommand : IRequest<DefaultResponse>
	{
		public Guid DynamicObjectId { get; set; }
	}
}
