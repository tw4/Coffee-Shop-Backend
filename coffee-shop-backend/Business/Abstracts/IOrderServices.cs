using coffee_shop_backend.Dto.Order;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Abstracts;

public interface IOrderServices: ICrudServices<AddOrderRequest, UpdateOrderStatusRequest>
{
    public IActionResult GetOrdersByUserId(string token);
    public IActionResult Update(UpdateOrderStatusRequest request,long orderId ,string token);
}