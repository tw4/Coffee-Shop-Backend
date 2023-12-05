using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace coffee_shop_backend.Business.Concreates;

public class ProductElasticSearchManager<T>: IProductElasticSearchServices<T> where T : class
{

    private readonly ElasticClient _client;
    private readonly CoffeeShopDbContex _coffeeShopDbContex;
    private readonly IJwtServices _jwtServices;


    public ProductElasticSearchManager(ElasticClient client, CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices)
    {
        var settings = new ConnectionSettings( new Uri("http://localhost:9200"))
            .DefaultIndex("coffee-shop");
        _client = new ElasticClient(settings);
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
    }

    public IActionResult Search(string query, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            return new UnauthorizedResult();
        }

        var result = _client.Search<Product>(s => s
            .Query(q => q
                .MatchPhrase(m => m
                    .Field(f => f.Name)
                    .Query(query)
                )
            )
        );

        return new OkObjectResult(new {message = "Data Found", success = true , data = result.Documents});
    }

    public void IndexDataFromDatabase()
    {
        var products = _coffeeShopDbContex.Products.ToList();
        var response = _client.IndexMany(products);
    }
}