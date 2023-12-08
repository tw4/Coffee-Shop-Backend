using System.Text;
using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Contexs;
using Microsoft.EntityFrameworkCore;
using Nest;
using Stripe;
using Product = coffee_shop_backend.Entitys.Concreates.Product;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// connection string for azure sql edge
var connectionString = "server=localhost, 1433;database=Databases;user=sa;password=P@ssw0rd1234; TrustServerCertificate=True";
builder.Services.AddDbContext<CoffeeShopDbContex>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// elasticsearch
builder.Services.AddSingleton<ElasticClient>(sp =>
{
    var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
        .DefaultIndex("coffee-shop");

    return new ElasticClient(settings);
});

// stripe configuration
StripeConfiguration.ApiKey = configuration["StripeSecretKey"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


// Add scoped services
builder.Services.AddScoped<IJwtServices, JwtManager>();
builder.Services.AddScoped<IAuthServices, AuthManager>();
builder.Services.AddScoped<IUserServices, UserManager>();
builder.Services.AddScoped<IProductServices, ProductManager>();
builder.Services.AddScoped<IStockServices, StockManager>();
builder.Services.AddScoped<IOrderServices, OrderManager>();
builder.Services.AddScoped<IRedisServices, RedisManager>();
builder.Services.AddScoped<IProductElasticSearchServices<Product>, ProductElasticSearchManager<Product>>();
builder.Services.AddScoped<IPaymentServices,PaymentManager>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();
app.Run();