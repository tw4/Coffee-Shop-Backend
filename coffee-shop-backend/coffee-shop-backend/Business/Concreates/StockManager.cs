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
    private readonly Logger<StockManager> _logger;

    public StockManager(CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices, Logger<StockManager> logger)
    {
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
        _logger = logger;
    }

    public IActionResult AddStock(AddStockRequest request, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation($"Token is invalid Add Stock");
            return new UnauthorizedResult();
        }

        long userId = _jwtServices.GetUserIdFromToken(token);

        User? user = _coffeeShopDbContex.Users.Find(userId);

        if (user == null)
        {
            _logger.LogInformation($"User not found Add Stock");
            return new NotFoundObjectResult(new { message = "User not found", success = false });
        }

        if (user.Role != EnumRole.ADMIN)
        {
            _logger.LogWarning($"User not admin Add Stock");
            return new UnauthorizedResult();
        }

        Product? product = _coffeeShopDbContex.Products.Find(request.ProductId);

        if (product == null)
        {
            _logger.LogInformation($"Product not found Add Stock");
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
            _logger.LogInformation($"Stock added successfully Add Stock");
            return new OkObjectResult(new { message = "Stock added successfully", success = true });
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while adding stock Add Stock",e);
            return new BadRequestObjectResult(new { message = e.Message, success = false });
        }
    }

    public IActionResult UpdateStock(UpdateStockRequest request, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation($"Token is invalid Update Stock");
            return new UnauthorizedResult();
        }

        long userId = _jwtServices.GetUserIdFromToken(token);


        User? user = _coffeeShopDbContex.Users.Find(userId);

        if (user == null)
        {
            _logger.LogInformation($"User not found Update Stock");
            return new NotFoundObjectResult(new { message = "User not found", success = false });
        }

        if (user.Role != EnumRole.ADMIN)
        {
            _logger.LogWarning($"User not admin Update Stock");
            return new UnauthorizedResult();
        }

        Stock? stock = _coffeeShopDbContex.Stocks.Find(request.Id);

        if (stock == null)
        {
            _logger.LogInformation($"Stock not found Update Stock");
            return new NotFoundObjectResult(new { message = "Stock not found", success = false });
        }

       stock.Amount = request.Amount;

       _coffeeShopDbContex.Stocks.Update(stock);

       try
       {
              _coffeeShopDbContex.SaveChanges();
              _logger.LogInformation($"Stock updated successfully Update Stock");
              return new OkObjectResult(new { message = "Stock updated successfully", success = true });
       }
       catch (Exception e)
       {
              _logger.LogError($"Error while updating stock Update Stock",e);
           return new BadRequestObjectResult(new { message = e.Message, success = false });
       }
    }

    public IActionResult DeleteStock(long id, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation($"Token is invalid Delete Stock");
            return new UnauthorizedResult();
        }

        Stock? stock = _coffeeShopDbContex.Stocks.Find(id);

        if (stock == null)
        {
            _logger.LogInformation($"Stock not found Delete Stock");
            return new NotFoundObjectResult(new { message = "Stock not found", success = false });
        }

        _coffeeShopDbContex.Stocks.Remove(stock);

        try
        {
            _coffeeShopDbContex.SaveChanges();
            _logger.LogInformation($"Stock deleted successfully Delete Stock");
            return new OkObjectResult(new { message = "Stock deleted successfully", success = true });
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while deleting stock Delete Stock",e);
            return new BadRequestObjectResult(new { message = e.Message, success = false });
        }
    }

    public IActionResult GetStockById(long id, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation($"Token is invalid Get Stock By Id");
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
            _logger.LogInformation($"Stock not found Get Stock By Id");
            return new NotFoundObjectResult(new { message = "Stock not found", success = false });
        }

        _logger.LogInformation($"Stock found Get Stock By Id");
        return new OkObjectResult(new { message = "Stock found", success = true, data = stockWithProduct });
    }
}