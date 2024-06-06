using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
	[Route("[controller]/{code}")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorsController : ControllerBase
	{
		[HttpGet]
		public ActionResult Error(int code)
		{
			if (code == 401 || code == 404)
				return NotFound(new ApiResponse(code));

			return StatusCode(code);
		}
	}
}
