using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Dto.Product;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coffee_shop_backend.Business.Concreates;

public class ProductManager: IProductServices
{

    private readonly CoffeeShopContex _coffeeShopContex;
    private readonly IJwtServices _jwtServices;

    public ProductManager(CoffeeShopContex coffeeShopContex, IJwtServices jwtServices)
    {
        _coffeeShopContex = coffeeShopContex;
        _jwtServices = jwtServices;
    }

    public IActionResult AddProduct(AddProductRequest request)
    {

        Product product = new()
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
        };
        _coffeeShopContex.Products.Add(product);

        try
        {
            _coffeeShopContex.SaveChanges();
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

        var productWithStock = _coffeeShopContex.Products
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

        Product? product = _coffeeShopContex.Products.Find(id);

        if (product == null)
        {
            return new NotFoundObjectResult(new { message = "Product not found", success = false });
        }

        _coffeeShopContex.Products.Remove(product);

        try
        {
            _coffeeShopContex.SaveChanges();
            return new OkObjectResult(new { message = "Product deleted successfully", success = true });
        }
        catch (Exception e)
        {
            return new NotFoundObjectResult(new { message = e.Message, success = false });
        }
    }
}