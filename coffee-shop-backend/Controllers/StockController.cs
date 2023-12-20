using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Dto.Stock;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockController: ControllerBase
{
    private readonly IStockServices _stockServices;

    public StockController(IStockServices stockServices)
    {
        _stockServices = stockServices;
    }

    [HttpPost]
    public IActionResult AddStock([FromBody]AddStockRequest request, [FromHeader]string token)
    {
        return _stockServices.AddStock(request, token);
    }

    [HttpPatch]
    public IActionResult UpdateStock([FromBody]UpdateStockRequest request, [FromHeader]string token)
    {
        return _stockServices.UpdateStock(request, token);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteStock([FromRoute]long id, [FromHeader]string token)
    {
        return _stockServices.DeleteStock(id, token);
    }

    [HttpGet("{id}")]
    public IActionResult GetStockById([FromRoute]long id, [FromHeader]string token)
    {
        return _stockServices.GetStockById(id, token);
    }


}