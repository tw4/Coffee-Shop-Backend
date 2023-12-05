using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IProductElasticSearchServices<Product> _productElasticSearchServices;

    public SearchController(IProductElasticSearchServices<Product> productElasticSearchServices)
    {
        _productElasticSearchServices = productElasticSearchServices;
    }

    [HttpGet]
    public IActionResult Search([FromQuery]string query, [FromHeader]string token)
    {
        var result = _productElasticSearchServices.Search(query, token);
        return Ok(result.Documents);
    }

    [HttpGet("index")]
    public IActionResult IndexDataFromDatabase()
    {
        _productElasticSearchServices.IndexDataFromDatabase();
        return Ok();
    }

}