using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/service")]
    public class ServiceSettingsController : ControllerBase
    {
        private readonly ITokensRepository _tokensRepository;
        private readonly IServiceSettingsRepository _serviceSettingsRepository;
        public ServiceSettingsController(ITokensRepository tokensRepository, IServiceSettingsRepository serviceSettingsRepository)
        {
            _tokensRepository = tokensRepository ??
                throw new ArgumentNullException(nameof(tokensRepository));

            _serviceSettingsRepository = serviceSettingsRepository ??
                throw new ArgumentNullException(nameof(serviceSettingsRepository));
        }

        /*
        [HttpGet()]
        public async Task<IActionResult> GetServiceSettings(long tokenId)
        {
            try
            {
                var serviceSettingsFromRepository = await _serviceSettingsRepository.GetServiceSettings();
                return Ok(serviceSettingsFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Service Settings Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        */
        
        [HttpPost()]
        public async Task<IActionResult> GetServiceSettingsPost()
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

                var serviceSettingsFromRepository = await _serviceSettingsRepository.GetServiceSettings();
                return Ok(serviceSettingsFromRepository);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Service Settings Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        [HttpPost("save")]
        public async Task<IActionResult> SaveServiceSettings()
        {
            Console.WriteLine("SERVICE SETTINGS REQUEST");
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                Int32 portValue = -1;
                var result = Int32.TryParse(Request.Form["port"], out portValue);

                if (!result)
                {
                    Console.WriteLine("Port Not Valid");
                    return Ok("Port Not Valid");
                }

                //Get Service Settings from array
                ServiceSettings serviceSettings = new ServiceSettings
                {
                    id = 0,
                    city = Request.Form["city"],
                    state = Request.Form["state"],
                    market = Request.Form["market"],
                    ip_address = Request.Form["ip"],
                    port = portValue,
                    description = Request.Form["description"],
                    public_Register_Url = Request.Form["public_register_url"]
                    //version = Request.Form["version"],
                    //build = Request.Form["build"],
                };

                var resultSave = await _serviceSettingsRepository.SaveServiceSettings(serviceSettings);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Service Settings Request: " + ex);
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
    }
}



