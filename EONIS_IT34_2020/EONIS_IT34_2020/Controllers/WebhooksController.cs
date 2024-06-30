using EONIS_IT34_2020.Data.PorudzbinaRepository;
using EONIS_IT34_2020.Models.Entities;
using EONIS_IT34_2020.Models.Stripe;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace EONIS_IT34_2020.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        private readonly StripeSettings _stripeSettings;
        private readonly IPorudzbinaRepository porudzbinaRepository;

        public WebhooksController(IOptions<StripeSettings> stripeSettings, IPorudzbinaRepository porudzbinaRepository)
        {
            _stripeSettings = stripeSettings.Value;
            this.porudzbinaRepository = porudzbinaRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Stripe()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _stripeSettings.WebhookSecret
                );

                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Handle successful payment intent
                    System.Console.WriteLine($"PaymentIntent was successful: {paymentIntent.Id}");

                    var orderId = paymentIntent.Description;
                    Guid.TryParse(orderId, out Guid guidOrderId);
                    if(guidOrderId != Guid.Empty )
                    {
                        Porudzbina porudzbina = porudzbinaRepository.GetExactPorudzbina(guidOrderId);
                        porudzbina.StatusPorudzbine = "Završena";
                        porudzbina.PotvrdaPlacanja = "Placeno";
                        porudzbinaRepository.UpdatePorudzbina(porudzbina);
                    }
                }
                else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Handle failed payment intent
                    System.Console.WriteLine($"PaymentIntent failed: {paymentIntent.Id}");

                    var orderId = paymentIntent.Description;
                    Guid.TryParse(orderId, out Guid guidOrderId);
                    if (guidOrderId != Guid.Empty)
                    {
                        Porudzbina porudzbina = porudzbinaRepository.GetExactPorudzbina(guidOrderId);
                        porudzbina.StatusPorudzbine = "Otkazana";
                        porudzbinaRepository.UpdatePorudzbina(porudzbina);
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
