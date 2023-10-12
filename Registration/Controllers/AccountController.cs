using System.Threading.Tasks;
using MetanApi.Registration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserService _userService;

    public AccountController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.RegisterAsync(model);

            if (result.Succeeded)
            {
                return Ok("Registration successful");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        return BadRequest("Invalid model data");
    }
}
