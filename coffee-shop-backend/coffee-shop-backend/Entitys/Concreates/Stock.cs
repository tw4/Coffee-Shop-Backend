namespace coffee_shop_backend.Entitys.Concreates;

public class Stock: BaseEntity
{
   public long ProductId { get; set; }
    public Product Product { get; set; }
    public long Amount { get; set; }
}