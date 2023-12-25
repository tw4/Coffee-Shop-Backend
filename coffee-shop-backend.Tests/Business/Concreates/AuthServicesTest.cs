using coffee_shop_backend.Dto.Auth;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Tests.Business;

public class AuthServicesTest: IClassFixture<AuthServicesFixture>
{
    private readonly AuthServicesFixture _fixture;
    private readonly CoffeeShopTestDbContext _context;

    public AuthServicesTest(AuthServicesFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.GetContext();
        _fixture.Dispose();
    }

    [Fact]
    public void Auth_Login_Test()
    {

        var user = TestHelper.GetTestUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = _fixture.CreateAuthServices();

        var result = authServices.UserLogin(new LoginRequest()
        {
            Email = "test_email_1",
            Password = "test_password_1"
        });

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Auth_Login_Test_With_Wrong_Password()
    {
        var user = TestHelper.GetTestAdminUser();

        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = _fixture.CreateAuthServices();

        var result = authServices.UserLogin(new LoginRequest()
        {
            Email = "test_email_1",
            Password = "test_password_2"
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void AuthServices_Auth_Test()
    {
        var user = TestHelper.GetTestUser();

        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = _fixture.CreateAuthServices();

        var token = TestHelper.GenerateJwtToken(1, "test_email_1");
        var result = authServices.Auth(token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void AuthServices_Auth_Test_With_Wrong_Token()
    {
        var user = TestHelper.GetTestUser();

        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = _fixture.CreateAuthServices();

        var token = TestHelper.GenerateJwtToken(1, "test_email_2");
        var result = authServices.Auth(token);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void AuthServices_Auth_Test_With_Null_Token()
    {
        var user = TestHelper.GetTestUser();

        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = _fixture.CreateAuthServices();

        var result = authServices.Auth(null);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void AuthServices_Auth_Test_With_Invalid_Token()
    {
        var user = TestHelper.GetTestUser();

        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = _fixture.CreateAuthServices();

        var token = TestHelper.GenerateJwtToken(1, "test_email_1");
        var result = authServices.Auth(token + "1");

        Assert.IsType<UnauthorizedResult>(result);
    }
}