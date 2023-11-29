using coffee_shop_backend.Dto.Product;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Abstracts;

public interface IProductServices
{
    public IActionResult AddProduct(AddProductRequest request);
    public IActionResult GetProductById(long id, string token);
    public IActionResult DeleteProductById(long id, string token);
}