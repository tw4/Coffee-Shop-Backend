using coffee_shop_backend.Dto.Product;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Abstracts;

public interface IProductServices: ICrudServices<AddProductRequest, UpdateProductRequest>
{
    public IActionResult GetProductsByPage(int page, string token);
    public IActionResult GetAllProducts(string token);
}