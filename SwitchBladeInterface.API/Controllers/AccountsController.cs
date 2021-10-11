using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using SwitchBladeInterface.API.Services.SecurityServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly ITokensRepository _tokensRepository;
        private readonly IAccountsRepository _accountsRepository;
        public AccountsController(ITokensRepository tokensRepository, IAccountsRepository accountsRepository)
        {
            _tokensRepository = tokensRepository ??
                throw new ArgumentNullException(nameof(tokensRepository));

            _accountsRepository = accountsRepository ??
                throw new ArgumentNullException(nameof(accountsRepository));
        }

        
        [HttpPost()]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                Int64 tokenId = -1;
                var result = Int64.TryParse(Request.Form["tokenid"], out tokenId);

                //Get Token
                var token = await _tokensRepository.GetToken(tokenId);

                if (token.expiration < DateTime.Now.Ticks)
                {
                    Console.WriteLine("Token Expired");
                    return Ok("Expired");
                }
                if (token.id < 1)
                {
                    Console.WriteLine("Token Not Found");
                    return Ok("Not Found");
                }
                //Verify Admin
                if(token.role != 100)
                {
                    Console.WriteLine("Token Invalid");
                    return Ok("Token Not Found");
                }

                var accountsFromRepository = await _accountsRepository.GetAccounts();
                return Ok(accountsFromRepository);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Accounts Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetPassword()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                if (string.IsNullOrEmpty(Request.Form["password"]))
                {
                    Console.WriteLine("Password Not Valid");
                    return Ok("Password Not Valid");
                }

                long accountId = ConvertLong(Request.Form["accountid"]);
                                if (accountId < 0)
                {
                    Console.WriteLine("Account Not Valid");
                    return Ok("Account Not Valid");
                }

                string password = Request.Form["password"];

                //Verify Password
                PasswordHashService hash = new PasswordHashService(password);
                byte[] hashBytes = hash.ToArray();
                
                //Get Account from array
                Account account = new Account
                {
                    id = accountId,
                    password_required = 1,
                    password = hashBytes
                };

                var resultSave = await _accountsRepository.SetAccountPassword(account);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Account Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveAccount()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                if (string.IsNullOrEmpty(Request.Form["accountid"]))
                {
                    Console.WriteLine("Account Not Valid");
                    return Ok("Account Not Valid");
                }
                Int64 accountId = -1;
                var result = Int64.TryParse(Request.Form["accountid"], out accountId);

                if (!result)
                {
                    Console.WriteLine("Account Not Valid");
                    return Ok("Account Not Valid");
                }

                int showPublicPhonebook = 0;
                if (Request.Form["showpublicphonebook"] == "true")
                {
                    showPublicPhonebook = 1;
                }
                int showPersonalPhonebook = 0;
                if (Request.Form["showpersonalphonebook"] == "true")
                {
                    showPersonalPhonebook = 1;
                }
                int passwordRequired = 0;
                if (Request.Form["passwordrequired"] == "true")
                {
                    passwordRequired = 1;
                }
                int roleValue = ConvertInt(Request.Form["role"]);

                if(roleValue < 0)
                {
                    roleValue = 0;
                }

                byte[] hashBytes = new byte[0];
                if(!string.IsNullOrEmpty(Request.Form["password"]))
                {
                    //Verify Password
                    PasswordHashService hash = new PasswordHashService(Request.Form["password"]);
                    hashBytes = hash.ToArray();
                }

                //Get Account from array
                Account account = new Account
                {
                    id = accountId,
                    first_name = Request.Form["firstname"],
                    last_name = Request.Form["lastname"],
                    user_name = Request.Form["username"],
                    password = hashBytes,
                    password_required = passwordRequired,
                    show_public_phonebook = showPublicPhonebook,
                    show_personal_phonebook = showPersonalPhonebook,
                    role = roleValue
                };

                var resultSave = await _accountsRepository.SaveAccount(account);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Account Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("stationsrelations/save")]
        public async Task<IActionResult> SaveAccountStationsRelations()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                var accountId = ConvertLong(Request.Form["accountid"]);
                if (accountId == -1)
                {
                    return Ok("Account ID is invalid.");
                }


                string relationids = Request.Form["relationids"];

                string[] ids = relationids.Split('~');

                List<long> relationIds = new List<long>();
                foreach (string id in ids)
                {
                    long itemToAdd = ConvertLong(id);
                    if (itemToAdd > 0)
                    {
                        relationIds.Add(itemToAdd);
                    }
                }

                var resultSave = await _accountsRepository.SaveAccountStationsRelations(accountId, relationIds);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Delete Account Stations Relations Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAccount()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                if(string.IsNullOrEmpty(Request.Form["accountid"]))
                {
                    Console.WriteLine("Account Not Found");
                    return Ok("Not Found");
                }

                Int64 accountId = -1;
                var result = Int64.TryParse(Request.Form["accountid"], out accountId);

                if (!result)
                {
                    Console.WriteLine("Account Not Found");
                    return Ok("Not Found");
                }

                var accountsFromRepository = await _accountsRepository.DeleteAccount(accountId);
                return Ok(accountsFromRepository);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Delete Account Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        private async Task<string> VerifyAdminToken(string tokenid)
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

            if (token.expiration < DateTime.Now.Ticks)
            {
                Console.WriteLine("Token Expired");
                return "Expired";
            }
            if (token.id < 1)
            {
                Console.WriteLine("Token Not Found");
                return "Not Found";
            }
            if (token.role != 100)
            {
                Console.WriteLine("Token Not Verified");
                return "Not Admin";
            }

            return "";
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
        private int ConvertInt(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return -1;
            }

            Int32 output = -1;
            var result = Int32.TryParse(input, out output);

            if (!result)
            {
                return -1;
            }

            return output;
        }
    }
}