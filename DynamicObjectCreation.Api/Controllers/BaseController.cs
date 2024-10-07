using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DynamicObjectCreation.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public abstract class BaseController : ControllerBase
	{
		private ISender _sender;

		protected ISender Sender => _sender ?? (_sender = HttpContext.RequestServices.GetService<ISender>());
	}
}
