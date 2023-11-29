using coffee_shop_backend.Dto.Auth;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Abstracts;

public interface IAuthServices
{
    public ActionResult UserLogin(LoginRequest request);
    public ActionResult Auth(string token);

}