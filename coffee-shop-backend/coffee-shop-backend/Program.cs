using System.Text;
using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.EntityFrameworkCore;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddDbContext<CoffeeShopDbContex>(
    options => options.UseNpgsql("Host=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword"));
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

// Add scoped services
builder.Services.AddScoped<IJwtServices, JwtManager>();
builder.Services.AddScoped<IAuthServices, AuthManager>();
builder.Services.AddScoped<IUserServices, UserManager>();
builder.Services.AddScoped<IProductServices, ProductManager>();
builder.Services.AddScoped<IStockServices, StockManager>();
builder.Services.AddScoped<IOrderServices, OrderManager>();
builder.Services.AddScoped<IRedisServices, RedisManager>();
builder.Services.AddScoped<IProductElasticSearchServices<Product>, ProductElasticSearchManager<Product>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();