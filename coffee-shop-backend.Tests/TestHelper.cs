using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace coffee_shop_backend.Tests;

public class TestHelper
{
    public static CoffeeShopTestDbContext CreateCoffeeShopTestDbContext()
    {
        var options = new DbContextOptionsBuilder<CoffeeShopTestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
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
}