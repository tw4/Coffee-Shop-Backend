using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Controllers;
using coffee_shop_backend.Dto.Stock;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace coffee_shop_backend.Tests.Controllers;

public class StockControllerTest
{
    private readonly IStockServices _mockStockServices;


    public StockControllerTest()
    {
        var mockStockServices = new Mock<IStockServices>();

        mockStockServices.Setup(services => services.Add(It.IsAny<AddStockRequest>(), It.IsAny<string>()))
            .Returns(new OkObjectResult("AddStock"));

        mockStockServices.Setup(services => services.Update(It.IsAny<UpdateStockRequest>(), It.IsAny<string>()))
            .Returns(new OkObjectResult("UpdateStock"));

        mockStockServices.Setup(services => services.DeleteById(It.IsAny<long>(), It.IsAny<string>()))
            .Returns(new OkObjectResult("DeleteStock"));

        mockStockServices.Setup(services => services.GetById(It.IsAny<long>(), It.IsAny<string>()))
            .Returns( new OkObjectResult("GetStockById"));

        _mockStockServices = mockStockServices.Object;
    }

    [Fact]
    public void StockController_AddStock()
    {
        var controller = new StockController(_mockStockServices);
        var result = controller.AddStock(new AddStockRequest(), "token");

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void StockController_UpdateStock()
    {
        var controller = new StockController(_mockStockServices);
        var result = controller.UpdateStock(new UpdateStockRequest(), "token");

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void StockController_DeleteStock()
    {
        var controller = new StockController(_mockStockServices);
        var result = controller.DeleteStock(1, "token");

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void StockController_GetStockById()
    {
        var controller = new StockController(_mockStockServices);
        var result = controller.GetStockById(1, "token");

        Assert.IsType<OkObjectResult>(result);
    }

}