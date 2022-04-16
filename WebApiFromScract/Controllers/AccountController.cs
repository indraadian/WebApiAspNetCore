using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebApiFromScract.Contract;
using WebApiFromScract.Services.Account;

namespace WebApiFromScract.Controllers
{
    public class AccountController : Controller
    {
        public readonly IAccountServices _accountService;
        
        public AccountController(
            IAccountServices accountServices)
        {
            _accountService = accountServices;
        }

        [HttpPost("api/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            return Ok(await _accountService.Register(registerDto));
        }

        [HttpPost("api/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            return Ok(await _accountService.Login(loginDto));
        }
    }
}
