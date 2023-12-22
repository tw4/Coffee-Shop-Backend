using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using LoginRequest = coffee_shop_backend.Dto.Auth.LoginRequest;

namespace coffee_shop_backend.Tests.Controllers;

public class AuthControllerTest
{
    private readonly IAuthServices _mockAuthServices;
    private readonly Logger<AuthController> _mockLogger;


    public AuthControllerTest()
    {
        var mockAuthServices = new Mock<IAuthServices>();
        var mockLogger = new Mock<Logger<AuthController>>(new LoggerFactory());

        mockAuthServices.Setup(x => x.UserLogin(It.IsAny<LoginRequest>())).Returns(new OkResult());
        mockAuthServices.Setup(x => x.Auth(It.IsAny<string>())).Returns(new OkResult());

        _mockAuthServices = mockAuthServices.Object;
        _mockLogger = mockLogger.Object;
    }

    [Fact]
    public void LoginTest()
    {
        var controller = new AuthController(_mockAuthServices, _mockLogger);
        var result = controller.Login(new LoginRequest());
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void VerifyTokenTest()
    {
        var controller = new AuthController(_mockAuthServices, _mockLogger);
        var result = controller.VerifyToken("token");
        Assert.IsType<OkResult>(result);

    }
}