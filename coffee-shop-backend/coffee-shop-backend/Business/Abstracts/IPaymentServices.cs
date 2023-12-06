using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Abstracts;

public interface IPaymantServices
{
   public IActionResult ProcessPayment(string token);
}