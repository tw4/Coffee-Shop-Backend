using coffee_shop_backend.Dto.Stock;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Abstracts;

public interface IStockServices
{
    public IActionResult AddStock(AddStockRequest request, string token);
    public IActionResult UpdateStock(UpdateStockRequest request, string token);
    public IActionResult DeleteStock(long id, string token);
    public IActionResult GetStockById(long id, string token);
}