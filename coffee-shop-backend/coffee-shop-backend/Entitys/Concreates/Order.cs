namespace coffee_shop_backend.Entitys.Concreates;

public class Order: BaseEntity
{
 public long UserId { get; set; }
 public long ProductId { get; set; }
 public EnumOrderStatus Status { get; set; }
 public User User { get; set; }
 public Product Product { get; set; }
}
