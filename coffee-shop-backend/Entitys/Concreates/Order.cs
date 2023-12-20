namespace coffee_shop_backend.Entitys.Concreates;

public class Order: BaseEntity
{
 public long UserId { get; set; }
 public long ProductId { get; set; }
 public EnumOrderStatus Status { get; set; }
 public string Email { get; set; }
 public string FullName { get; set; }
 public string Address { get; set; }
 public DateTime PaymentDate { get; set; }
 public User User { get; set; }
 public Product Product { get; set; }
}
