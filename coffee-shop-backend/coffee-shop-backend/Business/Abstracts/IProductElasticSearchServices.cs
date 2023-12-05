using Microsoft.AspNetCore.Mvc;
using Nest;

namespace coffee_shop_backend.Business.Abstracts;

public interface IProductElasticSearchServices<T> where T : class
{
    public ISearchResponse<T> Search(string query, string token);
    public void IndexDataFromDatabase();

}