using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class StockServicesFixture: IDisposable
{
    private readonly CoffeeShopTestDbContext _contex;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<StockServices> _logger;

    public StockServicesFixture()
    {
        _contex = TestHelper.CreateCoffeeShopTestDbContext("StockServicesTest");
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
        _logger = new Logger<StockServices>(new LoggerFactory());
    }

    public StockServices CreateStockServices()
    {
        return new StockServices(_contex, _jwtServices, _logger);
    }

    public CoffeeShopTestDbContext GetDbContext()
    {
        return _contex;
    }

    public void Dispose()
    {
        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteStocksOnDatabase(_contex);
        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteOrdersOnDatabase(_contex);
    }
}