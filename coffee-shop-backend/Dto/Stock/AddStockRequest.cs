namespace coffee_shop_backend.Dto.Stock;

public class AddStockRequest
{
   public long ProductId { get; set; }
   public long Amount { get; set; }
}