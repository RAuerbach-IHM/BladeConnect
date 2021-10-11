using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/phonebook")]
    public class PhonebookController : ControllerBase
    {

        private readonly IPhonebookRepository _phonebookRepository;
        private readonly ITokensRepository _tokensRepository;
        private readonly IAccountsRepository _accountsRepository;

        public PhonebookController(IPhonebookRepository phonebookRepository, ITokensRepository tokensRepository, IAccountsRepository accountsRepository)
        {
            _phonebookRepository = phonebookRepository ??
                throw new ArgumentNullException(nameof(phonebookRepository));

            _tokensRepository = tokensRepository ??
               throw new ArgumentNullException(nameof(tokensRepository));

            _accountsRepository = accountsRepository ??
               throw new ArgumentNullException(nameof(accountsRepository));
        }

        /*
        [HttpGet("listings/public")]  //iOS
        public async Task<IActionResult> GetPhonebook()
        {
            try
            {
                var phonebookFromRepository = await _phonebookRepository.GetPhonebook();
                return Ok(phonebookFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Phonebook Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        */
        [HttpPost("listings/public")]
        public async Task<IActionResult> GetPhonebookPost()
        {
            try
            {
                var resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "" && resultToken != "Not Admin")
                {
                    return Ok(resultToken);
                }

                //Get Token
                long tokenId = ConvertLong(Request.Form["tokenid"]);           
                var token = await _tokensRepository.GetToken(tokenId); 

                //Verify account allows phonebook
                var accountFromRepository = await _accountsRepository.GetAccount(token.user_name);

                if(accountFromRepository.show_public_phonebook == 0)
                {
                    List<PhonebookItem> emptyPhonebook = new List<PhonebookItem>();
                    return Ok(emptyPhonebook);
                }
                var phonebookFromRepository = await _phonebookRepository.GetPhonebook();
                return Ok(phonebookFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Phonebook Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("listings/public/all")]
        public async Task<IActionResult> GetPhonebookAll()
        {
            try  //Must be admin
            {
                var resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                var phonebookFromRepository = await _phonebookRepository.GetPhonebookAll();
                return Ok(phonebookFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Phonebook Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("listings/personal")]
        public async Task<IActionResult> GetPersonalPhonebook()
        {
            try
            {
                var resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "" && resultToken != "Not Admin")
                {
                    return Ok(resultToken);
                }

                //Get AccountID From Token
                long tokenId = ConvertLong(Request.Form["tokenid"]);
                Token token = await _tokensRepository.GetToken(tokenId);

                //Verify account allows phonebook
                var accountFromRepository = await _accountsRepository.GetAccount(token.user_name);

                if (accountFromRepository.show_personal_phonebook == 0)
                {
                    List<PhonebookItem> emptyPhonebook = new List<PhonebookItem>();
                    return Ok(emptyPhonebook);
                }

                var phonebookFromRepository = await _phonebookRepository.GetPersonalPhonebook(token.account_id);
                return Ok(phonebookFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Phonebook Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("listings/personal/account")]
        public async Task<IActionResult> GetPersonalPhonebookByAccount()
        {
            var resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

            if (resultToken != "")
            {
                return Ok(resultToken);
            }

            //Verify Long
            Int64 accountId = -1;
            var result = Int64.TryParse(Request.Form["accountid"], out accountId);

            if (!result)
            {
                Console.WriteLine("Phonebook Item Not Found");
                return Ok("Not Found");
            }

            try
            {
                var phonebookFromRepository = await _phonebookRepository.GetPersonalPhonebook(accountId);
                return Ok(phonebookFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Phonebook Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("saveitem")]
        public async Task<IActionResult> SavePhonebookItem()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                var phonebookitem = Request.Form["phonebookitem"];

                bool result = Int64.TryParse(Request.Form["id"], out long phonebookItemId);

                if (!result)
                {
                    return Ok("Phonebook Item ID is invalid.");
                }

                int hideFromPublic = 0;
                if(Request.Form["hidefrompublic"] == "true")
                {
                    hideFromPublic = 1;
                }

                //Get PhonebookItem from array
                PhonebookItem phonebookItem = new PhonebookItem
                {
                    Description = Request.Form["description"],
                    Hide_From_Public = hideFromPublic,
                    ID = phonebookItemId,
                    Name = Request.Form["name"],
                    SIP_Address = Request.Form["sipaddress"]
                };

                var resultSave = await _phonebookRepository.SavePhonebookItem(phonebookItem);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Delete Phonebook Item Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("personal/save")]
        public async Task<IActionResult> SavePersonalPhonebook()
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


                string phonebookitemids = Request.Form["phonebookitemids"];

                string[] ids = phonebookitemids.Split('~');

                List<long> phonebookItemIds = new List<long>();
                foreach (string id in ids)
                {
                    long itemToAdd = ConvertLong(id);
                    if (itemToAdd > 0)
                    {
                        phonebookItemIds.Add(itemToAdd);
                    }
                }

                var resultSave = await _phonebookRepository.SavePersonalPhonebook(accountId, phonebookItemIds);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Delete Phonebook Item Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("deleteitem")]
        public async Task<IActionResult> DeletePhonebookItem()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);
                
                if(resultToken != "")
                {
                    return Ok(resultToken);
                }

                if (string.IsNullOrEmpty(Request.Form["phonebookitemid"]))
                {
                    Console.WriteLine("Phonebook Item Not Found");
                    return Ok("Not Found");
                }

                Int64 phonebookItemId = -1;
                var result = Int64.TryParse(Request.Form["phonebookitemid"], out phonebookItemId);

                if (!result)
                {
                    Console.WriteLine("Phonebook Item Not Found");
                    return Ok("Not Found");
                }

                var phonebookFromRepository = await _phonebookRepository.DeletePhonebookItem(phonebookItemId);
                return Ok(phonebookFromRepository);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Delete Phonebook Item Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        private async Task<string> VerifyAdminToken(string tokenid)
        {
            if(string.IsNullOrEmpty(tokenid))
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

    }
}
