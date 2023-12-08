using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Dto.Order;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coffee_shop_backend.Business.Concreates;

public class OrderManager: IOrderServices
{
    private readonly CoffeeShopDbContex _coffeeShopDbContex;
    private readonly IJwtServices _jwtServices;

    public OrderManager(CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices)
    {
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
    }

    public IActionResult AddOrder(AddOrderRequest request, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        long userId = _jwtServices.GetUserIdFromToken(token);

        Stock? stock = _coffeeShopDbContex.Stocks.FirstOrDefault(s => s.ProductId == request.ProductId);

        if (stock.Amount <= 0)
        {
            return new BadRequestObjectResult(new {message = "Stock is empty", success = false});
        }

        Order order = new Order()
        {
            ProductId = request.ProductId,
            UserId = userId,
            Status = EnumOrderStatus.Waiting,
            PaymentDate = DateTime.Now,
            Address = request.Address,
            Email = request.Email,
            FullName = request.FullName,
        };

        stock.Amount -= 1;

        _coffeeShopDbContex.Orders.Add(order);
        _coffeeShopDbContex.Stocks.Update(stock);

        try
        {
            _coffeeShopDbContex.SaveChanges();
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

        var ordersWithUserAndProduct = _coffeeShopDbContex.Orders
            .Include(o => o.User)
            .Include(o => o.Product)
            .Where(o => o.UserId == id).Select(o => new
            {
                o.Id,
                o.PaymentDate,
                o.Address,
                o.Email,
                o.FullName,
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

        Order? order = _coffeeShopDbContex.Orders.Find(orderId);

        if (order  == null)
        {
            return new NotFoundResult();
        }

        long userId = _jwtServices.GetUserIdFromToken(token);
        User? user = _coffeeShopDbContex.Users.Find(userId);

        if (user == null)
        {
            return new NotFoundResult();
        }

        if (user.Role != EnumRole.ADMIN)
        {
            return new UnauthorizedResult();
        }

        order.Status = request.Status;
        _coffeeShopDbContex.Update(order);

        try
        {
            _coffeeShopDbContex.SaveChanges();
            return new OkObjectResult(new {message = "Order status updated successfully", success = true});
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(new {message = e.Message, success = false});
        }
    }
}