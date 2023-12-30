using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Controllers;
using coffee_shop_backend.Dto.Product;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace coffee_shop_backend.Tests.Controllers;

public class ProductControllerTest
{
    private readonly IProductServices _mockProductServices;

    public ProductControllerTest()
    {
        var mockProductServices = new Mock<IProductServices>();

        mockProductServices.Setup(x => x.Add(It.IsAny<AddProductRequest>(), It.IsAny<string>()))
            .Returns(new OkObjectResult("AddProduct"));

        mockProductServices.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<string>()))
            .Returns(new OkObjectResult("GetProductById"));

        mockProductServices.Setup(x => x.DeleteById(It.IsAny<long>(), It.IsAny<string>()))
            .Returns(new OkObjectResult("DeleteProductById"));

        mockProductServices.Setup(x => x.GetProductsByPage(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(new OkObjectResult("GetProductsByPage"));

        mockProductServices.Setup(x => x.GetAllProducts(It.IsAny<string>()))
            .Returns(new OkObjectResult("GetAllProducts"));

        _mockProductServices = mockProductServices.Object;
    }

    [Fact]
    public void ProductController_AddProduct()
    {
        var controller = new ProductController(_mockProductServices);
        var result = controller.AddProduct(new AddProductRequest(), "token");
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductController_GetProductById()
    {
        var controller = new ProductController(_mockProductServices);
        var result = controller.GetProductById(1, "token");
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductController_DeleteProductById()
    {
        var controller = new ProductController(_mockProductServices);
        var result = controller.DeleteProductById(1, "token");
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductController_GetProductsByPage()
    {
        var controller = new ProductController(_mockProductServices);
        var result = controller.GetProductsByPage(1, "token");
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ProductController_GetAllProducts()
    {
        var controller = new ProductController(_mockProductServices);
        var result = controller.GetAllProducts("token");
        Assert.IsType<OkObjectResult>(result);
    }
}