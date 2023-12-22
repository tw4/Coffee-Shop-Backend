using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Dto.Order;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace coffee_shop_backend.Tests.Business;

public class OrderServicesTest
{
    private readonly CoffeeShopTestDbContext _contex;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<OrderServices> _logger;

    public OrderServicesTest()
    {
        _contex = TestHelper.CreateCoffeeShopTestDbContext("OrderServicesTest");
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
        _logger = new Logger<OrderServices>(new LoggerFactory());
    }

    [Fact]
    public void OrderServices_AddOrder()
    {
        var orderServices = new OrderServices(_contex, _jwtServices, _logger);

        var product = new Product()
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Description = "Test Description",
            ImageUrl = "Test Image Url",
            Stock = new Stock()
            {
                Id = 1,
                Amount = 10,
                ProductId = 1
            }
        };

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Role = EnumRole.USER,
            Name = "test_name",
            Surname = "test_surname"
        };

        _contex.Users.Add(user);
        _contex.Products.Add(product);
        _contex.SaveChanges();

        var request = new AddOrderRequest()
        {
            ProductId = 1,
            Address = "Test Address",
            Email = "Test Email",
            FullName = "Test Full Name"
        };

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var result = orderServices.AddOrder(request, token);

        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteOrdersOnDatabase(_contex);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void OrderServices_AddOrder_When_Stock_Is_Empty()
    {
        var orderServices = new OrderServices(_contex, _jwtServices, _logger);

        var product = new Product()
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Description = "Test Description",
            ImageUrl = "Test Image Url",
            Stock = new Stock()
            {
                Id = 1,
                Amount = 0,
                ProductId = 1
            }
        };

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Role = EnumRole.USER,
            Name = "test_name",
            Surname = "test_surname"
        };

        _contex.Users.Add(user);
        _contex.Products.Add(product);
        _contex.SaveChanges();

        var request = new AddOrderRequest()
        {
            ProductId = 1,
            Address = "Test Address",
            Email = "Test Email",
            FullName = "Test Full Name"
        };

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var result = orderServices.AddOrder(request, token);

        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteOrdersOnDatabase(_contex);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void OrderServices_AddOrder_When_Token_Is_Invalid()
    {
        var orderServices = new OrderServices(_contex, _jwtServices, _logger);

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
        var orderServices = new OrderServices(_contex, _jwtServices, _logger);

        var product = new Product()
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Description = "Test Description",
            ImageUrl = "Test Image Url",
            Stock = new Stock()
            {
                Id = 1,
                Amount = 10,
                ProductId = 1
            }
        };

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Role = EnumRole.USER,
            Name = "test_name",
            Surname = "test_surname"
        };

        var order = new Order()
        {
            Id = 1,
            ProductId = 1,
            UserId = 1,
            Status = EnumOrderStatus.Waiting,
            PaymentDate = DateTime.Now,
            Address = "Test Address",
            Email = "Test Email",
            FullName = "Test Full Name"
        };

        _contex.Users.Add(user);
        _contex.Products.Add(product);
        _contex.Orders.Add(order);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var result = orderServices.GetOrdersByUserId(token);

        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteOrdersOnDatabase(_contex);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void OrderServices_GetOrdersByUserId_Token_Is_Not_Valid()
    {
        var orderServices = new OrderServices(_contex, _jwtServices, _logger);

        var token = "invalid_token";

        var result = orderServices.GetOrdersByUserId(token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void OrderServices_UpdateOrderStatus()
    {
        var orderServices = new OrderServices(_contex, _jwtServices, _logger);

        var product = new Product()
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Description = "Test Description",
            ImageUrl = "Test Image Url",
            Stock = new Stock()
            {
                Id = 1,
                Amount = 10,
                ProductId = 1
            }
        };

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Role = EnumRole.ADMIN,
            Name = "test_name",
            Surname = "test_surname"
        };

        var order = new Order()
        {
            Id = 1,
            ProductId = 1,
            UserId = 1,
            Status = EnumOrderStatus.Waiting,
            PaymentDate = DateTime.Now,
            Address = "Test Address",
            Email = "Test Email",
            FullName = "Test Full Name"
        };

        _contex.Users.Add(user);
        _contex.Products.Add(product);
        _contex.Orders.Add(order);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var request = new UpdateOrderStatusRequest()
        {
            Status = EnumOrderStatus.Waiting
        };

        var result = orderServices.UpdateOrderStatus(request, 1, token);

        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteOrdersOnDatabase(_contex);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void OrderServices_UpdateOrderStatus_Token_Is_Not_Valid()
    {
        var orderServices = new OrderServices(_contex, _jwtServices, _logger);

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
        var orderServices = new OrderServices(_contex, _jwtServices, _logger);

        var product = new Product()
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Description = "Test Description",
            ImageUrl = "Test Image Url",
            Stock = new Stock()
            {
                Id = 1,
                Amount = 10,
                ProductId = 1
            }
        };

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Role = EnumRole.ADMIN,
            Name = "test_name",
            Surname = "test_surname"
        };

        _contex.Users.Add(user);
        _contex.Products.Add(product);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var request = new UpdateOrderStatusRequest()
        {
            Status = EnumOrderStatus.Waiting
        };

        var result = orderServices.UpdateOrderStatus(request, 1, token);

        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteUsersOnDatabase(_contex);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void OrderServices_UpdateOrderStatus_User_Not_Found()
    {
        var orderServices = new OrderServices(_contex, _jwtServices, _logger);

        var product = new Product()
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Description = "Test Description",
            ImageUrl = "Test Image Url",
            Stock = new Stock()
            {
                Id = 1,
                Amount = 10,
                ProductId = 1
            }
        };

        var order = new Order()
        {
            Id = 1,
            ProductId = 1,
            UserId = 1,
            Status = EnumOrderStatus.Waiting,
            PaymentDate = DateTime.Now,
            Address = "Test Address",
            Email = "Test Email",
            FullName = "Test Full Name"
        };

        _contex.Products.Add(product);
        _contex.Orders.Add(order);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var request = new UpdateOrderStatusRequest()
        {
            Status = EnumOrderStatus.Waiting
        };

        var result = orderServices.UpdateOrderStatus(request, 1, token);

        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteOrdersOnDatabase(_contex);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void OrderServices_UpdateOrderStatus_User_Is_Not_Admin()
    {
        var orderServices = new OrderServices(_contex, _jwtServices, _logger);

        var product = new Product()
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Description = "Test Description",
            ImageUrl = "Test Image Url",
            Stock = new Stock()
            {
                Id = 1,
                Amount = 10,
                ProductId = 1
            }
        };

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Role = EnumRole.USER,
            Name = "test_name",
            Surname = "test_surname"
        };

        var order = new Order()
        {
            Id = 1,
            ProductId = 1,
            UserId = 1,
            Status = EnumOrderStatus.Waiting,
            PaymentDate = DateTime.Now,
            Address = "Test Address",
            Email = "Test Email",
            FullName = "Test Full Name"
        };

        _contex.Users.Add(user);
        _contex.Products.Add(product);
        _contex.Orders.Add(order);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var request = new UpdateOrderStatusRequest()
        {
            Status = EnumOrderStatus.Waiting
        };

        var result = orderServices.UpdateOrderStatus(request, 1, token);

        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteOrdersOnDatabase(_contex);
        Assert.IsType<UnauthorizedResult>(result);
    }
}