using System.ComponentModel.DataAnnotations.Schema;

namespace coffee_shop_backend.Entitys.Concreates;

public class Stock: BaseEntity
{
    [ForeignKey("Product")]
   public long ProductId { get; set; }
   public long Amount { get; set; }
   public Product Product { get; set; }
}