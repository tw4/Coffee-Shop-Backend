using coffee_shop_backend.Dto.User;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Abstracts;

public interface IUserServices
{
    public IActionResult AddUser(AddUserRequest request);
    public IActionResult GetUserById(string token);
    public IActionResult DeleteUserById(string token);

    public IActionResult UpdateUserPassword(string token, UpdateUserPasswordRequest request);

    public IActionResult UpdateBasicUserInformation(string token, UpdateBasicUserInformationRequest request);
}