using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Dto.Order;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController:ControllerBase
{
    private readonly IOrderServices _orderServices;

    public OrderController(IOrderServices orderServices)
    {
        _orderServices = orderServices;
    }

    [HttpPost]
    public IActionResult AddOrder([FromBody] AddOrderRequest request, [FromHeader] string token)
    {
        return _orderServices.Add(request, token);
    }

    [HttpGet]
    public IActionResult GetOrdersByUserId([FromHeader] string token)
    {
        return _orderServices.GetOrdersByUserId(token);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request, [FromRoute] long id, [FromHeader] string token)
    {
        return _orderServices.Update(request, id, token);
    }

}