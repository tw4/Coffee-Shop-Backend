using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Dto.Product;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace coffee_shop_backend.Tests.Business;

public class ProductServicesTest
{
    private readonly CoffeeShopTestDbContext _context;
    private readonly IJwtServices _jwtServices;
    private readonly IRedisServices _mockRedisServices;
    private readonly Logger<ProductServices> _logger;

    public ProductServicesTest()
    {
        _context = TestHelper.CreateCoffeeShopTestDbContext("ProductServicesTest");
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
        _mockRedisServices = new Mock<IRedisServices>().Object;
        _logger = new Logger<ProductServices>(new LoggerFactory());
    }

    [Fact]
    public void ProductServices_AddProduct()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var user = new User
        {
            Id = 1,
            Name = "test_name",
            Surname = "test_surname",
            Email = "test_email",
            Password = "test_password",
            Role = EnumRole.ADMIN,
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var request = new AddProductRequest
        {
            Name = "test_name",
            Price = 1,
            Description = "test_description",
            ImageUrl = "test_image_url",
        };

        var result = productServices.AddProduct(request, token);

        TestHelper.DeleteUsersOnDatabase(_context);
        TestHelper.DeleteProductsOnDatabase(_context);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductServices_AddProduct_When_User_Not_Found()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var request = new AddProductRequest
        {
            Name = "test_name",
            Price = 1,
            Description = "test_description",
            ImageUrl = "test_image_url",
        };

        var result = productServices.AddProduct(request, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ProductServices_AddProduct_When_User_Not_Admin()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var user = new User
        {
            Id = 1,
            Name = "test_name",
            Surname = "test_surname",
            Email = "test_email",
            Password = "test_password",
            Role = EnumRole.USER,
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, user.Email);

        var request = new AddProductRequest
        {
            Name = "test_name",
            Price = 1,
            Description = "test_description",
            ImageUrl = "test_image_url",
        };

        var result = productServices.AddProduct(request, token);

        TestHelper.DeleteUsersOnDatabase(_context);
        TestHelper.DeleteProductsOnDatabase(_context);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void ProductServices_AddProduct_Token_Is_Not_Valid()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var token = "test_token";

        var request = new AddProductRequest
        {
            Name = "test_name",
            Price = 1,
            Description = "test_description",
            ImageUrl = "test_image_url",
        };

        var result = productServices.AddProduct(request, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void ProductServices_GetById()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var product = new Product
        {
            Id = 1,
            Name = "test_name",
            Price = 1,
            Description = "test_description",
            ImageUrl = "test_image_url",
        };

        _context.Products.Add(product);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var result = productServices.GetProductById(1, token);

        TestHelper.DeleteProductsOnDatabase(_context);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductServices_GetById_When_Product_Not_Found()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var result = productServices.GetProductById(1, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ProductServices_GetById_Token_Is_Not_Valid()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var token = "test_token";

        var result = productServices.GetProductById(1, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
public void ProductServices_DeleteProductById()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var product = new Product
        {
            Id = 1,
            Name = "test_name",
            Price = 1,
            Description = "test_description",
            ImageUrl = "test_image_url",
        };

        var user = new User()
        {
            Id = 1,
            Name = "test_name",
            Surname = "test_surname",
            Email = "test_email",
            Password = "test_password",
            Role = EnumRole.ADMIN,
        };

        _context.Users.Add(user);
        _context.Products.Add(product);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var result = productServices.DeleteProductById(1, token);

        TestHelper.DeleteProductsOnDatabase(_context);
        TestHelper.DeleteUsersOnDatabase(_context);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductServices_DeleteProductById_When_User_Not_Found()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var result = productServices.DeleteProductById(1, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ProductServices_DeleteProductById_When_User_Not_Admin()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var user = new User()
        {
            Id = 1,
            Name = "test_name",
            Surname = "test_surname",
            Email = "test_email",
            Password = "test_password",
            Role = EnumRole.USER,
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var result = productServices.DeleteProductById(1, token);

        TestHelper.DeleteUsersOnDatabase(_context);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void ProductServices_DeleteProductById_When_Product_Not_Found()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var user = new User()
        {
            Id = 1,
            Name = "test_name",
            Surname = "test_surname",
            Email = "test_email",
            Password = "test_password",
            Role = EnumRole.ADMIN,
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var result = productServices.DeleteProductById(1, token);

        TestHelper.DeleteUsersOnDatabase(_context);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ProductServices_DeleteProductById_Token_Is_Not_Valid()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var result = productServices.DeleteProductById(1, "test_token");

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void ProductServices_GetProductsByPage()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var product = new Product
        {
            Id = 1,
            Name = "test_name",
            Price = 1,
            Description = "test_description",
            ImageUrl = "test_image_url",
        };

        _context.Products.Add(product);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var result = productServices.GetProductsByPage(0, token);

        TestHelper.DeleteProductsOnDatabase(_context);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductServices_GetProductsByPage_Token_Is_Not_Valid()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var result = productServices.GetProductsByPage(0, "test_token");

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void ProductServices_GetAllProducts()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var product = new Product
        {
            Id = 1,
            Name = "test_name",
            Price = 1,
            Description = "test_description",
            ImageUrl = "test_image_url",
        };

        _context.Products.Add(product);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, "test_email");

        var result = productServices.GetAllProducts(token);

        TestHelper.DeleteProductsOnDatabase(_context);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductServices_GetAllProducts_Token_Is_Not_Valid()
    {
        var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

        var result = productServices.GetAllProducts("test_token");

        Assert.IsType<UnauthorizedResult>(result);
    }
}