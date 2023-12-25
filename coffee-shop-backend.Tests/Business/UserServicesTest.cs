using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Dto.User;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class UserServicesTest: IClassFixture<UserServicesFixture>
{

    private readonly CoffeeShopTestDbContext _context;
    private readonly UserServicesFixture _fixture;

    public UserServicesTest(UserServicesFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.GetContext();
        _fixture.Dispose();
    }


    [Fact]
    public void UserServices_AddUser()
    {
        var userServices = _fixture.CreateUserServices();
        var request = new AddUserRequest()
        {
            Name = "Test",
            Surname = "Test",
            Email = "test_email",
            Password = "test_password"
        };

        var result = userServices.AddUser(request);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserServices_AddUser_WhenUserAlreadyExists()
    {
        var userServices = _fixture.CreateUserServices();
        var request = new AddUserRequest()
        {
            Name = "Test",
            Surname = "Test",
            Email = "test_email",
            Password = "test_password"
        };

        userServices.AddUser(request);
        var result = userServices.AddUser(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void UserServices_GetUserById()
    {
        var userServices = _fixture.CreateUserServices();
        var u = TestHelper.GetTestUser();

        _context.Users.Add(u);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, u.Email);
        var user = userServices.GetUserById(token);

        Assert.IsType<OkObjectResult>(user);
    }

    [Fact]
    public void UserServices_GetUserById_WhenUserNotFound()
    {
        var userServices = _fixture.CreateUserServices();
        var token = TestHelper.GenerateJwtToken(1, "test_email");
        var user = userServices.GetUserById(token);

        Assert.IsType<BadRequestObjectResult>(user);
    }

    [Fact]
    public void UserServices_GetUserById_WhenUserToken_IsNotValid()
    {
        var userServices = _fixture.CreateUserServices();
        var token = "test_token";
        var user = userServices.GetUserById(token);

        Assert.IsType<UnauthorizedResult>(user);
    }

    [Fact]
    public void UserServices_DeleteById()
    {
        var userServices = _fixture.CreateUserServices();
        var u = TestHelper.GetTestUser();

        _context.Users.Add(u);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, "test_email");
        var result = userServices.DeleteUserById(token);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserServices_DeleteById_WhenUserNotFound()
    {
        var userServices = _fixture.CreateUserServices();
        var token = TestHelper.GenerateJwtToken(1, "test_email");
        var result = userServices.DeleteUserById(token);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void UserServices_DeleteById_WhenUserToken_IsNotValid()
    {
        var userServices = _fixture.CreateUserServices();
        var token = "test_token";
        var result = userServices.DeleteUserById(token);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void UserServices_UpdateUserPassword()
    {
        var userServices = _fixture.CreateUserServices();
        var u = TestHelper.GetTestUser();

        _context.Users.Add(u);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, "test_email");
        var request = new UpdateUserPasswordRequest()
        {
            Password = "test_password1"
        };
        var result = userServices.UpdateUserPassword(token, request);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserServices_UpdateUserPassword_WhenUserNotFound()
    {
        var userServices = _fixture.CreateUserServices();
        var token = TestHelper.GenerateJwtToken(1, "test_email");
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
        var userServices = _fixture.CreateUserServices();
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
        var userServices = _fixture.CreateUserServices();
        var u = TestHelper.GetTestUser();

        _context.Users.Add(u);
        _context.SaveChanges();

        var token = TestHelper.GenerateJwtToken(1, "test_email");
        var request = new UpdateBasicUserInformationRequest()
        {
            Name = "Test1",
            Surname = "Test1",
            Email = "test_email1"
        };
        var result = userServices.UpdateBasicUserInformation(token, request);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UserServices_UpdateBasicUserInformation_WhenUserNotFound()
    {
        var userServices = _fixture.CreateUserServices();
        var token = TestHelper.GenerateJwtToken(1, "test_email");
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
        var userServices = _fixture.CreateUserServices();
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