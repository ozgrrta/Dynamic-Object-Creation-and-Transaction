using DynamicObjectCreation.Application.Methods.Commands.CreateDynamicObject;
using DynamicObjectCreation.Application.Methods.Commands.DeleteDynamicObject;
using DynamicObjectCreation.Application.Methods.Commands.UpdateDynamicObject;
using DynamicObjectCreation.Application.Methods.Queries.GetDynamicObjects;
using Microsoft.AspNetCore.Mvc;

namespace DynamicObjectCreation.Api.Controllers
{
	public class DynamicObjectController : BaseController
	{
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] GetDynamicObjectsQuery query, CancellationToken cancellationToken)
		{
			return Ok(await Sender.Send(query, cancellationToken));
		}

		[HttpPost]
		public async Task<IActionResult> CreateDynamicObject([FromBody] CreateDynamicObjectCommand request, CancellationToken cancellationToken)
		{
			return Ok(await Sender.Send(request, cancellationToken));
		}

		[HttpPut]
		public async Task<IActionResult> UpdateDynamicObject([FromBody] UpdateDynamicObjectCommand request, CancellationToken cancellationToken)
		{
			return Ok(await Sender.Send(request, cancellationToken));
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteDynamicObject([FromBody] DeleteDynamicObjectCommand request, CancellationToken cancellationToken)
		{
			return Ok(await Sender.Send(request, cancellationToken));
		}
	}
}
