namespace coffee_shop_backend.Dto.Product;

public class AddProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public long Price { get; set; }
    public string ImageUrl { get; set; }
}