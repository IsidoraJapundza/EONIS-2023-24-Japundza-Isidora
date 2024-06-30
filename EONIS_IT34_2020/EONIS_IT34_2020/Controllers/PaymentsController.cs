using EONIS_IT34_2020.Models.Stripe;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace EONIS_IT34_2020.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly StripeSettings _stripeSettings;

        public PaymentsController(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest paymentRequest)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = paymentRequest.Price,
                Description = paymentRequest.OrderId,
                PaymentMethod = paymentRequest.PaymentMethodId,
                Currency = "usd",
                ConfirmationMethod = "manual",
                Confirm = true,
                ReturnUrl = "http://192.168.100.10:3000/"
            };

            var service = new PaymentIntentService();
            try
            {
                var paymentIntent = await service.CreateAsync(options);

                if (paymentIntent.Status == "requires_action" || paymentIntent.Status == "requires_source_action")
                {
                    return Ok(new { requiresAction = true, clientSecret = paymentIntent.ClientSecret });
                }

                return Ok(new { success = true });
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
