using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Repositories.Interfaces;
using SwitchBladeInterface.API.Services.LocalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/bladeio")]

    public class BladeIOController : ControllerBase
    {
        private readonly IBladeIORepository _bladeIORepository;
        private readonly ITokensRepository _tokensRepository;
        private readonly ILocalCallService _localCallService;

        public BladeIOController(IBladeIORepository bladeIORepository, ITokensRepository tokensRepository, ILocalCallService localCallService)
        {
            _localCallService = localCallService;

            _bladeIORepository = bladeIORepository ??
                throw new ArgumentNullException(nameof(bladeIORepository));

            _tokensRepository = tokensRepository ??
                throw new ArgumentNullException(nameof(tokensRepository));
        }

        /*
        [HttpGet("source/{accountid}")]  //For iOS
        public async Task<IActionResult> GetBladeIOsForSource(long accountid)
        {
            try
            {
                var sourcesFromRepository = await _bladeIORepository.GetBladeIOsByAccount("Source", accountid);
                return Ok(sourcesFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Blade IO Source Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpGet("dest/{accountid}")]  //For iOS
        public async Task<IActionResult> GetBladeIOsForDest(long accountid)
        {
            try
            {
                var destinationsFromRepository = await _bladeIORepository.GetBladeIOsByAccount("Dest", accountid);
                return Ok(destinationsFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Blade IO Destination Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        */

        [HttpPost("source/all")]
        public async Task<IActionResult> GetBladeIOsForSource()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                var bladeIOsFromRepository = await _bladeIORepository.GetBladeIOs("Source");
                return Ok(bladeIOsFromRepository);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad BladeIO Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("source/account")]
        public async Task<IActionResult> GetBladeIOsForSourceByAccount()
        {
            try
            {
                Int64 tokenId = -1;
                var result = Int64.TryParse(Request.Form["tokenid"], out tokenId);

                Int64 accountId = -1;
                var resultAccount = Int64.TryParse(Request.Form["accountid"], out accountId);

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


                var bladeIOsFromRepository = await _bladeIORepository.GetBladeIOsByAccount("Source", accountId);

                return Ok(bladeIOsFromRepository);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad BladeIO Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        [HttpPost("dest/all")]
        public async Task<IActionResult> GetBladeIOsForDest()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                var bladeIOsFromRepository = await _bladeIORepository.GetBladeIOs("Dest");
                return Ok(bladeIOsFromRepository);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad BladeIO Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("dest/account")]
        public async Task<IActionResult> GetBladeIOsForDestByAccount()
        {
            try
            {
                Int64 tokenId = -1;
                var result = Int64.TryParse(Request.Form["tokenid"], out tokenId);

                Int64 accountId = -1;
                var resultAccount = Int64.TryParse(Request.Form["accountid"], out accountId);

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


                var bladeIOsFromRepository = await _bladeIORepository.GetBladeIOsByAccount("Dest", accountId);
                return Ok(bladeIOsFromRepository);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad BladeIO Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }


        [HttpPost("switchbladechannels")]
        public async Task<IActionResult> GetBladeIOsForSwitchBladeChannels()
        {
            try
            {
                Int64 tokenId = -1;
                var result = Int64.TryParse(Request.Form["tokenid"], out tokenId);

                if (!result)
                {
                    Console.WriteLine("Token Not Found");
                    return Ok("Not Found");
                }

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
                if (token.role != 100)
                {
                    Console.WriteLine("Token Not Verified");
                    return Ok("Not Verified");
                }

                var destinationsFromRepository = await _bladeIORepository.GetBladeIOsForSwitchBladeChannels();
                return Ok(destinationsFromRepository);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Blade IO SwitchBlade Channels Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("source/personal/save")]
        public async Task<IActionResult> SavePersonalSources()
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


                string sourceids = Request.Form["sourceids"];

                string[] ids = sourceids.Split('~');

                List<long> sourceIds = new List<long>();
                foreach (string id in ids)
                {
                    long itemToAdd = ConvertLong(id);
                    if (itemToAdd > 0)
                    {
                        sourceIds.Add(itemToAdd);
                    }
                }

                var resultSave = await _bladeIORepository.SavePersonalBladeIOs("Source", accountId, sourceIds);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Personal BladeIOs Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("dest/personal/save")]
        public async Task<IActionResult> SavePersonalDestinations()
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


                string destinationids = Request.Form["destinationids"];

                string[] ids = destinationids.Split('~');

                List<long> destinationIds = new List<long>();
                foreach (string id in ids)
                {
                    long itemToAdd = ConvertLong(id);
                    if (itemToAdd > 0)
                    {
                        destinationIds.Add(itemToAdd);
                    }
                }

                var resultSave = await _bladeIORepository.SavePersonalBladeIOs("Dest", accountId, destinationIds);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Personal BladeIO Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpGet("dest/{accountid}/{destination}")]
        public async Task<IActionResult> GetBladeIOForDestByWNID(long accountid, String destination)
        {
            try
            {
                var destinationFromRepository = await _bladeIORepository.GetBladeIOByWNID("Dest", destination);
                return Ok(destinationFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Blade IO Destination Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        /*
                [HttpGet("api/sources/call/{panelID}")]
                public void Call(long panelID)
                {
                    _localCallService.Call(panelID);

                    //var panelsFromRepository = _panelsRepository.GetPanels(stationId);
                    //return Ok(panelsFromRepository);
                }
        */
        //[HttpGet("{panelId}")]
        //public IActionResult GetPanel(Guid authorId)
        //{
        //    var panelFromRepository = _panelsRepository.GetPanel(authorId);

        //    if (panelFromRepository == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(panelFromRepository);
        //}

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
    }
}
