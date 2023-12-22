using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Business.Concreates;
using coffee_shop_backend.Dto.Auth;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Contexs;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{

    private readonly IAuthServices _authServices;
    private readonly Logger<AuthController> _logger;

    public AuthController(IAuthServices authServices, Logger<AuthController> logger)
    {
        _authServices = authServices;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        return _authServices.UserLogin(request);
    }

    [HttpGet]
    public IActionResult VerifyToken([FromHeader] string token)
    {
        return _authServices.Auth(token);
    }
}