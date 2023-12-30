using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Controllers;
using coffee_shop_backend.Dto.Order;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace coffee_shop_backend.Tests.Controllers;

public class OrderControllerTest
{
    private readonly IOrderServices _orderServices;

    public OrderControllerTest()
    {
        var orderServicesMock = new Mock<IOrderServices>();

        orderServicesMock.Setup(x => x.Add(It.IsAny<AddOrderRequest>(), It.IsAny<string>()))
            .Returns(new OkObjectResult("Success"));

        orderServicesMock.Setup(x => x.GetOrdersByUserId(It.IsAny<string>()))
            .Returns(new OkObjectResult("Success"));

        orderServicesMock.Setup(x =>
                x.Update(It.IsAny<UpdateOrderStatusRequest>(), It.IsAny<long>(), It.IsAny<string>()))
            .Returns(new OkObjectResult("Success"));

        _orderServices = orderServicesMock.Object;
    }

    [Fact]
    public void OrderController_AddOrder()
    {
        var controller = new OrderController(_orderServices);
        var request = new AddOrderRequest();
        var token = "token";

        var result = controller.AddOrder(request, token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void OrderController_GetOrdersByUserId()
    {
        var controller = new OrderController(_orderServices);
        var request = new AddOrderRequest();
        var token = "token";

        var result = controller.GetOrdersByUserId(token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void OrderController_UpdateOrderStatus()
    {
        var controller = new OrderController(_orderServices);
        var request = new UpdateOrderStatusRequest();
        var id = 1;
        var token = "token";

        var result = controller.UpdateOrderStatus(request, id, token);

        Assert.IsType<OkObjectResult>(result);
    }

}