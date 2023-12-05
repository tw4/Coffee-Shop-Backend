using Microsoft.AspNetCore.Mvc;
using Nest;

namespace coffee_shop_backend.Business.Abstracts;

public interface IProductElasticSearchServices<T> where T : class
{
    public IActionResult Search(string query, string token);
    public void IndexDataFromDatabase();

}