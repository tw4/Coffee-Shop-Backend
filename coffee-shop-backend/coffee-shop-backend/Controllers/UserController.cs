using coffee_shop_backend.Contexs;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    public readonly CoffeeShopContex _context;

    public UserController(CoffeeShopContex context)
    {
        _context = context;
    }

    [HttpGet]
    public string Get()
    {
        return "Hello World";
    }

}