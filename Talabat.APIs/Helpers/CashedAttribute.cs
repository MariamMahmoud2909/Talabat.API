using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Helpers
{
	public class CashedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _timeToLiveInSeconds;

		public CashedAttribute(int timeToLiveInSeconds)
		{
			_timeToLiveInSeconds = timeToLiveInSeconds;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var responsCasheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCashService>();

			var casheKey = GenerateCasheKeyFromRequest(context.HttpContext.Request);

			var response = await responsCasheService.GetCashedResponseAsync(casheKey);

			if (!string.IsNullOrEmpty(response))
			{
				var result = new ContentResult()
				{
					Content = response,
					ContentType = "application/json",
					StatusCode = 200
				};

				context.Result = result;
				return;
			}

			var excutedActionContext = await next.Invoke(); // excutes the next action filter or the action itself

			if (excutedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
			{
				await responsCasheService.CasheResponseAsync(casheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
			}
		}

		private string GenerateCasheKeyFromRequest(HttpRequest request)
		{
			var keyBuilder = new StringBuilder();

			keyBuilder.Append(request.Path);

			foreach (var (key , value) in request.Query.OrderBy(x => x.Key))
			{
				keyBuilder.Append($"|{key}-{value}");
			}

			return keyBuilder.ToString();
		}
	}
}
