using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/stations")]
    public class StationsController : ControllerBase
    {
        private readonly ITokensRepository _tokensRepository;
        private readonly IStationsRepository _stationsRepository;
        public StationsController(ITokensRepository tokensRepository, IStationsRepository stationsRepository)
        {
            _tokensRepository = tokensRepository ??
                throw new ArgumentNullException(nameof(tokensRepository));

            _stationsRepository = stationsRepository ??
                throw new ArgumentNullException(nameof(stationsRepository));
        }

        [HttpGet("{accountId}")]  //For iOS and Web
        public async Task<IActionResult> GetStations(long accountId)
        {
            try
            {
                var stationsFromRepository = await _stationsRepository.GetStationsByAccount(accountId);
                return Ok(stationsFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Stations Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("all")]
        public async Task<IActionResult> GetStations()
        {
            try
            {
                Int64 tokenId = -1;
                var result = Int64.TryParse(Request.Form["tokenid"], out tokenId);

                string resultAdmin = await VerifyAdminToken(Request.Form["tokenid"]);

                if(resultAdmin != "")
                {
                    return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
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


                var stationsFromRepository = await _stationsRepository.GetStations();
                return Ok(stationsFromRepository);
            
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Bad Stations Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("account")]
        public async Task<IActionResult> GetStationsByAccount()
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


                var stationsFromRepository = await _stationsRepository.GetStationsByAccount(accountId);
                return Ok(stationsFromRepository);
            
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Bad Stations Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        [HttpPost("save")]
        public async Task<IActionResult> SaveStation()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                if (string.IsNullOrEmpty(Request.Form["id"]))
                {
                    Console.WriteLine("Account Not Valid");
                    return Ok("Account Not Valid");
                }
                Int64 stationId = -1;
                var result = Int64.TryParse(Request.Form["id"], out stationId);

                if (!result)
                {
                    Console.WriteLine("Station ID Not Valid");
                    return Ok("Station ID Not Valid");
                }

                //Get Station from array
                Station station = new Station
                {
                    Id = stationId,
                    Call_Letters = Request.Form["callletters"],
                    Name = Request.Form["name"],
                    Market = Request.Form["market"],
                    Frequency = Request.Form["frequency"],
                    Band = Request.Form["band"]
                };

                var resultSave = await _stationsRepository.SaveStation(station);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Station Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteStation()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                if (string.IsNullOrEmpty(Request.Form["stationid"]))
                {
                    Console.WriteLine("Station Not Found");
                    return Ok("Not Found");
                }

                Int64 stationId = -1;
                var result = Int64.TryParse(Request.Form["stationid"], out stationId);

                if (!result)
                {
                    Console.WriteLine("Station Not Found");
                    return Ok("Not Found");
                }

                var resultDelete = await _stationsRepository.DeleteStation(stationId);
                return Ok(resultDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Delete Station Request: " + ex);
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
