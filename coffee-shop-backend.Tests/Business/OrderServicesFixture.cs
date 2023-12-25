using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests.Business;

public class OrderServicesFixture:IDisposable
{
    private readonly CoffeeShopTestDbContext _contex;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<OrderServices> _logger;

    public OrderServicesFixture()
    {
        _contex = TestHelper.CreateCoffeeShopTestDbContext("OrderServicesTest");
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
        _logger = new Logger<OrderServices>(new LoggerFactory());
    }

    public OrderServices CreateOrderServices()
    {
        return new OrderServices(_contex, _jwtServices, _logger);
    }

    public CoffeeShopTestDbContext GetContext()
    {
        return _contex;
    }

    public void Dispose()
    {
        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteProductsOnDatabase(_contex);
        TestHelper.DeleteStocksOnDatabase(_contex);
        TestHelper.DeleteOrdersOnDatabase(_contex);
    }


}