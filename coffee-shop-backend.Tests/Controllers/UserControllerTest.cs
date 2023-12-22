using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Controllers;
using coffee_shop_backend.Dto.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace coffee_shop_backend.Tests.Controllers;

public class UserControllerTest
{
    private readonly IUserServices _mockUserServices;

    public UserControllerTest()
    {
    var mockUserServices = new Mock<IUserServices>();

        mockUserServices.Setup(x => x.AddUser(It.IsAny<AddUserRequest>())).Returns(new OkObjectResult("User added successfully"));

        mockUserServices.Setup(x => x.GetUserById(It.IsAny<string>())).Returns(new OkObjectResult("User found successfully"));

        mockUserServices.Setup(x => x.DeleteUserById(It.IsAny<string>())).Returns(new OkObjectResult("User deleted successfully"));

        mockUserServices.Setup(x => x.UpdateUserPassword(It.IsAny<string>(), It.IsAny<UpdateUserPasswordRequest>())).Returns(new OkObjectResult("User password updated successfully"));

        mockUserServices.Setup(x => x.UpdateBasicUserInformation(It.IsAny<string>(), It.IsAny<UpdateBasicUserInformationRequest>())).Returns(new OkObjectResult("User information updated successfully"));

        _mockUserServices = mockUserServices.Object;

    }

    [Fact]
    public void UserController_AddUser()
    {
        var controller = new UserController(_mockUserServices);
        var request = new AddUserRequest();

        var result = controller.AddUser(request);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserController_GetUserById()
    {
        var controller = new UserController(_mockUserServices);

        var result = controller.GetUserById("token");

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserController_DeleteUserById()
    {
        var controller = new UserController(_mockUserServices);

        var result = controller.DeleteUserById("token");

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserController_UpdateUserPassword()
    {
        var controller = new UserController(_mockUserServices);
        var request = new UpdateUserPasswordRequest();

        var result = controller.UpdateUserPassword("token", request);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserController_UpdateBasicUserInformation()
    {
        var controller = new UserController(_mockUserServices);
        var request = new UpdateBasicUserInformationRequest();

        var result = controller.UpdateBasicUserInformation("token", request);

        Assert.IsType<OkObjectResult>(result);
    }
    
}