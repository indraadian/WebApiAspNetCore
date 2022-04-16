using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiFromScract.Contract;
using WebApiFromScract.Contract.Account;

namespace WebApiFromScract.Services.Account
{
    public interface IAccountServices
    {
        Task<AuthenticationResponse> Register(RegisterDto registerDto);
        Task<AuthenticationResponse> Login(LoginDto loginDto);
    }
}
