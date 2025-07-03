using Microsoft.AspNetCore.Mvc;
using Practice_BoilerPlate.Stripe;
using System.Threading.Tasks;


namespace Practice_BoilerPlate.Controllers
{
    [Route("api/stripe")]
    [ApiController]
    public class StripeController:Practice_BoilerPlateControllerBase
    {
        private readonly StripeAppService _stripeAppService;

        public StripeController(StripeAppService stripeAppService)
        {
            _stripeAppService = stripeAppService;
        }

        [HttpPost("create-session")]
        public async Task<IActionResult> CreateSession([FromBody] StripeSessionRequest input)
        {
            var url = await _stripeAppService.CreateCheckoutSessionAsync(input.PriceId);
            return Content(url, "text/plain");  // Sends raw string, not JSON
                                                // now valid, returns plain text to frontend
        }
        
    }
}
