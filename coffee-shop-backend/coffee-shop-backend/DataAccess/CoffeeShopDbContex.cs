using System.Security.Cryptography;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.EntityFrameworkCore;

namespace coffee_shop_backend.Contexs;

public class CoffeeShopDbContex : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Order> Orders { get; set; }

    public CoffeeShopDbContex(DbContextOptions<CoffeeShopDbContex> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // seed admin user
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "Admin",
                Surname = "Admin",
                Email = "admin@example.com",
                Password = "123456",
                Role = EnumRole.ADMIN
            }
        );

        // seed products
        List<Product> products = new List<Product>();

        for (int i = 1; i < 10000; i++)
        {
           Product p = new Product
           {
               Id = i,
               Name = $"Product {i}",
               Description = $"Product {i} description",
               ImageUrl = $"www.example.com/{i}.jpg",
               Price =  RandomNumberGenerator.GetInt32(1, 1000),
           };
           products.Add(p);
        }

        modelBuilder.Entity<Product>().HasData(products);

    }
}
