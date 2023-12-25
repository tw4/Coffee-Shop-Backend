using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class UserServicesFixture:IDisposable
{
    private readonly CoffeeShopTestDbContext _context;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<UserServices> _logger;


    public UserServicesFixture()
    {
        _context = TestHelper.CreateCoffeeShopTestDbContext("UserServicesTest");
        _logger = new Logger<UserServices>(new LoggerFactory());
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
    }

    public UserServices CreateUserServices()
    {
        return new UserServices(_context, _jwtServices, _logger);
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