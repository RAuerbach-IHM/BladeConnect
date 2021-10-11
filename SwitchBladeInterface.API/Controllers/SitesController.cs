using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/sites")]
    public class SitesController : ControllerBase
    {
        private readonly ITokensRepository _tokensRepository;
        private readonly ISitesRepository _sitesRepository;

        public SitesController(ITokensRepository tokensRepository, ISitesRepository sitesRepository)
        {
            _tokensRepository = tokensRepository ??
                throw new ArgumentNullException(nameof(tokensRepository));

            _sitesRepository = sitesRepository ??
                throw new ArgumentNullException(nameof(sitesRepository));
        }

        [HttpPost()]
        public async Task<IActionResult> GetSites()
        {
            try
            {
                Int64 tokenId = -1;
                var test = Request.Form["tokenid"];
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

                var sitesFromRepository = await _sitesRepository.GetSites();


                return Ok(sitesFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Sites Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("siteid")]
        public async Task<IActionResult> GetSiteByID()
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

                Int32 siteIdValue = ConvertInt(Request.Form["siteid"]);


                //Get Site
                var siteFromRepository = await _sitesRepository.GetSite(siteIdValue);

                return Ok(siteFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Site Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("ipaddress")]
        public async Task<IActionResult> GetSiteByIPAddress()
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

                //Get Site
                var sitesFromRepository = await _sitesRepository.GetSites(Request.Form["ipaddress"]);

                return Ok(sitesFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Site Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveSite()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                //Verify id exists
                if (string.IsNullOrEmpty(Request.Form["id"]))
                {
                    Console.WriteLine("Site Not Valid");
                    return Ok("Site Not Valid");
                }

                //Verify id
                Int32 siteId = -1;
                var result = Int32.TryParse(Request.Form["id"], out siteId);

                if (!result)
                {
                    Console.WriteLine("Site ID Not Valid");
                    return Ok("Site ID Not Valid");
                }

                

                //Get Site from array
                Site site = new Site
                {
                    ID = siteId,
                    Market = Request.Form["market"],
                    Ip_Range_Low = Request.Form["iprangelow"],
                    Ip_Range_High = Request.Form["iprangehigh"],
                    Site_Code = Request.Form["sitecode"],
                    State = Request.Form["state"],
                    City = Request.Form["city"],
                };

                var resultSave = await _sitesRepository.SaveSite(site);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Site Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteSite()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                if (string.IsNullOrEmpty(Request.Form["siteid"]))
                {
                    Console.WriteLine("Site Not Found");
                    return Ok("Not Found");
                }

                Int32 siteId = -1;
                var result = Int32.TryParse(Request.Form["siteid"], out siteId);

                if (!result)
                {
                    Console.WriteLine("Site Not Found");
                    return Ok("Not Found");
                }

                var resultDelete = await _sitesRepository.DeleteSite(siteId);
                return Ok(resultDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Delete Site Request: " + ex);
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