using AutoMapper;
using DynamicObjectCreation.Application.Common.Mapper;
using DynamicObjectCreation.Domain.Entities;
using System.Text.Json;

namespace DynamicObjectCreation.Application.Methods.Queries.GetDynamicObjects
{
	public class GetDynamicObjectsQueryDto : IMapFrom<DynamicObject>
	{
		public string ObjectType { get; set; }
		public JsonDocument Data { get; set; }
		public void Mapping(Profile profile)
		{
			profile.CreateMap<DynamicObject, GetDynamicObjectsQueryDto>();
		}
	}
}
