namespace coffee_shop_backend.Dto.Order;

public class AddOrderRequest
{
    public long ProductId { get; set; }
    public string Email { get; set; }
     public string FullName { get; set; }
     public string Address { get; set; }
}