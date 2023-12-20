using coffee_shop_backend.Contexs;
using Microsoft.EntityFrameworkCore;

namespace coffee_shop_backend.Tests
{
    public class CoffeeShopTestDbContext : CoffeeShopDbContex
    {
        public CoffeeShopTestDbContext(DbContextOptions<CoffeeShopTestDbContext> options) : base(options)
        {
        }
    }
}