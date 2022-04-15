using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebApiFromScract.Contract;

namespace WebApiFromScract.Controllers
{
    public class AccountController : Controller
    {
        public readonly UserManager<IdentityUser> _userManger;
        public readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManger = userManager;
        }

        [HttpPost("api/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var user = await _userManger.FindByEmailAsync(registerDto.email);
            if (user != null)
            {
                return BadRequest(new { Message = $"User with email {registerDto.email} is already exist" });
            }

            var newUser = new IdentityUser
            {
                UserName = registerDto.email,
                Email = registerDto.email,
            };

            var createdUser = await _userManger.CreateAsync(newUser, registerDto.password);
            if (!createdUser.Succeeded)
            {
                return BadRequest(new { Message = "Registration is failed, please try again!" });
            }

            return Ok(newUser);
        }
    }
}
