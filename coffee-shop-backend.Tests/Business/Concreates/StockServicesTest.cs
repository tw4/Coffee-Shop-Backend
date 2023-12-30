using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Dto.Stock;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class StockServicesTest: IClassFixture<StockServicesFixture>
{
    private CoffeeShopTestDbContext _context;
    private StockServicesFixture _fixture;

    public StockServicesTest(StockServicesFixture fixture)
    {
        _fixture = fixture;
        _context = fixture.GetDbContext();
        _fixture.Dispose();
    }

    [Fact]
    public void StockServices_Add_Stock()
    {
        var stockServices = _fixture.CreateStockServices();

        var product = TestHelper.GetTestProduct();

        var user = TestHelper.GetTestAdminUser();

        _context.Users.Add(user);
        _context.Products.Add(product);
        _context.SaveChanges();


        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var request = new AddStockRequest()
        {
            Amount = 1,
            ProductId = 1,
        };

        var result = stockServices.Add(request, token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void StockServices_Add_Stock_Invalid_Token()
    {
        var stockServices = _fixture.CreateStockServices();

        var token = "invalid_token";

        var request = new AddStockRequest()
        {
            Amount = 1,
            ProductId = 1,
        };

        var result = stockServices.Add(request, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void StockServices_Add_Stock_User_Not_Found()
    {
        var stockServices = _fixture.CreateStockServices();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

        var request = new AddStockRequest()
        {
            Amount = 1,
            ProductId = 1,
        };

        var result = stockServices.Add(request, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_Add_Stock_User_Not_Admin()
    {
        var stockServices = _fixture.CreateStockServices();

        var user = TestHelper.GetTestUser();

        _context.Users.Add(user);
        _context.SaveChanges();

        var token =  TestHelper.GenerateJwtToken(1, user.Email);

        var request = new AddStockRequest()
        {
            Amount = 1,
            ProductId = 1,
        };

        var result = stockServices.Add(request, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void StockServices_Add_Stock_Product_Not_Found()
    {
        var stockServices = _fixture.CreateStockServices();

        var user = TestHelper.GetTestAdminUser();

        _context.Users.Add(user);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var request = new AddStockRequest()
        {
            Amount = 1,
            ProductId = 1,
        };

        var result = stockServices.Add(request, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_UpdateStock()
    {
        var stockServices = _fixture.CreateStockServices();

        var product = TestHelper.GetTestProduct();

        var user = TestHelper.GetTestAdminUser();

        var stock = TestHelper.GetTestStock();

        _context.Users.Add(user);
        _context.Products.Add(product);
        _context.Stocks.Add(stock);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var request = new UpdateStockRequest()
        {
            Id = 1,
            Amount = 2,
        };

        var result = stockServices.Update(request, token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void StockServices_UpdateStock_Token_Is_Not_Valid()
    {
        var stockServices = _fixture.CreateStockServices();

        var token = "invalid_token";

        var request = new UpdateStockRequest()
        {
            Id = 1,
            Amount = 2,
        };

        var result = stockServices.Update(request, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void StockServices_UpdateStock_User_Not_Found()
    {
        var stockServices = _fixture.CreateStockServices();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

        var request = new UpdateStockRequest()
        {
            Id = 1,
            Amount = 2,
        };

        var result = stockServices.Update(request, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_UpdateStock_User_Not_Admin()
    {
        var stockServices = _fixture.CreateStockServices();

        var user = TestHelper.GetTestUser();

        _context.Users.Add(user);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var request = new UpdateStockRequest()
        {
            Id = 1,
            Amount = 2,
        };

        var result = stockServices.Update(request, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void StockServices_UpdateStock_Stock_Not_Found()
    {
        var stockServices = _fixture.CreateStockServices();

        var user = TestHelper.GetTestAdminUser();

        _context.Users.Add(user);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var request = new UpdateStockRequest()
        {
            Id = 1,
            Amount = 2,
        };

        var result = stockServices.Update(request, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_DeleteStock()
    {
        var stockServices = _fixture.CreateStockServices();

        var product = TestHelper.GetTestProduct();

        var user = TestHelper.GetTestAdminUser();

        var stock = TestHelper.GetTestStock();

        _context.Users.Add(user);
        _context.Products.Add(product);
        _context.Stocks.Add(stock);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var result = stockServices.DeleteById(1, token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void StockServices_DeleteStock_Token_Is_Not_Valid()
    {
        var stockServices = _fixture.CreateStockServices();

        var result = stockServices.DeleteById(1, "invalid_token");

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void StockServices_DeleteStock_Stock_Not_Found()
    {
        var stockServices = _fixture.CreateStockServices();

        var user = TestHelper.GetTestAdminUser();

        _context.Users.Add(user);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var result = stockServices.DeleteById(1, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_GetStockById()
    {
        var stockServices = _fixture.CreateStockServices();

        var user = TestHelper.GetTestAdminUser();

        var product = TestHelper.GetTestProduct();

        var stock = TestHelper.GetTestStock();

        _context.Products.Add(product);
        _context.Stocks.Add(stock);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var result = stockServices.GetById(1, token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void StockServices_GetStockById_Stock_Not_Found()
    {
        var stockServices = _fixture.CreateStockServices();

        var user = TestHelper.GetTestUser();

        _context.Users.Add(user);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var result = stockServices.GetById(1, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void StockServices_GetStockById_Token_Is_Not_Valid()
    {
        var stockServices = _fixture.CreateStockServices();

        var token = "invalid_token";

        var result = stockServices.GetById(1, token);

        Assert.IsType<UnauthorizedResult>(result);
    }
}