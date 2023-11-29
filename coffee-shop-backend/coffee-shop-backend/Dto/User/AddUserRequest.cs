using coffee_shop_backend.Entitys.Concreates;

namespace coffee_shop_backend.Dto.User;

public class AddUserRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}