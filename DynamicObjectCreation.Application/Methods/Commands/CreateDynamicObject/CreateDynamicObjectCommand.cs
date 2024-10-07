using DynamicObjectCreation.Application.Common.Dtos;
using MediatR;
using System.Text.Json;

namespace DynamicObjectCreation.Application.Methods.Commands.CreateDynamicObject
{
	public class CreateDynamicObjectCommand : IRequest<DefaultResponse>
	{
		public string ObjectType { get; set; }
		public JsonDocument Data { get; set; }
	}
}
