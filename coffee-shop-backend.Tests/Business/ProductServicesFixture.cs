using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using Microsoft.Extensions.Logging;
using Moq;

namespace coffee_shop_backend.Tests.Business;

public class ProductServicesFixture:IDisposable
{
    private readonly CoffeeShopTestDbContext _contex;
    private readonly IJwtServices _jwtServices;
    private readonly IRedisServices _mockRedisServices;
    private readonly Logger<ProductServices> _logger;

    public ProductServicesFixture()
    {
        _contex = TestHelper.CreateCoffeeShopTestDbContext("ProductServicesTest");
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
        _mockRedisServices = new Mock<IRedisServices>().Object;
        _logger = new Logger<ProductServices>(new LoggerFactory());
    }

    public ProductServices CreateProductServices()
    {
        return new ProductServices(_contex, _jwtServices, _mockRedisServices, _logger);
    }

    public CoffeeShopTestDbContext GetContex()
    {
        return _contex;
    }

    public void Dispose()
    {
        TestHelper.DeleteUsersOnDatabase(_contex);
        TestHelper.DeleteProductsOnDatabase(_contex);
    }

}