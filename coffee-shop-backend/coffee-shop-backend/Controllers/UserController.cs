using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{


    [HttpGet]
    public AcceptedResult Get()
    {
        return Accepted();
    }

}