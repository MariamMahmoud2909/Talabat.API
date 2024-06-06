using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
	public class PaymentsController : BaseApiController
	{
		private readonly IPaymentService _paymentService;
		private readonly ILogger<PaymentsController> _logger;
		private const string endpointSecret = "whsec_e024ecb3cd9c99d99e82be07f135f9f38824610d9626e8f8791fab61ecd136a8";

		
		public PaymentsController(
			IPaymentService paymentService,
			ILogger<PaymentsController> logger)
		{
			_paymentService = paymentService;
			_logger = logger;
		}

		[Authorize]
		[ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[HttpPost("{basketId}")]
		public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

			if (basketId is null) return BadRequest(new ApiResponse(400, "An Error with your Basket"));

			return Ok(basket);
		}

		[HttpPost("webhook")]
		public async Task<IActionResult> WebHook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			var stripeEvent = EventUtility.ConstructEvent(json,
				Request.Headers["Stripe-Signature"], endpointSecret);

			Order? order;

			var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

			switch (stripeEvent.Type)
			{
				case Events.PaymentIntentSucceeded:
					order = await _paymentService.UpdateOrderStatus(paymentIntent.Id , true);
					_logger.LogInformation($"Order is succeeded {order?.PaymentIntentId}");
					break;
				case Events.PaymentIntentPaymentFailed:
					order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
					_logger.LogInformation($"Order is failed {order?.PaymentIntentId}");
					break;
			}

			return Ok();
		}
	}
}
