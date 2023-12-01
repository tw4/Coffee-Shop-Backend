using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Dto.Auth;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Concreates;

public class AuthManager: IAuthServices
{
    private readonly CoffeeShopDbContex _coffeeShopDbContex;
    private readonly IJwtServices _jwtServices;

    public AuthManager(CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices)
    {
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
    }

    public IActionResult UserLogin(LoginRequest request)
    {
        User? user = (from u in _coffeeShopDbContex.Users
            where u.Email == request.Email && u.Password == request.Password
            select u).FirstOrDefault();

        if (user == null)
        {
            return new BadRequestObjectResult(new {message = "Email or password is wrong"});
        }
        string token = _jwtServices.GenerateJwtToken(user.Id, user.Email);
        return new OkObjectResult(new {token});
    }

    public IActionResult Auth(string token)
    {
        if (token == null)
        {
            return new BadRequestObjectResult(new {message = "Token is null"});
        }

       Boolean isTokenValid = _jwtServices.IsTokenValid(token);

       if (!isTokenValid)
       {
           return new UnauthorizedResult();
       }
       return new OkObjectResult(new { message = "Token is valid", sucsess = isTokenValid});
    }
}