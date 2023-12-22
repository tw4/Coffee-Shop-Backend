using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Dto.Product;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace coffee_shop_backend.Tests.Business;

public class ProductServicesTest
{
       private readonly CoffeeShopTestDbContext _context;
       private readonly IJwtServices _jwtServices;
       private readonly IRedisServices _mockRedisServices;
       private readonly Logger<ProductServices> _logger;

       public ProductServicesTest()
       {
           _context = TestHelper.CreateCoffeeShopTestDbContext("ProductServicesTest");
           _jwtServices = new JwtServices(TestHelper.CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
          _mockRedisServices = new Mock<IRedisServices>().Object;
           _logger = new Logger<ProductServices>(new LoggerFactory());
       }

       [Fact]
       public void ProductServices_AddProduct()
       {
           var productServices = new ProductServices(_context, _jwtServices, _mockRedisServices, _logger);

           var user = new User
           {
               Id = 1,
               Name = "test_name",
               Surname = "test_surname",
               Email = "test_email",
               Password = "test_password",
               Role = EnumRole.USER,
           };

           _context.Users.Add(user);
           _context.SaveChanges();

           var token = _jwtServices.GenerateJwtToken(1,user.Email );

           var request = new AddProductRequest
           {
               Name = "test_name",
                Price = 1,
                Description = "test_description",
                ImageUrl = "test_image_url",
           };

           var result = productServices.AddProduct(request, token);

           TestHelper.DeleteUsersOnDatabase(_context);
           TestHelper.DeleteProductsOnDatabase(_context);
           Assert.IsType<OkObjectResult>(result);
       }
}