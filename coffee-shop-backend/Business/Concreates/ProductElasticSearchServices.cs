using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace coffee_shop_backend.Business.Concreates;

public class ProductElasticSearchServices<T>: IProductElasticSearchServices<T> where T : class
{

    private readonly ElasticClient _client;
    private readonly CoffeeShopDbContex _coffeeShopDbContex;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<ProductElasticSearchServices<Product>> _logger;


    public ProductElasticSearchServices(ElasticClient client, CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices, Logger<ProductElasticSearchServices<Product>> logger)
    {
        var settings = new ConnectionSettings( new Uri("http://localhost:9200"))
            .DefaultIndex("coffee-shop");
        _client = new ElasticClient(settings);
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
        _logger = logger;
    }

    public IActionResult Search(string query, string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation("Token is not valid. search");
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

        _logger.LogInformation("Search is successfull");
        return new OkObjectResult(new {message = "Data Found", success = true , data = result.Documents});
    }

    public void IndexDataFromDatabase()
    {
        var products = _coffeeShopDbContex.Products.ToList();
        var response = _client.IndexMany(products);
        if (response.Errors)
        {
            foreach (var item in response.ItemsWithErrors)
            {
                _logger.LogError("Indexing Error", item.Error);
            }
        }
        else
        {
            _logger.LogInformation("Indexing is successfull");
        }
    }
}