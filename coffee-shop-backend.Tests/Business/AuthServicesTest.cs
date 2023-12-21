using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Dto.Auth;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class AuthServicesTest
{

    private readonly CoffeeShopTestDbContext _context;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<AuthServices> _mockLogger;

    public AuthServicesTest()
    {
        _context = TestHelper.CreateCoffeeShopTestDbContext();
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>( new LoggerFactory()));
        _mockLogger = new Logger<AuthServices>(new LoggerFactory());
    }

    [Fact]
    public void Auth_Login_Test()
    {
        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Surname = "Admin",
            Email = "test_email_1",
            Password = "test_password_1",
            Orders = null,
            Role = EnumRole.ADMIN
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = new AuthServices(_context, _jwtServices, _mockLogger);

        var result = authServices.UserLogin(new LoginRequest()
        {
            Email = "test_email_1",
            Password = "test_password_1"
        });

        TestHelper.DeleteUserOnDatabase(_context, user);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Auth_Login_Test_With_Wrong_Password()
    {
        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Surname = "Admin",
            Email = "test_email_1",
            Password = "test_password_1",
            Orders = null,
            Role = EnumRole.ADMIN
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = new AuthServices(_context,_jwtServices ,_mockLogger);

        var result = authServices.UserLogin(new LoginRequest()
        {
            Email = "test_email_1",
            Password = "test_password_2"
        });

        TestHelper.DeleteUserOnDatabase(_context, user);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void AuthServices_Auth_Test()
    {
        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Surname = "Admin",
            Email = "test_email_1",
            Password = "test_password_1",
            Orders = null,
            Role = EnumRole.ADMIN
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = new AuthServices(_context, _jwtServices, _mockLogger);

        var token = _jwtServices.GenerateJwtToken(1, "test_email_1");
        var result = authServices.Auth(token);

        TestHelper.DeleteUserOnDatabase(_context, user);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void AuthServices_Auth_Test_With_Wrong_Token()
    {
        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Surname = "Admin",
            Email = "test_email_1",
            Password = "test_password_1",
            Orders = null,
            Role = EnumRole.ADMIN
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = new AuthServices(_context, _jwtServices, _mockLogger);

        var token = _jwtServices.GenerateJwtToken(1, "test_email_2");
        var result = authServices.Auth(token);

        TestHelper.DeleteUserOnDatabase(_context, user);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void AuthServices_Auth_Test_With_Null_Token()
    {
        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Surname = "Admin",
            Email = "test_email_1",
            Password = "test_password_1",
            Orders = null,
            Role = EnumRole.ADMIN
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = new AuthServices(_context, _jwtServices, _mockLogger);

        var result = authServices.Auth(null);

        TestHelper.DeleteUserOnDatabase(_context, user);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void AuthServices_Auth_Test_With_Invalid_Token()
    {
        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Surname = "Admin",
            Email = "test_email_1",
            Password = "test_password_1",
            Orders = null,
            Role = EnumRole.ADMIN
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var authServices = new AuthServices(_context, _jwtServices, _mockLogger);

        var token = _jwtServices.GenerateJwtToken(1, "test_email_1");
        var result = authServices.Auth(token + "1");

        TestHelper.DeleteUserOnDatabase(_context, user);
        Assert.IsType<UnauthorizedResult>(result);
    }

}