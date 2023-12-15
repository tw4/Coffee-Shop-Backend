using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Dto.User;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Concreates;

public class UserServices : IUserServices
{
    private readonly CoffeeShopDbContex _coffeeShopDbContex;
    private readonly IJwtServices _jwtServices;
    private readonly Logger<UserServices> _logger;

    public UserServices(CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices, Logger<UserServices> logger)
    {
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
        _logger = logger;
    }

    public IActionResult AddUser(AddUserRequest request)
    {

        User u = _coffeeShopDbContex.Users.Where(u => u.Email == request.Email).FirstOrDefault();

        if (u != null)
        {
            _logger.LogInformation($"User already exists. Add user");
            return new BadRequestObjectResult(new { message = "User already exists", success = false });
        }

        User user = new User() { };
        user.Name = request.Name;
        user.Surname = request.Surname;
        user.Email = request.Email;
        user.Password = request.Password;
        user.Role = EnumRole.USER;
        _coffeeShopDbContex.Users.Add(user);
        try
        {
            _coffeeShopDbContex.SaveChanges();
            _logger.LogInformation("User added successfully");
            return new OkObjectResult(new { message = "User added successfully", success = true });
        }
        catch (Exception e)
        {
            _logger.LogError($"User added failed.",e);
            return new BadRequestObjectResult( new { message = e.Message});
        }
    }

    public IActionResult GetUserById(string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation($"Token is not valid. Get user by id");
            return new UnauthorizedResult();
        }

        long Id = _jwtServices.GetUserIdFromToken(token);
        User? user = (from u in _coffeeShopDbContex.Users where u.Id == Id select u).FirstOrDefault();

        if (user == null )
        {
            _logger.LogInformation($"User not found. Get user by id");
            return new BadRequestObjectResult(new { message = "User not found", success = false });
        }
        return new OkObjectResult(new { message = "User found", success = true, user = user });
    }

    public IActionResult DeleteUserById(string token)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation($"Token is not valid. Delete user by id");
            return new UnauthorizedResult();
        }

        long Id = _jwtServices.GetUserIdFromToken(token);
        User? user = (from u in _coffeeShopDbContex.Users where u.Id == Id select u).FirstOrDefault();

        if (user == null)
        {
            _logger.LogInformation($"User not found. Delete user by id");
            return new BadRequestObjectResult(new { message = "User not found", success = false });
        }

        _coffeeShopDbContex.Users.Remove(user);

        try
        {
            _coffeeShopDbContex.SaveChanges();
            _logger.LogInformation($"User deleted successfully. Delete user by id");
            return new OkObjectResult(new { message = "User deleted successfully", success = true });
        }
        catch (Exception e)
        {
            _logger.LogError($"User deleted failed.",e);
            return new BadRequestObjectResult(new { message = e.Message });
        }
    }

    public IActionResult UpdateUserPassword(string token, UpdateUserPasswordRequest request)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation($"Token is not valid. Update user password");
            return new UnauthorizedResult();
        }

        long Id = _jwtServices.GetUserIdFromToken(token);

        User? user = (from u in _coffeeShopDbContex.Users where u.Id == Id select u).FirstOrDefault();

        if (user == null)
        {
            _logger.LogInformation($"User not found. Update user password");
            return new BadRequestObjectResult(new { message = "User not found", success = false });
        }

        user.Password = request.Password;
        _coffeeShopDbContex.Users.Update(user);

        try
        {
            _coffeeShopDbContex.SaveChanges();
            _logger.LogInformation($"User password updated successfully. Update user password");
            return new OkObjectResult(new { message = "User password updated successfully", success = true });
        }
        catch (Exception e)
        {
            _logger.LogError($"User password updated failed.",e);
            return new BadRequestObjectResult(new { message = e.Message });
        }
    }

    public IActionResult UpdateBasicUserInformation(string token, UpdateBasicUserInformationRequest request)
    {
        if (!_jwtServices.IsTokenValid(token))
        {
            _logger.LogInformation($"Token is not valid. Update user information");
            return new UnauthorizedResult();
        }

        long Id = _jwtServices.GetUserIdFromToken(token);
        User? user = (from u in _coffeeShopDbContex.Users where u.Id == Id select u).FirstOrDefault();

        if (user == null)
        {
            _logger.LogInformation($"User not found. Update user information");
            return new BadRequestObjectResult(new { message = "User not found", success = false });
        }

        user.Name = request.Name;
        user.Surname = request.Surname;
        user.Email = request.Email;
        _coffeeShopDbContex.Users.Update(user);

        try
        {
            _coffeeShopDbContex.SaveChanges();
            _logger.LogInformation($"User information updated successfully. Update user information");
            return new OkObjectResult(new { message = "User information updated successfully", success = true });
        }
        catch (Exception e)
        {
            _logger.LogError($"User information updated failed.",e);
            return new BadRequestObjectResult(new { message = e.Message });
        }
    }
}