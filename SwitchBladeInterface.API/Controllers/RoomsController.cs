using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly ITokensRepository _tokensRepository;
        private readonly IRoomsRepository _roomsRepository;
        
        public RoomsController(ITokensRepository tokensRepository, IRoomsRepository roomsRepository)
        {
            _tokensRepository = tokensRepository ??
                throw new ArgumentNullException(nameof(tokensRepository));

            _roomsRepository = roomsRepository ??
                throw new ArgumentNullException(nameof(roomsRepository));
        }

        [HttpPost()]
        public async Task<IActionResult> GetRooms()
        {
            try
            {
                Int64 tokenId = -1;
                var result = Int64.TryParse(Request.Form["tokenid"], out tokenId);

                bool includeHidden = false;
                if (Request.Form["includeHidden"] == "True")
                    includeHidden = true;

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

                var roomsFromRepository = await _roomsRepository.GetRooms(includeHidden);


                return Ok(roomsFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Rooms Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("siteid")]
        public async Task<IActionResult> GetRoomsBySiteId()
        {
            try
            {
                Int64 tokenId = -1;
                var result = Int64.TryParse(Request.Form["tokenid"], out tokenId);

                bool includeHidden = false;
                if (Request.Form["includeHidden"] == "True")
                    includeHidden = true;

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

                var roomsFromRepository = await _roomsRepository.GetRoomsBySiteId(ConvertInt(Request.Form["siteid"]), includeHidden);


                return Ok(roomsFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Rooms Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("roomid")]
        public async Task<IActionResult> GetRoomByID()
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

                Int64 roomIdValue = ConvertLong(Request.Form["roomid"]);


                //Get Room
                var roomFromRepository = await _roomsRepository.GetRoom(roomIdValue);

                return Ok(roomFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Room Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }


        [HttpPost("save")]
        public async Task<IActionResult> SaveRoom()
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
                    Console.WriteLine("Room Not Valid");
                    return Ok("Room Not Valid");
                }

                //Verify id
                Int64 id = -1;
                var result = Int64.TryParse(Request.Form["id"], out id);

                if (!result)
                {
                    Console.WriteLine("ID Not Valid");
                    return Ok("ID Not Valid");
                }

                //Verify Type
                Int32 type = -1;
                var resultType = Int32.TryParse(Request.Form["type"], out type);

                if (!resultType)
                {
                    Console.WriteLine("Room Type Not Valid");
                    return Ok("Room Type Not Valid");
                }


                //Verify Hidden
                Int32 hidden = 0;
                var resultHidden = Int32.TryParse(Request.Form["hidden"], out hidden);

                //Verify Hidden Mic
                Int32 hidden_mic = 0;
                var resultHidden_mic = Int32.TryParse(Request.Form["hiddenmic"], out hidden_mic);


                //Get Room from array
                Room room = new Room
                {
                    ID = id,
                    Name = Request.Form["name"],
                    Type = type,
                    Designator = Request.Form["designator"],
                    Hidden = hidden,
                    Hidden_mic = hidden_mic,
                    Site_id = ConvertInt(Request.Form["siteid"]),
                    Room_id = ConvertInt(Request.Form["roomid"])

                };

                var resultSave = await _roomsRepository.SaveRoom(room);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Room Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteRoom()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                if (string.IsNullOrEmpty(Request.Form["roomid"]))
                {
                    Console.WriteLine("Room Not Found");
                    return Ok("Not Found");
                }

                Int64 roomId = -1;
                var result = Int64.TryParse(Request.Form["roomid"], out roomId);

                if (!result)
                {
                    Console.WriteLine("Room Not Found");
                    return Ok("Not Found");
                }

                var resultDelete = await _roomsRepository.DeleteRoom(roomId);
                return Ok(resultDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Delete Room Request: " + ex);
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
