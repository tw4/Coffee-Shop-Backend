using System.Text.Json.Serialization;

namespace coffee_shop_backend.Entitys.Concreates;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public Stock Stock { get; set; }
}