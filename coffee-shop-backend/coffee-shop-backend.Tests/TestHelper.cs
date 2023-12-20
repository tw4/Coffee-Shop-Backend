using coffee_shop_backend.Entitys.Concreates;
using Microsoft.EntityFrameworkCore;

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
}