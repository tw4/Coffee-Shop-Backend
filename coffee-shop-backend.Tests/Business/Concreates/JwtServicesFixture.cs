using coffee_shop_backend.Business.Concreates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class JwtServicesFixture
{
    private readonly IConfiguration _configuration;
    private readonly Logger<JwtServices> _logger;

    public JwtServicesFixture()
    {
        _configuration = TestHelper.CreateConfiguration();
        _logger = new Logger<JwtServices>(new LoggerFactory());
    }

    public JwtServices CreateJwtServices()
    {
        return new JwtServices(_configuration, _logger);
    }
}