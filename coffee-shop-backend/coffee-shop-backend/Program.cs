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
// Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
// builder.Host.UseSerilog(((ctx, lc) => lc
//     .ReadFrom.Configuration(ctx.Configuration)));

// connection string for azure sql edge
builder.Services.AddDbContext<CoffeeShopDbContex>(options =>
    options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));

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
builder.Services.AddScoped<IJwtServices, JwtServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IStockServices, StockServices>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IRedisServices, RedisServices>();
builder.Services.AddScoped<IProductElasticSearchServices<Product>, ProductElasticSearchServices<Product>>();
builder.Services.AddScoped<IPaymentServices,PaymentServices>();
builder.Services.AddScoped<Logger<UserServices>>();
builder.Services.AddScoped<Logger<StockServices>>();
builder.Services.AddScoped<Logger<RedisServices>>();
builder.Services.AddScoped<Logger<ProductServices>>();
builder.Services.AddScoped<Logger<ProductElasticSearchServices<Product>>>();
builder.Services.AddScoped<Logger<PaymentServices>>();
builder.Services.AddScoped<Logger<OrderServices>>();
builder.Services.AddScoped<Logger<JwtServices>>();
builder.Services.AddScoped<Logger<AuthServices>>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseSerilogRequestLogging();
app.UseCors();
app.MapControllers();
app.Run();