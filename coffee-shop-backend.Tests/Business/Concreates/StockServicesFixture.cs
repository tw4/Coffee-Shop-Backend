using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class StockServicesFixture: IDisposable
{
    private readonly CoffeeShopTestDbContext _context;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<StockServices> _logger;

    public StockServicesFixture()
    {
        _context = TestHelper.CreateCoffeeShopTestDbContext("StockServicesTest");
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
        _logger = new Logger<StockServices>(new LoggerFactory());
    }

    public StockServices CreateStockServices()
    {
        return new StockServices(_context, _jwtServices, _logger);
    }

    public CoffeeShopTestDbContext GetDbContext()
    {
        return _context;
    }

    public void Dispose()
    {
        TestHelper.DeleteUsersOnDatabase(_context);
        TestHelper.DeleteStocksOnDatabase(_context);
        TestHelper.DeleteProductsOnDatabase(_context);
        TestHelper.DeleteOrdersOnDatabase(_context);
    }
}