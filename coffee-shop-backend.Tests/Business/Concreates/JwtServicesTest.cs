using coffee_shop_backend.Business.Concreates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class JwtServicesTest: IClassFixture<JwtServicesFixture>
{

    private JwtServicesFixture _fixture;
    public JwtServicesTest( JwtServicesFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void JwtServices_GenerateJwtToken_Returns_Token()
    {
        var jwtServices = _fixture.CreateJwtServices();
        var user = TestHelper.GetTestUser();
        var token = jwtServices.GenerateJwtToken(user.Id, user.Email);

        Assert.NotNull(token);
        Assert.IsType<string>(token);
    }

    [Fact]
    public void JwtServices_GenerateJwtToken_Returns_UserId()
    {
        var jwtServices = _fixture.CreateJwtServices();
        var user = TestHelper.GetTestUser();
        var token = jwtServices.GenerateJwtToken(user.Id, user.Email);
        var userId = jwtServices.GetUserIdFromToken(token);

        Assert.IsType<long>(userId);
        Assert.Equal(user.Id, userId);
    }

    [Fact]
    public void JwtServices_GenereateJwtToken_Returns_UserEmail()
    {
        var jwtServices = _fixture.CreateJwtServices();
        var user = TestHelper.GetTestUser();
        var token = jwtServices.GenerateJwtToken(user.Id, user.Email);
        var userEmail = jwtServices.GetUserEmailFromToken(token);

        Assert.NotNull(userEmail);
        Assert.IsType<string>(userEmail);
        Assert.Equal(user.Email, userEmail);
    }

    [Fact]
    public void JwtServices_ValidateToken()
    {
        var jwtServices = _fixture.CreateJwtServices();
        var user = TestHelper.GetTestUser();
        var token = jwtServices.GenerateJwtToken(user.Id, user.Email);
        var isValid = jwtServices.IsTokenValid(token);

        Assert.True(isValid);
    }
}