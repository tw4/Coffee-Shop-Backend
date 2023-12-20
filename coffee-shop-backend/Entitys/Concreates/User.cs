
namespace coffee_shop_backend.Entitys.Concreates;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public ICollection<Order> Orders { get; set; }
    public EnumRole Role { get; set; }
}
