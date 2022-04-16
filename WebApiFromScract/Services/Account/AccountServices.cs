using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiFromScract.Contract;
using WebApiFromScract.Contract.Account;

namespace WebApiFromScract.Services.Account
{
    public class AccountServices : IAccountServices
    {
        public readonly UserManager<IdentityUser> _userManger;
        public readonly SignInManager<IdentityUser> _signInManager;

        public IConfiguration Configuration { get; }
        public AccountServices(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManger = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
        }

        public async Task<AuthenticationResponse> Login(LoginDto loginDto)
        {
            var user = await _userManger.FindByEmailAsync(loginDto.email);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    ErrorMessage = new[] { $"User With email {loginDto.email} is not exist" },
                };
            }

            var passwordChecked = await _userManger.CheckPasswordAsync(user, loginDto.password);
            if (!passwordChecked)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    ErrorMessage = new[] { "User password combination is not valid" }
                };
            }

            var token = GenerateToken(user.Id);

            return new AuthenticationResponse
            {
                Success = true,
                Token = token,
            };
        }

        public async Task<AuthenticationResponse> Register(RegisterDto registerDto)
        {
            var existingUser = await _userManger.FindByEmailAsync(registerDto.email);
            if (existingUser != null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    ErrorMessage = new[]{ $"User With email {registerDto.email} is already exist" },
                };  
            }

            var newUser = new IdentityUser
            {
                UserName = registerDto.email,
                Email = registerDto.email,
            };

            var createdUser = await _userManger.CreateAsync(newUser, registerDto.password);
            if (!createdUser.Succeeded)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    ErrorMessage = new[]{ "Registration is failed, please try again!" }
                };
            }

            var user = await _userManger.FindByEmailAsync(registerDto.email);
            var token = GenerateToken(user.Id);

            return new AuthenticationResponse
            {
                Success = true,
                Token = token,
            };
        }

        public string GenerateToken(string userId)
        {
            var mySecret = Configuration["Jwt:Key"];
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
            var myIssuer = Configuration["Jwt:Issuer"];
            var myAudience = Configuration["Jwt:Audience"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
