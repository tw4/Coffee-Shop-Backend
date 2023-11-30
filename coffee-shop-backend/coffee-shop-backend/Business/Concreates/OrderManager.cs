using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Dto.Order;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coffee_shop_backend.Business.Concreates;

public class OrderManager: IOrderServices
{
    private readonly CoffeeShopContex _coffeeShopContex;
    private readonly IJwtServices _jwtServices;

    public OrderManager(CoffeeShopContex coffeeShopContex, IJwtServices jwtServices)
    {
        _coffeeShopContex = coffeeShopContex;
        _jwtServices = jwtServices;
    }

    public IActionResult AddOrder(AddOrderRequest request, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        long userId = _jwtServices.GetUserIdFromToken(token);

        Order order = new Order()
        {
            ProductId = request.ProductId,
            UserId = userId,
            Status = EnumOrderStatus.Waiting,
        };

        _coffeeShopContex.Orders.Add(order);

        try
        {
            _coffeeShopContex.SaveChanges();
            return new OkObjectResult(new { message = "Order added successfully", success = true });
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(new {message = e.Message, success = false});
        }
    }

    public IActionResult GetOrdersByUserId(string token)
    {
if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        long id = _jwtServices.GetUserIdFromToken(token);

        var ordersWithUserAndProduct = _coffeeShopContex.Orders
            .Include(o => o.User)
            .Include(o => o.Product)
            .Where(o => o.UserId == id).Select(o => new
            {
                o.Id,
                o.Status,
                o.User,
                o.Product
            }).ToList();

        return new OkObjectResult(new {message = "Orders fetched successfully", success = true, data = ordersWithUserAndProduct});
    }

    public IActionResult UpdateOrderStatus(UpdateOrderStatusRequest request, long orderId, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        Order order = _coffeeShopContex.Orders.Find(orderId);
        order.Status = request.Status;
        _coffeeShopContex.Update(order);

        try
        {
            _coffeeShopContex.SaveChanges();
            return new OkObjectResult(new {message = "Order status updated successfully", success = true});
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(new {message = e.Message, success = false});
        }
    }
}