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
    }
}
