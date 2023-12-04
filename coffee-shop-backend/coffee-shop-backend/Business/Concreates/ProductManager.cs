using System.Text.Json;
using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Dto.Product;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coffee_shop_backend.Business.Concreates;

public class ProductManager: IProductServices
{

    private readonly CoffeeShopDbContex _coffeeShopDbContex;
    private readonly IJwtServices _jwtServices;
    private readonly IRedisServices _redisServices;

    public ProductManager(CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices, IRedisServices redisServices)
    {
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
        _redisServices = redisServices;
    }

    public IActionResult AddProduct(AddProductRequest request, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        long userId = _jwtServices.GetUserIdFromToken(token);

        User? user = _coffeeShopDbContex.Users.Find(userId);

        if (user == null)
        {
            return new NotFoundObjectResult(new { message = "User not found", success = false });
        }

        if (user.Role != EnumRole.ADMIN)
        {
            return new UnauthorizedResult();
        }

        Product product = new()
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
        };
        _coffeeShopDbContex.Products.Add(product);

        try
        {
            _coffeeShopDbContex.SaveChanges();
            return new OkObjectResult(new { message = "Product added successfully", success = true });
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(new { message = e.Message, success = false });
        }
    }

    public IActionResult GetProductById(long id, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        var productWithStock = _coffeeShopDbContex.Products
            .Include(p => p.Stock)
            .Where(p => p.Id == id)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Description,
                p.ImageUrl,
                p.Stock
            })
            .FirstOrDefault();

        if (productWithStock == null)
        {
            return new NotFoundObjectResult(new { message = "Product not found", success = false });
        }
        return new OkObjectResult(new { message = "Product found", success = true, data = productWithStock });
    }

    public IActionResult DeleteProductById(long id, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        long userId = _jwtServices.GetUserIdFromToken(token);

        User? user = _coffeeShopDbContex.Users.Find(userId);

        if (user == null)
        {
            return new NotFoundObjectResult(new { message = "User not found", success = false });
        }

        if (user.Role != EnumRole.ADMIN)
        {
            return new UnauthorizedResult();
        }

        Product? product = _coffeeShopDbContex.Products.Find(id);

        if (product == null)
        {
            return new NotFoundObjectResult(new { message = "Product not found", success = false });
        }

        _coffeeShopDbContex.Products.Remove(product);

        try
        {
            _coffeeShopDbContex.SaveChanges();
            return new OkObjectResult(new { message = "Product deleted successfully", success = true });
        }
        catch (Exception e)
        {
            return new NotFoundObjectResult(new { message = e.Message, success = false });
        }
    }

    public IActionResult GetProductsByPage(int page,string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        var products = _coffeeShopDbContex.Products
            .Include(p => p.Stock)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Description,
                p.ImageUrl,
                p.Stock
            })
            .Skip(page * 10)
            .Take(10).ToList();

        if (products.Count == 0)
        {
            return new NotFoundObjectResult(new { message = "Products not found", success = false });
        }

        return new OkObjectResult(new { message = "Products found", success = true, data = products });
    }

    public IActionResult GetAllProducts(string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        if (_redisServices.GetValue("products") != null)
        {
            var products = JsonSerializer.Deserialize<List<object>>(_redisServices.GetValue("products")!);
            return new OkObjectResult(new { message = "Products found", success = true, data = products });
        }
        else
        {
            var products = _coffeeShopDbContex.Products
                .Include(p => p.Stock)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Description,
                    p.ImageUrl,
                    p.Stock
                })
                .ToList();

            if (products.Count == 0)
            {
                return new NotFoundObjectResult(new {message = "Products not found", success = false});
            }

            _redisServices.SetValue("products", JsonSerializer.Serialize(products), TimeSpan.FromMinutes(60));
            return new OkObjectResult(new {message = "Products found", success = true, data = products});
        }
    }
}