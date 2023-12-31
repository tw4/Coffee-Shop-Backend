using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Dto.Auth;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Concreates;

public class AuthServices: IAuthServices
{
    private readonly CoffeeShopDbContex _coffeeShopDbContex;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<AuthServices> _logger;

    public AuthServices(CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices, Logger<AuthServices> logger)
    {
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
        _logger = logger;
    }

    public IActionResult UserLogin(LoginRequest request)
    {
        User? user = (from u in _coffeeShopDbContex.Users
            where u.Email == request.Email && u.Password == request.Password
            select u).FirstOrDefault();

        if (user == null)
        {
            _logger.LogInformation("Email or password is wrong");
            return new BadRequestObjectResult(new {message = "Email or password is wrong"});
        }
        string token = _jwtServices.GenerateJwtToken(user.Id, user.Email);
        _logger.LogInformation("User login success");
        return new OkObjectResult(new {token});
    }

    public IActionResult Auth(string token)
    {
        if (token == null)
        {
            _logger.LogInformation("Token is null Auth");
            return new BadRequestObjectResult(new {message = "Token is null"});
        }

       Boolean isTokenValid = _jwtServices.IsTokenValid(token);

       if (!isTokenValid)
       {
           _logger.LogInformation("Token is invalid Auth");
           return new UnauthorizedResult();
       }

       long userId = _jwtServices.GetUserIdFromToken(token);
       string userEmail = _jwtServices.GetUserEmailFromToken(token);

       User user = _coffeeShopDbContex.Users.FirstOrDefault(u => u.Id == userId && u.Email == userEmail);

       if (user == null)
       {
              _logger.LogInformation("User is null Auth");
              return new BadRequestObjectResult(new {message = "User is null"});
       }


       _logger.LogInformation("Token is valid Auth");
       return new OkObjectResult(new { message = "Token is valid", sucsess = isTokenValid});
    }
}