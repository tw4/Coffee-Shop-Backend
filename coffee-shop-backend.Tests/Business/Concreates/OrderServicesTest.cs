using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Dto.Order;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace coffee_shop_backend.Tests.Business;

public class OrderServicesTest : IClassFixture<OrderServicesFixture>
{
    private readonly CoffeeShopTestDbContext _context;
    private readonly OrderServicesFixture _fixture;

    public OrderServicesTest(OrderServicesFixture fixture)
    {
        _fixture = fixture;
        _context = fixture.GetContext();
        _fixture.Dispose();
    }

    [Fact]
    public void OrderServices_AddOrder()
    {
        var orderServices = _fixture.CreateOrderServices();
        var product = TestHelper.GetTestProductWithStock();
        var user = TestHelper.GetTestUser();

        _context.Users.Add(user);
        _context.Products.Add(product);
        _context.SaveChanges();

        var request = new AddOrderRequest()
        {
            ProductId = 1,
            Address = "Test Address",
            Email = "Test Email",
            FullName = "Test Full Name"
        };

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var result = orderServices.AddOrder(request, token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void OrderServices_AddOrder_When_Stock_Is_Empty()
    {
        var orderServices = _fixture.CreateOrderServices();
        var product = TestHelper.GetTestProductWithStock();
        var user = TestHelper.GetTestUser();

        product.Stock.Amount = 0;
        _context.Users.Add(user);
        _context.Products.Add(product);
        _context.SaveChanges();

        var request = new AddOrderRequest()
        {
            ProductId = 1,
            Address = "Test Address",
            Email = "Test Email",
            FullName = "Test Full Name"
        };

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var result = orderServices.AddOrder(request, token);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void OrderServices_AddOrder_When_Token_Is_Invalid()
    {
        var orderServices = _fixture.CreateOrderServices();

        var request = new AddOrderRequest()
        {
            ProductId = 1,
            Address = "Test Address",
            Email = "Test Email",
            FullName = "Test Full Name"
        };

        var token = "invalid_token";

        var result = orderServices.AddOrder(request, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void OrderServices_GetOrdersByUserId()
    {
        var orderServices = _fixture.CreateOrderServices();
        var product = TestHelper.GetTestProductWithStock();
        var user = TestHelper.GetTestUser();
        var order = TestHelper.GetTestOrder();

        _context.Users.Add(user);
        _context.Products.Add(product);
        _context.Orders.Add(order);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var result = orderServices.GetOrdersByUserId(token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void OrderServices_GetOrdersByUserId_Token_Is_Not_Valid()
    {
        var orderServices = _fixture.CreateOrderServices();

        var token = "invalid_token";

        var result = orderServices.GetOrdersByUserId(token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void OrderServices_UpdateOrderStatus()
    {
        var orderServices = _fixture.CreateOrderServices();
        var product = TestHelper.GetTestProductWithStock();
        var user = TestHelper.GetTestAdminUser();
        var order = TestHelper.GetTestOrder();

        _context.Users.Add(user);
        _context.Products.Add(product);
        _context.Orders.Add(order);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var request = new UpdateOrderStatusRequest()
        {
            Status = EnumOrderStatus.Waiting
        };

        var result = orderServices.UpdateOrderStatus(request, 1, token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void OrderServices_UpdateOrderStatus_Token_Is_Not_Valid()
    {
        var orderServices = _fixture.CreateOrderServices();

        var token = "invalid_token";

        var request = new UpdateOrderStatusRequest()
        {
            Status = EnumOrderStatus.Waiting
        };

        var result = orderServices.UpdateOrderStatus(request, 1, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void OrderServices_UpdateOrderStatus_Order_Not_Found()
    {
        var orderServices = _fixture.CreateOrderServices();
        var product = TestHelper.GetTestProductWithStock();
        var user = TestHelper.GetTestAdminUser();

        _context.Users.Add(user);
        _context.Products.Add(product);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var request = new UpdateOrderStatusRequest()
        {
            Status = EnumOrderStatus.Waiting
        };

        var result = orderServices.UpdateOrderStatus(request, 1, token);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void OrderServices_UpdateOrderStatus_User_Not_Found()
    {
        var orderServices = _fixture.CreateOrderServices();
        var product = TestHelper.GetTestProductWithStock();
        var order = TestHelper.GetTestOrder();

        _context.Products.Add(product);
        _context.Orders.Add(order);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

        var request = new UpdateOrderStatusRequest()
        {
            Status = EnumOrderStatus.Waiting
        };

        var result = orderServices.UpdateOrderStatus(request, 1, token);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void OrderServices_UpdateOrderStatus_User_Is_Not_Admin()
    {
        var orderServices = _fixture.CreateOrderServices();
        var product = TestHelper.GetTestProductWithStock();
        var user = TestHelper.GetTestUser();
        var order = TestHelper.GetTestOrder();

        _context.Users.Add(user);
        _context.Products.Add(product);
        _context.Orders.Add(order);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var request = new UpdateOrderStatusRequest()
        {
            Status = EnumOrderStatus.Waiting
        };

        var result = orderServices.UpdateOrderStatus(request, 1, token);

        Assert.IsType<UnauthorizedResult>(result);
    }
}