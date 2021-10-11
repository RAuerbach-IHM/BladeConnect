using System;
using SwitchBladeInterface.API.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Services.SecurityServices
{
    public interface IAuthenticationService
    {
        Task<Token> VerifyCredentials(String username, String pw);

        Task<bool> Logout(String username);
    }
}
