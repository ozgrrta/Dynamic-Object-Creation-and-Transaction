using DynamicObjectCreation.Application.Common.Dtos;
using DynamicObjectCreation.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DynamicObjectCreation.Application.Methods.Commands.UpdateDynamicObject
{
	public class UpdateDynamicObjectCommandHandler : IRequestHandler<UpdateDynamicObjectCommand, DefaultResponse>
	{
		private readonly IAppDbContext _context;

		public UpdateDynamicObjectCommandHandler(IAppDbContext context)
		{
			_context = context;
		}

		public async Task<DefaultResponse> Handle(UpdateDynamicObjectCommand request, CancellationToken cancellationToken)
		{
			return await UpdateDynamicObject(request, cancellationToken).ConfigureAwait(false);
		}

		private async Task<DefaultResponse> UpdateDynamicObject(UpdateDynamicObjectCommand request, CancellationToken cancellationToken)
		{
			var dynamicObject = await _context.DynamicObjects.FirstOrDefaultAsync(o => o.Id == request.DynamicObjectId, cancellationToken);

			if (dynamicObject is null)
			{
				return DefaultResponse.Failure("The dynamic object with the id provided could not be found!", HttpStatusCode.NotFound);
			}

			if (object.Equals(dynamicObject.Data, request.Data) &&
				string.Equals(dynamicObject.ObjectType, request.ObjectType, StringComparison.OrdinalIgnoreCase))
			{
				return DefaultResponse.Failure("The dynamic object with the data provided already exists", HttpStatusCode.BadRequest);
			}

			var operationDate = DateTime.UtcNow;

			dynamicObject.ObjectType = request.ObjectType;
			dynamicObject.Data = request.Data;
			dynamicObject.CreatedAt = operationDate;
			dynamicObject.IsActive = request.IsActive;
			dynamicObject.IsDeleted = request.IsDeleted;

			_context.DynamicObjects.Update(dynamicObject);
			await _context.SaveChangesAsync(cancellationToken);

			return DefaultResponse.Successful();
		}
	}
}
