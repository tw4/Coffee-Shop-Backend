using coffee_shop_backend.Dto.Auth;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Abstracts;

public interface IAuthServices
{
    public IActionResult UserLogin(LoginRequest request);
    public IActionResult Auth(string token);

}