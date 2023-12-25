using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests;

public class TestHelper
{
    public static CoffeeShopTestDbContext CreateCoffeeShopTestDbContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<CoffeeShopTestDbContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        var context = new CoffeeShopTestDbContext(options);
        return context;
    }

    public static void DeleteUserOnDatabase(CoffeeShopTestDbContext context, User user)
    {
        context.Users.Remove(user);
        context.SaveChanges();
    }

    public static IConfiguration CreateConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static void DeleteUsersOnDatabase(CoffeeShopTestDbContext context)
    {
        // TODO: this is not a good way to delete all users on database because it is slow
        var users = context.Users.ToList();
        foreach (var user in users)
        {
            context.Users.Remove(user);
        }

        context.SaveChanges();
    }

    public static void DeleteProductsOnDatabase(CoffeeShopTestDbContext context)
    {
        var products = context.Products.ToList();
        foreach (var product in products)
        {
            context.Products.Remove(product);
        }

        context.SaveChanges();
    }

    public static void DeleteStocksOnDatabase(CoffeeShopTestDbContext context)
    {
        var stocks = context.Stocks.ToList();
        foreach (var stock in stocks)
        {
            context.Stocks.Remove(stock);
        }

        context.SaveChanges();
    }

    public static void DeleteOrdersOnDatabase(CoffeeShopTestDbContext context)
    {
        var orders = context.Orders.ToList();
        foreach (var order in orders)
        {
            context.Orders.Remove(order);
        }

        context.SaveChanges();
    }

    public static string GenerateJwtToken(int id, string email)
    {
        var jwtServices = new JwtServices(CreateConfiguration(), new Logger<JwtServices>(new LoggerFactory()));
        return jwtServices.GenerateJwtToken(id, email);
    }

    public static User GetTestUser()
    {
        return new User
        {
            Id = 1,
            Name = "Test",
            Surname = "Test",
            Email = "test_email_1",
            Password = "test_password_1",
            Orders = null,
            Role = EnumRole.USER
        };
    }

    public static User GetTestAdminUser()
    {
        return new User
        {
            Id = 1,
            Name = "Admin",
            Surname = "Admin",
            Email = "test_email_1",
            Password = "test_password_1",
            Orders = null,
            Role = EnumRole.ADMIN
        };
    }

    public static Product GetTestProduct()
    {
        return new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Stock = null
        };
    }

    public static Product GetTestProductWithStock()
    {
        return new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Description = "Test Description",
            ImageUrl = "Test Image Url",
            Stock = new Stock
            {
                Id = 1,
                ProductId = 1,
                Amount = 200,
            }
        };
    }

    public static Order GetTestOrder()
    {
        return new Order
        {
            Id = 1,
            Address = "Test Address",
            Email = "Test Email",
            FullName =  "Test Full Name",
            PaymentDate = DateTime.Now,
            Status = EnumOrderStatus.Ready,
            ProductId = 1,
            UserId = 1,
        };
    }
}