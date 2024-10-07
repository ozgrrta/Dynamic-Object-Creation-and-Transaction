using DynamicObjectCreation.Application.Common.Dtos;
using DynamicObjectCreation.Domain.Entities;
using DynamicObjectCreation.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace DynamicObjectCreation.Application.Methods.Commands.CreateDynamicObject
{
	public class CreateDynamicObjectCommandHandler : IRequestHandler<CreateDynamicObjectCommand, DefaultResponse>
	{
		private readonly IAppDbContext _context;

		public CreateDynamicObjectCommandHandler(IAppDbContext context)
		{
			_context = context;
		}

		public async Task<DefaultResponse> Handle(CreateDynamicObjectCommand request, CancellationToken cancellationToken)
		{
			return await CreateDynamicObject(request, cancellationToken).ConfigureAwait(false);
		}

		private async Task<DefaultResponse> CreateDynamicObject(CreateDynamicObjectCommand request, CancellationToken cancellationToken)
		{
			var isObjectExist = await _context.DynamicObjects.AnyAsync(o => JsonDocument.Equals(o.Data, request.Data), cancellationToken);

			if (isObjectExist)
			{
				return DefaultResponse.Failure("The dynamic object with the data provided already exists!", HttpStatusCode.BadRequest);
			}

			var operationDate = DateTime.UtcNow;

			var dynamicObject = new DynamicObject
			{
				Id = Guid.NewGuid(),
				ObjectType = request.ObjectType,
				Data = request.Data,
				CreatedAt = operationDate,
				IsActive = true,
				IsDeleted = false,
			};

			await _context.DynamicObjects.AddAsync(dynamicObject, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return DefaultResponse.Successful();
		}
	}
}
