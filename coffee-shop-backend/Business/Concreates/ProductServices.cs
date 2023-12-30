using System.Text.Json;
using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Dto.Product;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coffee_shop_backend.Business.Concreates;

public class ProductServices: IProductServices
{

    private readonly CoffeeShopDbContex _coffeeShopDbContex;
    private readonly IJwtServices _jwtServices;
    private readonly IRedisServices _redisServices;
    private readonly Logger<ProductServices> _logger;

    public ProductServices(CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices, IRedisServices redisServices, Logger<ProductServices> logger)
    {
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
        _redisServices = redisServices;
        _logger = logger;
    }

    public IActionResult Add(AddProductRequest request, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation("Token is not valid add product");
            return new UnauthorizedResult();
        }

        long userId = _jwtServices.GetUserIdFromToken(token);

        User? user = _coffeeShopDbContex.Users.Find(userId);

        if (user == null)
        {
            _logger.LogInformation($"User not found Add Product");
            return new NotFoundObjectResult(new { message = "User not found", success = false });
        }

        if (user.Role != EnumRole.ADMIN)
        {
            _logger.LogWarning($"User not admin Add Product");
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
            _logger.LogInformation($"Product added successfully");
            return new OkObjectResult(new { message = "Product added successfully", success = true });
        }
        catch (Exception e)
        {
            _logger.LogError($"Product not added",e);
            return new BadRequestObjectResult(new { message = e.Message, success = false });
        }
    }

    // TODO: Implement this method
    public IActionResult Update(UpdateProductRequest request, string token)
    {
        throw new NotImplementedException();
    }

    public IActionResult GetById(long id, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation("Token is not valid get product by id");
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
            _logger.LogInformation($"Product not found get product by id");
            return new NotFoundObjectResult(new { message = "Product not found", success = false });
        }

        _logger.LogInformation($"Product found get product by id");
        return new OkObjectResult(new { message = "Product found", success = true, data = productWithStock });
    }

    public IActionResult DeleteById(long id, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation("Token is not valid delete product by id");
            return new UnauthorizedResult();
        }

        long userId = _jwtServices.GetUserIdFromToken(token);

        User? user = _coffeeShopDbContex.Users.Find(userId);

        if (user == null)
        {
            _logger.LogInformation($"User not found delete product by id");
            return new NotFoundObjectResult(new { message = "User not found", success = false });
        }

        if (user.Role != EnumRole.ADMIN)
        {
            _logger.LogWarning($"User not admin delete product by id");
            return new UnauthorizedResult();
        }

        Product? product = _coffeeShopDbContex.Products.Find(id);

        if (product == null)
        {
            _logger.LogInformation($"Product not found delete product by id");
            return new NotFoundObjectResult(new { message = "Product not found", success = false });
        }

        _coffeeShopDbContex.Products.Remove(product);

        try
        {
            _coffeeShopDbContex.SaveChanges();
            _logger.LogInformation($"Product deleted successfully");
            return new OkObjectResult(new { message = "Product deleted successfully", success = true });
        }
        catch (Exception e)
        {
            _logger.LogError($"Product not deleted",e);
            return new NotFoundObjectResult(new { message = e.Message, success = false });
        }
    }

    public IActionResult GetProductsByPage(int page,string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation("Token is not valid get products by page");
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
            _logger.LogInformation($"Products not found get products by page");
            return new NotFoundObjectResult(new { message = "Products not found", success = false });
        }

        _logger.LogInformation($"Products found get products by page");
        return new OkObjectResult(new { message = "Products found", success = true, data = products });
    }

    public IActionResult GetAllProducts(string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation("Token is not valid get all products");
            return new UnauthorizedResult();
        }

        if (_redisServices.GetValue("products") != null)
        {
            _logger.LogInformation($"Products found get all products from redis");
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
                _logger.LogInformation($"Products not found get all products from db");
                return new NotFoundObjectResult(new {message = "Products not found", success = false});
            }

            _redisServices.SetValue("products", JsonSerializer.Serialize(products), TimeSpan.FromMinutes(60));
            _logger.LogInformation($"Products found get all products from db");
            return new OkObjectResult(new {message = "Products found", success = true, data = products});
        }
    }
}