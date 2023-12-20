using coffee_shop_backend.Dto.Order;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Abstracts;

public interface IOrderServices
{
    public IActionResult AddOrder(AddOrderRequest request, string token);
    public IActionResult GetOrdersByUserId(string token);
    public IActionResult UpdateOrderStatus(UpdateOrderStatusRequest request,long orderId ,string token);
}