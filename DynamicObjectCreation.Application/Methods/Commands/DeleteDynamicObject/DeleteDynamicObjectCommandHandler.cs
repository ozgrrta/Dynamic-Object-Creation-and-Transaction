using DynamicObjectCreation.Application.Common.Dtos;
using DynamicObjectCreation.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DynamicObjectCreation.Application.Methods.Commands.DeleteDynamicObject
{
	public class DeleteDynamicObjectCommandHandler : IRequestHandler<DeleteDynamicObjectCommand, DefaultResponse>
	{
		private readonly IAppDbContext _context;

		public DeleteDynamicObjectCommandHandler(IAppDbContext context)
		{
			_context = context;
		}

		public async Task<DefaultResponse> Handle(DeleteDynamicObjectCommand request, CancellationToken cancellationToken)
		{
			return await DeleteDynamicObject(request, cancellationToken).ConfigureAwait(false);
		}

		private async Task<DefaultResponse> DeleteDynamicObject(DeleteDynamicObjectCommand request, CancellationToken cancellationToken)
		{
			var dynamicObject = await _context.DynamicObjects.FirstOrDefaultAsync(o => o.Id == request.DynamicObjectId, cancellationToken);

			if (dynamicObject == null)
			{
				return DefaultResponse.Failure("The dynamic object with the id provided could not be found!", HttpStatusCode.NotFound);
			}

			_context.DynamicObjects.Remove(dynamicObject);
			await _context.SaveChangesAsync(cancellationToken);

			return DefaultResponse.Successful("The dynamic object with the id provided was successfully deleted", HttpStatusCode.OK);
		}
	}
}
