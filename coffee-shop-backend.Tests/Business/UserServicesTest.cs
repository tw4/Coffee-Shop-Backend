using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Dto.User;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class UserServicesTest
{
    private readonly CoffeeShopTestDbContext _context;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<UserServices> _logger;

    public UserServicesTest()
    {
        _context = TestHelper.CreateCoffeeShopTestDbContext("UserServicesTest");
        _logger = new Logger<UserServices>(new LoggerFactory());
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
    }

    [Fact]
    public void UserServices_AddUser()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var request = new AddUserRequest()
        {
            Name = "Test",
            Surname = "Test",
            Email = "test_email",
            Password = "test_password"
        };

        var result = userServices.AddUser(request);

        TestHelper.DeleteUsersOnDatabase(_context);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserServices_AddUser_WhenUserAlreadyExists()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var request = new AddUserRequest()
        {
            Name = "Test",
            Surname = "Test",
            Email = "test_email",
            Password = "test_password"
        };

        userServices.AddUser(request);
        var result = userServices.AddUser(request);

        TestHelper.DeleteUsersOnDatabase(_context);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void UserServices_GetUserById()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var u = new User()
        {
            Id = 1,
            Name = "Test",
            Surname = "Test",
            Email = "test_email",
            Password = "test_password"
        };

        _context.Users.Add(u);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, "test_email");
        var user = userServices.GetUserById(token);


        TestHelper.DeleteUsersOnDatabase(_context);
        Assert.IsType<OkObjectResult>(user);
    }

    [Fact]
    public void UserServices_GetUserById_WhenUserNotFound()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var token = _jwtServices.GenerateJwtToken(1, "test_email");
        var user = userServices.GetUserById(token);

        Assert.IsType<BadRequestObjectResult>(user);
    }

    [Fact]
    public void UserServices_GetUserById_WhenUserToken_IsNotValid()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var token = "test_token";
        var user = userServices.GetUserById(token);

        Assert.IsType<UnauthorizedResult>(user);
    }

    [Fact]
    public void UserServices_DeleteById()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var u = new User()
        {
            Id = 1,
            Name = "Test",
            Surname = "Test",
            Email = "test_email",
            Password = "test_password"
        };

        _context.Users.Add(u);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, "test_email");
        var result = userServices.DeleteUserById(token);

        TestHelper.DeleteUsersOnDatabase(_context);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserServices_DeleteById_WhenUserNotFound()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var token = _jwtServices.GenerateJwtToken(1, "test_email");
        var result = userServices.DeleteUserById(token);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void UserServices_DeleteById_WhenUserToken_IsNotValid()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var token = "test_token";
        var result = userServices.DeleteUserById(token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void UserServices_UpdateUserPassword()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var u = new User()
        {
            Id = 1,
            Name = "Test",
            Surname = "Test",
            Email = "test_email",
            Password = "test_password"
        };

        _context.Users.Add(u);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, "test_email");
        var request = new UpdateUserPasswordRequest()
        {
            Password = "test_password1"
        };
        var result = userServices.UpdateUserPassword(token, request);

        TestHelper.DeleteUsersOnDatabase(_context);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserServices_UpdateUserPassword_WhenUserNotFound()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var token = _jwtServices.GenerateJwtToken(1, "test_email");
        var request = new UpdateUserPasswordRequest()
        {
            Password = "test_password1"
        };
        var result = userServices.UpdateUserPassword(token, request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void UserServices_UpdateUserPassword_WhenUserToken_IsNotValid()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var token = "test_token";
        var request = new UpdateUserPasswordRequest()
        {
            Password = "test_password1"
        };
        var result = userServices.UpdateUserPassword(token, request);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void UserServices_UpdateBasicUserInformation()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var u = new User()
        {
            Id = 1,
            Name = "Test",
            Surname = "Test",
            Email = "test_email",
            Password = "test_password"
        };

        _context.Users.Add(u);
        _context.SaveChanges();

        var token = _jwtServices.GenerateJwtToken(1, "test_email");
        var request = new UpdateBasicUserInformationRequest()
        {
            Name = "Test1",
            Surname = "Test1",
            Email = "test_email1"
        };
        var result = userServices.UpdateBasicUserInformation(token, request);

        TestHelper.DeleteUsersOnDatabase(_context);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserServices_UpdateBasicUserInformation_WhenUserNotFound()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var token = _jwtServices.GenerateJwtToken(1, "test_email");
        var request = new UpdateBasicUserInformationRequest()
        {
            Name = "Test1",
            Surname = "Test1",
            Email = "test_email1"
        };
        var result = userServices.UpdateBasicUserInformation(token, request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void UserServices_UpdateBasicUserInformation_WhenUserToken_IsNotValid()
    {
        var userServices = new UserServices(_context, _jwtServices, _logger);
        var token = "test_token";
        var request = new UpdateBasicUserInformationRequest()
        {
            Name = "Test1",
            Surname = "Test1",
            Email = "test_email1"
        };
        var result = userServices.UpdateBasicUserInformation(token, request);

        Assert.IsType<UnauthorizedResult>(result);
    }


}