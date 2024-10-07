using AutoMapper;
using AutoMapper.QueryableExtensions;
using DynamicObjectCreation.Application.Common.Dtos;
using DynamicObjectCreation.Infrastructure.Interfaces;
using DynamicObjectCreation.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace DynamicObjectCreation.Application.Methods.Queries.GetDynamicObjects
{
	public class GetDynamicObjectsQueryHandler : IRequestHandler<GetDynamicObjectsQuery, DefaultResponse<GetDynamicObjectsQueryVm>>
	{
		private readonly IAppDbContext _context;
		private readonly IMapper _mapper;
		private dynamic value;

		public GetDynamicObjectsQueryHandler(IAppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<DefaultResponse<GetDynamicObjectsQueryVm>> Handle(GetDynamicObjectsQuery request, CancellationToken cancellationToken)
		{
			return await GetDynamicObjects(request, cancellationToken).ConfigureAwait(false);
		}

		private async Task<DefaultResponse<GetDynamicObjectsQueryVm>> GetDynamicObjects(GetDynamicObjectsQuery request, CancellationToken cancellationToken)
		{
			JsonDocument? document = default;

			if (!string.IsNullOrEmpty(request.DataAsJson))
			{
				document = JsonDocument.Parse(request.DataAsJson);
			}

			var query = _context.DynamicObjects.AsQueryable().AsNoTracking();

			List<GetDynamicObjectsQueryDto>? dto;
			GetDynamicObjectsQueryVm vm = new();

			if (request.DynamicObjectId is not null)
			{
				dto = await query.Where(o => o.Id == request.DynamicObjectId)
					.ProjectTo<GetDynamicObjectsQueryDto>(_mapper.ConfigurationProvider)
					.ToListAsync(cancellationToken);

				if (!dto.Any())
				{
					return DefaultResponse<GetDynamicObjectsQueryVm>.Failure("The dynamic object with the id provided could not be found!", HttpStatusCode.NotFound);
				}
			}
			else
			{
				if (request.ObjectType != null)
				{
					query = query.Where(o => o.ObjectType == request.ObjectType);
				}

				if (request.IsActive != null)
				{
					query = query.Where(o => o.IsActive == request.IsActive);
				}

				if (request.IsDeleted != null)
				{
					query = query.Where(o => o.IsDeleted == request.IsDeleted);
				}

				if (document is not null)
				{
					JsonElement element = document.RootElement;

					foreach (var kvp in element.EnumerateObject())
					{
						string key = kvp.Name;
						var value = kvp.Value;

						bool isStringValue = value.ValueKind is JsonValueKind.String;
						query = query.Where(d => AppDbContext.JsonbDeepSearch(d.Data, key, value.ToString(), isStringValue));
					}
				}
			}

			dto = await query.ProjectTo<GetDynamicObjectsQueryDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

			vm.DynamicObjects = dto;
			vm.Count = dto.Count;

			return DefaultResponse<GetDynamicObjectsQueryVm>.Successful(vm, HttpStatusCode.OK);
		}
	}
}
