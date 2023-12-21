using coffee_shop_backend.Business.Concreates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class JwtServicesTest
{
    private readonly IConfiguration _configuration;
    private readonly Logger<JwtServices> _logger;

    public JwtServicesTest()
    {
        _configuration = TestHelper.CreateConfiguration();
        _logger = new Logger<JwtServices>(new LoggerFactory());
    }

    [Fact]
    public void JwtServices_GenerateJwtToken_Returns_Token()
    {
        var jwtServices = new JwtServices(_configuration, _logger);
        var id = 1;
        var email = "test_email";
        var token = jwtServices.GenerateJwtToken(id, email);

        Assert.NotNull(token);
        Assert.IsType<string>(token);
    }

    [Fact]
    public void JwtServices_GenerateJwtToken_Returns_UserId()
    {
        var jwtServices = new JwtServices(_configuration, _logger);
        var id = 1;
        var email = "test_email";
        var token = jwtServices.GenerateJwtToken(id, email);
        var userId = jwtServices.GetUserIdFromToken(token);

        Assert.IsType<long>(userId);
        Assert.Equal(id, userId);
    }

    [Fact]
    public void JwtServices_GenereateJwtToken_Returns_UserEmail()
    {
        var jwtServices = new JwtServices(_configuration, _logger);
        var id = 1;
        var email = "test_email";
        var token = jwtServices.GenerateJwtToken(id, email);
        var userEmail = jwtServices.GetUserEmailFromToken(token);

        Assert.NotNull(userEmail);
        Assert.IsType<string>(userEmail);
        Assert.Equal(email, userEmail);
    }

    [Fact]
    public void JwtServices_ValidateToken()
    {
        var jwtServices = new JwtServices(_configuration, _logger);
        var id = 1;
        var email = "test_email";
        var token = jwtServices.GenerateJwtToken(id, email);
        var isValid = jwtServices.IsTokenValid(token);

        Assert.True(isValid);
    }
}