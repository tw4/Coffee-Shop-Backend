using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using Microsoft.Extensions.Logging;
using Moq;

namespace coffee_shop_backend.Tests.Business;

public class ProductServicesFixture:IDisposable
{
    private readonly CoffeeShopTestDbContext _context;
    private readonly IJwtServices _jwtServices;
    private readonly IRedisServices _mockRedisServices;
    private readonly Logger<ProductServices> _logger;

    public ProductServicesFixture()
    {
        _context = TestHelper.CreateCoffeeShopTestDbContext("ProductServicesTest");
        _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
        _mockRedisServices = new Mock<IRedisServices>().Object;
        _logger = new Logger<ProductServices>(new LoggerFactory());
    }

    public ProductServices CreateProductServices()
    {
        return new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);
    }

    public CoffeeShopTestDbContext GetContex()
    {
        return _context;
    }

    public void Dispose()
    {
        TestHelper.DeleteUsersOnDatabase(_context);
        TestHelper.DeleteProductsOnDatabase(_context);
    }

}