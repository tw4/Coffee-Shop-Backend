using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Dto.Product;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController: ControllerBase
{
    private readonly IProductServices _productServices;

    public ProductController(IProductServices productServices)
    {
        _productServices = productServices;
    }

    [HttpPost]
    public IActionResult AddProduct([FromBody]AddProductRequest request, [FromHeader] string token)
    {
        return _productServices.Add(request, token);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById([FromRoute]long id, [FromHeader]string token)
    {
        return _productServices.GetById(id, token);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProductById([FromRoute]long id, [FromHeader]string token)
    {
        return _productServices.DeleteById(id, token);
    }

    [HttpGet("page/{page}")]
    public IActionResult GetProductsByPage([FromRoute]int page, [FromHeader]string token)
    {
        return _productServices.GetProductsByPage(page, token);
    }

    [HttpGet]
    public IActionResult GetAllProducts([FromHeader]string token)
    {
        return _productServices.GetAllProducts(token);
    }

}