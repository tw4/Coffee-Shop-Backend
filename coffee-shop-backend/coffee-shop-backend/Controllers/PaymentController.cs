using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Dto.Paymant;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace coffee_shop_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController: ControllerBase
{
    private readonly IPaymentServices _paymentServices;
    private readonly IConfiguration _configuration;
    public PaymentController(IPaymentServices paymentServices, IConfiguration configuration)
    {
        _paymentServices = paymentServices;
        _configuration = configuration;
    }

    [HttpPost( "create")]
    public IActionResult Create([FromBody] CreatePaymenRequest request, [FromHeader] string token)
    {
        Session session = _paymentServices.Create(request,token);
        if (session == null)
        {
            return BadRequest();
        }
        Response.Headers.Add("Location", session.Url);
        return new OkObjectResult(new { Location = session.Url});
    }

    [HttpPost]
    public async Task<IActionResult> WebHook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            // Retrieve the event by verifying the signature using the raw body and secret if webhook signing is configured.
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _configuration["EndPointScopes"]);

            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                // Get the payment intent
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                return _paymentServices.PaymentIntentSucceeded(paymentIntent);

            }
            else if (stripeEvent.Type == Events.PaymentMethodAttached)
            {
                var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                Console.WriteLine("PaymentMethod was attached to a Customer!");
            }
            else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                Console.WriteLine("PaymentIntent failed!");

            }
            // ... handle other event types
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
    }

}