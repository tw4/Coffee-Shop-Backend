using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Dto.Product;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace coffee_shop_backend.Tests.Business;

public class ProductServicesTest: IClassFixture<ProductServicesFixture>
{

    private readonly CoffeeShopTestDbContext _contex;
    private readonly ProductServicesFixture _fixture;

    public ProductServicesTest(ProductServicesFixture fixture)
    {
        _fixture = fixture;
        _contex = fixture.GetContex();
        _fixture.Dispose();
    }

    [Fact]
    public void ProductServices_AddProduct()
    {
        var productServices = _fixture.CreateProductServices();

        var user = TestHelper.GetTestAdminUser();

        _contex.Users.Add(user);
        _contex.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

        var request = new AddProductRequest
        {
            Name = "test_name",
            Price = 1,
            Description = "test_description",
            ImageUrl = "test_image_url",
        };

        var result = productServices.AddProduct(request, token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductServices_AddProduct_When_User_Not_Found()
    {
        var productServices = _fixture.CreateProductServices();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

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
        var productServices = _fixture.CreateProductServices();

        var user = TestHelper.GetTestUser();

        _contex.Users.Add(user);
        _contex.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, user.Email);

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
    public void ProductServices_AddProduct_Token_Is_Not_Valid()
    {
        var productServices = _fixture.CreateProductServices();

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
        var productServices = _fixture.CreateProductServices();

        var product = TestHelper.GetTestProduct();

        _contex.Products.Add(product);
        _contex.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

        var result = productServices.GetProductById(1, token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductServices_GetById_When_Product_Not_Found()
    {
        var productServices = _fixture.CreateProductServices();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

        var result = productServices.GetProductById(1, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ProductServices_GetById_Token_Is_Not_Valid()
    {
        var productServices = _fixture.CreateProductServices();

        var token = "test_token";

        var result = productServices.GetProductById(1, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
public void ProductServices_DeleteProductById()
    {
        var productServices = _fixture.CreateProductServices();
        var product = TestHelper.GetTestProduct();
        var user = TestHelper.GetTestAdminUser();

        _contex.Users.Add(user);
        _contex.Products.Add(product);
        _contex.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

        var result = productServices.DeleteProductById(1, token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductServices_DeleteProductById_When_User_Not_Found()
    {
        var productServices = _fixture.CreateProductServices();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

        var result = productServices.DeleteProductById(1, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ProductServices_DeleteProductById_When_User_Not_Admin()
    {
        var productServices = _fixture.CreateProductServices();

        var user = TestHelper.GetTestUser();

        _contex.Users.Add(user);
        _contex.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

        var result = productServices.DeleteProductById(1, token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void ProductServices_DeleteProductById_When_Product_Not_Found()
    {
        var productServices = _fixture.CreateProductServices();

        var user = TestHelper.GetTestAdminUser();

        _contex.Users.Add(user);
        _contex.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

        var result = productServices.DeleteProductById(1, token);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ProductServices_DeleteProductById_Token_Is_Not_Valid()
    {
        var productServices = _fixture.CreateProductServices();

        var result = productServices.DeleteProductById(1, "test_token");

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void ProductServices_GetProductsByPage()
    {
        var productServices = _fixture.CreateProductServices();

        var product = TestHelper.GetTestProduct();

        _contex.Products.Add(product);
        _contex.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

        var result = productServices.GetProductsByPage(0, token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductServices_GetProductsByPage_Token_Is_Not_Valid()
    {
        var productServices = _fixture.CreateProductServices();

        var result = productServices.GetProductsByPage(0, "test_token");

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void ProductServices_GetAllProducts()
    {
        var productServices = _fixture.CreateProductServices();

        var product = TestHelper.GetTestProduct();

        _contex.Products.Add(product);
        _contex.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, "test_email");

        var result = productServices.GetAllProducts(token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductServices_GetAllProducts_Token_Is_Not_Valid()
    {
        var productServices = _fixture.CreateProductServices();

        var result = productServices.GetAllProducts("test_token");

        Assert.IsType<UnauthorizedResult>(result);
    }
}