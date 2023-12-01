using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Dto.Stock;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coffee_shop_backend.Business.Concreates;

public class StockManager:IStockServices
{
    private readonly CoffeeShopDbContex _coffeeShopDbContex;
    private readonly IJwtServices _jwtServices;

    public StockManager(CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices)
    {
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
    }

    public IActionResult AddStock(AddStockRequest request, string token)
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

        Product? product = _coffeeShopDbContex.Products.Find(request.ProductId);

        if (product == null)
        {
            return new NotFoundObjectResult(new { message = "Product not found", success = false });
        }

        Stock stock = new Stock()
        {
           ProductId = request.ProductId,
           Amount = request.Amount,
        };

        _coffeeShopDbContex.Stocks.Add(stock);

        try
        {
            _coffeeShopDbContex.SaveChanges();
            return new OkObjectResult(new { message = "Stock added successfully", success = true });
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(new { message = e.Message, success = false });
        }
    }

    public IActionResult UpdateStock(UpdateStockRequest request, string token)
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

        Stock? stock = _coffeeShopDbContex.Stocks.Find(request.Id);

        if (stock == null)
        {
            return new NotFoundObjectResult(new { message = "Stock not found", success = false });
        }

       stock.Amount = request.Amount;

       _coffeeShopDbContex.Stocks.Update(stock);

       try
       {
              _coffeeShopDbContex.SaveChanges();
              return new OkObjectResult(new { message = "Stock updated successfully", success = true });
       }
       catch (Exception e)
       {
           return new BadRequestObjectResult(new { message = e.Message, success = false });
       }
    }

    public IActionResult DeleteStock(long id, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        Stock? stock = _coffeeShopDbContex.Stocks.Find(id);

        if (stock == null)
        {
            return new NotFoundObjectResult(new { message = "Stock not found", success = false });
        }

        _coffeeShopDbContex.Stocks.Remove(stock);

        try
        {
            _coffeeShopDbContex.SaveChanges();
            return new OkObjectResult(new { message = "Stock deleted successfully", success = true });
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(new { message = e.Message, success = false });
        }
    }

    public IActionResult GetStockById(long id, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        var stockWithProduct = _coffeeShopDbContex.Stocks
            .Include(s => s.Product)
            .Where(s => s.Id == id)
            .Select(s => new
            {
                s.Id,
                s.Amount,
                Product = new
                {
                    s.Product.Id,
                    s.Product.Name,
                    s.Product.Price,
                    s.Product.Description,
                    s.Product.ImageUrl,
                }
            });

        if (stockWithProduct == null)
        {
            return new NotFoundObjectResult(new { message = "Stock not found", success = false });
        }

        return new OkObjectResult(new { message = "Stock found", success = true, data = stockWithProduct });
    }
}