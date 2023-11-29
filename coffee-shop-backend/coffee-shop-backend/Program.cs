using System.Text;
using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Contexs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddDbContext<CoffeeShopContex>(
    options => options.UseNpgsql("Host=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add scoped services
builder.Services.AddScoped<IJwtServices, JwtManager>();
builder.Services.AddScoped<IAuthServices, AuthManager>();
builder.Services.AddScoped<IUserServices, UserManager>();
builder.Services.AddScoped<IProductServices, ProductManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();