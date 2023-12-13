using System.Text;
using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Contexs;
using Microsoft.EntityFrameworkCore;
using Nest;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Stripe;
using Product = coffee_shop_backend.Entitys.Concreates.Product;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// serilog configuration
Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
builder.Host.UseSerilog(((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)));

// connection string for azure sql edge
builder.Services.AddDbContext<CoffeeShopDbContex>(options =>
    options.UseSqlServer(configuration["ConectionStrings:DefaultConnection"]));

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
builder.Services.AddScoped<Logger<UserManager>>();
builder.Services.AddScoped<Logger<StockManager>>();
builder.Services.AddScoped<Logger<RedisManager>>();
builder.Services.AddScoped<Logger<ProductManager>>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseCors();
app.MapControllers();
app.Run();