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
    private readonly Logger<OrderManager> _logger;

    public OrderManager(CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices, Logger<OrderManager> logger)
    {
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
        _logger = logger;
    }

    public IActionResult AddOrder(AddOrderRequest request, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation("Token is invalid add order");
            return new UnauthorizedResult();
        }

        long userId = _jwtServices.GetUserIdFromToken(token);

        Stock? stock = _coffeeShopDbContex.Stocks.FirstOrDefault(s => s.ProductId == request.ProductId);

        if (stock.Amount <= 0)
        {
            _logger.LogInformation("Stock is empty add order");
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
            _logger.LogInformation("Order added successfully");
            return new OkObjectResult(new { message = "Order added successfully", success = true });
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while adding order",e);
            return new BadRequestObjectResult(new {message = e.Message, success = false});
        }
    }

    public IActionResult GetOrdersByUserId(string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation("Token is invalid get orders by user id");
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

        _logger.LogInformation("Orders fetched successfully");
        return new OkObjectResult(new {message = "Orders fetched successfully", success = true, data = ordersWithUserAndProduct});
    }

    public IActionResult UpdateOrderStatus(UpdateOrderStatusRequest request, long orderId, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation("Token is invalid update order status");
            return new UnauthorizedResult();
        }

        Order? order = _coffeeShopDbContex.Orders.Find(orderId);

        if (order  == null)
        {
            _logger.LogInformation("Order not found update order status");
            return new NotFoundResult();
        }

        long userId = _jwtServices.GetUserIdFromToken(token);
        User? user = _coffeeShopDbContex.Users.Find(userId);

        if (user == null)
        {
            _logger.LogInformation("User not found update order status");
            return new NotFoundResult();
        }

        if (user.Role != EnumRole.ADMIN)
        {
            _logger.LogWarning("User is not admin update order status");
            return new UnauthorizedResult();
        }

        order.Status = request.Status;
        _coffeeShopDbContex.Update(order);

        try
        {
            _coffeeShopDbContex.SaveChanges();
            _logger.LogInformation("Order status updated successfully");
            return new OkObjectResult(new {message = "Order status updated successfully", success = true});
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while updating order status",e);
            return new BadRequestObjectResult(new {message = e.Message, success = false});
        }
    }
}