using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Models;
using SwitchBladeInterface.API.Repositories.Interfaces;
using SwitchBladeInterface.API.Services.SecurityServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokensRepository _tokensRepository;
        public AuthenticationController(IAuthenticationService authenticationService, ITokensRepository tokensRepository)
        {
            _authenticationService = authenticationService ??
                throw new ArgumentNullException(nameof(authenticationService));

            _tokensRepository = tokensRepository ??
                throw new ArgumentNullException(nameof(tokensRepository));
        }

        [HttpPost("request")]
        public async Task<IActionResult> GetToken()
        {
            try
            {
                var username = "";
                var password = "";
                username = Request.Form["username"];
                password = Request.Form["password"];


                Console.WriteLine("RECEIVED Token Request");

                var tokenFromRepository = await _authenticationService.VerifyCredentials(username, password);
                return Ok(tokenFromRepository);
            } catch(Exception ex)
            {
                Console.WriteLine("Bad Token Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var username = "";

                long tokenId = ConvertLong(Request.Form["tokenid"]);

                string resultVerify = await VerifyToken(Request.Form["tokenid"]);

                username = Request.Form["username"];

                //Console.WriteLine("RECEIVED Logout Request");

                if (string.IsNullOrEmpty(username) || resultVerify != "" || tokenId < 1)
                {
                    return Ok(false);
                }

                bool result = await _authenticationService.Logout(username);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Token Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        private long ConvertLong(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return -1;
            }

            Int64 output = -1;
            var result = Int64.TryParse(input, out output);

            if (!result)
            {
                return -1;
            }

            return output;
        }
        private async Task<string> VerifyToken(string tokenid)
        {
            if (string.IsNullOrEmpty(tokenid))
            {
                Console.WriteLine("Token Not Found");
                return "Not Found";
            }
            Int64 tokenId = -1;
            var result = Int64.TryParse(tokenid, out tokenId);

            if (!result)
            {
                Console.WriteLine("Token Not Found");
                return "Not Found";
            }

            //Get Token
            var token = await _tokensRepository.GetToken(tokenId);

            /*
            if (token.expiration < DateTime.Now.Ticks)
            {
                Console.WriteLine("Token Expired");
                return "Expired";
            }
            ?*/

            if (token.id < 1)
            {
                Console.WriteLine("Token Not Found");
                return "Not Found";
            }
            /*
            if (token.role != 100)
            {
                Console.WriteLine("Token Not Verified");
                return "Not Admin";
            }
            */
            return "";
        }
    }
}
