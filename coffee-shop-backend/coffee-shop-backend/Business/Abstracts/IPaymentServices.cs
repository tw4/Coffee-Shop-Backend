using coffee_shop_backend.Dto.Paymant;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace coffee_shop_backend.Business.Abstracts;

public interface IPaymentServices
{
   public Session Create(CreatePaymenRequest request,string token);
   public IActionResult PaymentIntentSucceeded(PaymentIntent paymentIntent);
}