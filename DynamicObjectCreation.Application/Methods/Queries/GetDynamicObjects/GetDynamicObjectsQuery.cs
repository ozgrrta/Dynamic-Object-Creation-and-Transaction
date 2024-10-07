using DynamicObjectCreation.Application.Common.Dtos;
using MediatR;

namespace DynamicObjectCreation.Application.Methods.Queries.GetDynamicObjects
{
	public class GetDynamicObjectsQuery : IRequest<DefaultResponse<GetDynamicObjectsQueryVm>>
	{
		public Guid? DynamicObjectId { get; set; }
		public string? ObjectType { get; set; }
		public string? DataAsJson { get; set; }
		public bool? IsActive { get; set; }
		public bool? IsDeleted { get; set; }
	}
}
