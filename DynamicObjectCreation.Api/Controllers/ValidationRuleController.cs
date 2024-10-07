using DynamicObjectCreation.Application.Methods.Commands.CreateValidationRule;
using DynamicObjectCreation.Application.Methods.Commands.UpdateValidationRule;
using Microsoft.AspNetCore.Mvc;

namespace DynamicObjectCreation.Api.Controllers
{
	public class ValidationRuleController : BaseController
	{
		[HttpPost]
		public async Task<IActionResult> CreateValidationRule([FromBody] CreateValidationRuleCommand request, CancellationToken cancellationToken)
		{
			return Ok(await Sender.Send(request, cancellationToken));
		}

		[HttpPost]
		public async Task<IActionResult> UpdateValidationRule([FromBody] UpdateValidationRuleCommand request, CancellationToken cancellationToken)
		{
			return Ok(await Sender.Send(request, cancellationToken));
		}
	}
}
