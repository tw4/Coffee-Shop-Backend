using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class AuthServicesFixture: IDisposable
{
    private readonly CoffeeShopTestDbContext _context;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<AuthServices> _mockLogger;


    public AuthServicesFixture()
    {
        _context = TestHelper.CreateCoffeeShopTestDbContext("AuthServicesTest");
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>( new LoggerFactory()));
        _mockLogger = new Logger<AuthServices>(new LoggerFactory());
    }

    public AuthServices CreateAuthServices()
    {
        return new AuthServices(_context, _jwtServices, _mockLogger);
    }

    public CoffeeShopTestDbContext GetContext()
    {
        return _context;
    }

    public void Dispose()
    {
        TestHelper.DeleteUsersOnDatabase(_context);
    }
}