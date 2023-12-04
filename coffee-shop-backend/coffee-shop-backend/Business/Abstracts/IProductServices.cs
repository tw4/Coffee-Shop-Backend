using coffee_shop_backend.Dto.Product;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Abstracts;

public interface IProductServices
{
    public IActionResult AddProduct(AddProductRequest request, string token);
    public IActionResult GetProductById(long id, string token);
    public IActionResult DeleteProductById(long id, string token);
    public IActionResult GetProductsByPage(int page, string token);
    public IActionResult GetAllProducts(string token);
}