using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Dto.User;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserServices _userServices;

    public UserController(IUserServices userServices)
    {
        _userServices = userServices;
    }

    [HttpPost]
    public IActionResult AddUser([FromBody] AddUserRequest request)
    {
        return _userServices.AddUser(request);
    }

    [HttpGet]
    public IActionResult getUserById([FromHeader] string token)
    {
        return _userServices.GetUserById(token);
    }

    [HttpDelete]
    public IActionResult DeleteUserById([FromHeader] string token)
    {
        return _userServices.DeleteUserById(token);
    }

    [HttpPatch]
    public IActionResult UpdateUserPassword([FromHeader] string token, [FromBody] UpdateUserPasswordRequest request)
    {
        return _userServices.UpdateUserPassword(token, request);
    }

    [HttpPut]
    public IActionResult UpdateBasicUserInformation([FromHeader] string token, [FromBody] UpdateBasicUserInformationRequest request)
    {
        return _userServices.UpdateBasicUserInformation(token, request);
    }
}