using coffee_shop_backend.Entitys.Concreates;
using Microsoft.EntityFrameworkCore;

namespace coffee_shop_backend.Contexs;

public class CoffeeShopContex : DbContext
{
   public DbSet<User> Users { get; set; }
   public DbSet<Product> Products { get; set; }
   public DbSet<Stock> Stocks { get; set; }
   public DbSet<Order> Orders { get; set; }

    public CoffeeShopContex(DbContextOptions<CoffeeShopContex> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword");
        }
    }
}