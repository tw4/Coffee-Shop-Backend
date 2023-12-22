using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Dto.Stock;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class StockServicesTest
{
    private readonly CoffeeShopTestDbContext _contex;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<StockServices> _logger;


    public StockServicesTest()
    {
        _contex = TestHelper.CreateCoffeeShopTestDbContext("StockServicesTest");
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
        _logger = new Logger<StockServices>(new LoggerFactory());
    }

    [Fact]
    public void StockServices_Add_Stock()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var product = new Product()
        {
            Id = 1,
            Description = "Test",
            Name = "Test",
            Price = 1,
            ImageUrl = "Test",
        };

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Name = "test_name",
            Surname = "test_surname",
            Role = EnumRole.ADMIN,
        };

        _contex.Users.Add(user);
        _contex.Products.Add(product);
        _contex.SaveChanges();


        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var request = new AddStockRequest()
        {
            Amount = 1,
            ProductId = 1,
        };

        var result = stockServices.AddStock(request, token);

        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteStocksOnDatabase(_contex);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void StockServices_Add_Stock_Invalid_Token()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var token = "invalid_token";

        var request = new AddStockRequest()
        {
            Amount = 1,
            ProductId = 1,
        };

        var result = stockServices.AddStock(request, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void StockServices_Add_Stock_User_Not_Found()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var request = new AddStockRequest()
        {
            Amount = 1,
            ProductId = 1,
        };

        var result = stockServices.AddStock(request, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_Add_Stock_User_Not_Admin()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Name = "test_name",
            Surname = "test_surname",
            Role = EnumRole.USER,
        };

        _contex.Users.Add(user);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var request = new AddStockRequest()
        {
            Amount = 1,
            ProductId = 1,
        };

        var result = stockServices.AddStock(request, token);

        TestHelper.DeleteUsersOnDatabase(_contex);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void StockServices_Add_Stock_Product_Not_Found()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Name = "test_name",
            Surname = "test_surname",
            Role = EnumRole.ADMIN,
        };

        _contex.Users.Add(user);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var request = new AddStockRequest()
        {
            Amount = 1,
            ProductId = 1,
        };

        var result = stockServices.AddStock(request, token);

        TestHelper.DeleteUsersOnDatabase(_contex);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_UpdateStock()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var product = new Product()
        {
            Id = 1,
            Description = "Test",
            Name = "Test",
            Price = 1,
            ImageUrl = "Test",
        };

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Name = "test_name",
            Surname = "test_surname",
            Role = EnumRole.ADMIN,
        };

        var stock = new Stock()
        {
            Id = 1,
            ProductId = 1,
            Amount = 1,
        };

        _contex.Users.Add(user);
        _contex.Products.Add(product);
        _contex.Stocks.Add(stock);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var request = new UpdateStockRequest()
        {
            Id = 1,
            Amount = 2,
        };

        var result = stockServices.UpdateStock(request, token);

        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteStocksOnDatabase(_contex);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void StockServices_UpdateStock_Token_Is_Not_Valid()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var token = "invalid_token";

        var request = new UpdateStockRequest()
        {
            Id = 1,
            Amount = 2,
        };

        var result = stockServices.UpdateStock(request, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void StockServices_UpdateStock_User_Not_Found()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var request = new UpdateStockRequest()
        {
            Id = 1,
            Amount = 2,
        };

        var result = stockServices.UpdateStock(request, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_UpdateStock_User_Not_Admin()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Name = "test_name",
            Surname = "test_surname",
            Role = EnumRole.USER,
        };

        _contex.Users.Add(user);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var request = new UpdateStockRequest()
        {
            Id = 1,
            Amount = 2,
        };

        var result = stockServices.UpdateStock(request, token);

        TestHelper.DeleteUsersOnDatabase(_contex);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void StockServices_UpdateStock_Stock_Not_Found()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Name = "test_name",
            Surname = "test_surname",
            Role = EnumRole.ADMIN,
        };

        _contex.Users.Add(user);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var request = new UpdateStockRequest()
        {
            Id = 1,
            Amount = 2,
        };

        var result = stockServices.UpdateStock(request, token);

        TestHelper.DeleteUsersOnDatabase(_contex);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_DeleteStock()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var product = new Product()
        {
            Id = 1,
            Description = "Test",
            Name = "Test",
            Price = 1,
            ImageUrl = "Test",
        };

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Name = "test_name",
            Surname = "test_surname",
            Role = EnumRole.ADMIN,
        };

        var stock = new Stock()
        {
            Id = 1,
            ProductId = 1,
            Amount = 1,
        };

        _contex.Users.Add(user);
        _contex.Products.Add(product);
        _contex.Stocks.Add(stock);
        _contex.SaveChanges();


        var token = _jwtServices.GenerateJwtToken(1, user.Email);


        var result = stockServices.DeleteStock(1, token);

        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteStocksOnDatabase(_contex);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void StockServices_DeleteStock_Token_Is_Not_Valid()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var result = stockServices.DeleteStock(1, "invalid_token");

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void StockServices_DeleteStock_Stock_Not_Found()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Name = "test_name",
            Surname = "test_surname",
            Role = EnumRole.ADMIN,
        };

        _contex.Users.Add(user);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var result = stockServices.DeleteStock(1, token);

        TestHelper.DeleteUsersOnDatabase(_contex);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_GetStockById()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Name = "test_name",
            Surname = "test_surname",
        };

        var product = new Product()
        {
            Id = 1,
            Description = "Test",
            Name = "Test",
            Price = 1,
            ImageUrl = "Test",
        };

        var stock = new Stock()
        {
            Id = 1,
            ProductId = 1,
            Amount = 1,
        };

        _contex.Products.Add(product);
        _contex.Stocks.Add(stock);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var result = stockServices.GetStockById(1, token);

        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteStocksOnDatabase(_contex);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void StockServices_GetStockById_Stock_Not_Found()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var user = new User()
        {
            Id = 1,
            Email = "test_email",
            Password = "test_password",
            Name = "test_name",
            Surname = "test_surname",
        };

        _contex.Users.Add(user);
        _contex.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var result = stockServices.GetStockById(1, token);

        TestHelper.DeleteUsersOnDatabase(_contex);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_GetStockById_Token_Is_Not_Valid()
    {
        var stockServices = new StockServices(_contex, _jwtServices, _logger);

        var token = "invalid_token";

        var result = stockServices.GetStockById(1, token);

        Assert.IsType<UnauthorizedResult>(result);
    }
}