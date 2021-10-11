using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using SwitchBladeInterface.API.Services.LocalServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/displayevents")]
    public class DisplayEventsController : ControllerBase
    {
        private readonly ITokensRepository _tokensRepository;
        private readonly IDisplayEventsRepository _displayEventsRepository;
        private readonly IRoomsRepository _roomsRepository;

        private readonly ILocalDisplayEventTakeService _localDisplayEventTakeService;

        public DisplayEventsController(ITokensRepository tokensRepository, IRoomsRepository roomsRepository, IDisplayEventsRepository displayEventsRepository, ILocalDisplayEventTakeService localDisplayEventTakeService)
        {
            _tokensRepository = tokensRepository ??
                throw new ArgumentNullException(nameof(tokensRepository));

            _roomsRepository = roomsRepository ??
                throw new ArgumentNullException(nameof(roomsRepository));

            _displayEventsRepository = displayEventsRepository ??
                throw new ArgumentNullException(nameof(displayEventsRepository));

            _localDisplayEventTakeService = localDisplayEventTakeService ??
                throw new ArgumentNullException(nameof(LocalDisplayEventTakeService));
        }

        [HttpPost()]
        public async Task<IActionResult> GetDisplayEvents()
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

                bool includeHidden = false;
                if (Request.Form["includeHidden"] == "True")
                    includeHidden = true;

                var displayEventsFromRepository = await _displayEventsRepository.GetDisplayEvents(includeHidden);
                return Ok(displayEventsFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Display Events Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("siteid")]
        public async Task<IActionResult> GetDisplayEventsBySiteId()
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

                bool includeHidden = false;
                if (Request.Form["includeHidden"] == "True")
                    includeHidden = true;

                var displayEventsFromRepository = await _displayEventsRepository.GetDisplayEventsBySiteId(ConvertInt(Request.Form["siteid"]), includeHidden);
                return Ok(displayEventsFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Display Events Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("displayeventid")]
        public async Task<IActionResult> GetDisplayEventByID()
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

                Int64 displayEventIdValue = ConvertLong(Request.Form["displayeventid"]);


                //Get Room
                var displayEventFromRepository = await _displayEventsRepository.GetDisplayEvent(displayEventIdValue);

                return Ok(displayEventFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Room Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }


        [HttpPost("save")]
        public async Task<IActionResult> SaveDisplayEvent()
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
                    Console.WriteLine("Display Event Not Valid");
                    return Ok("Display Event Not Valid");
                }

                //Verify id
                Int64 id = -1;
                var result = Int64.TryParse(Request.Form["id"], out id);

                if (!result)
                {
                    Console.WriteLine("Display Event ID Not Valid");
                    return Ok("Display Event ID Not Valid");
                }

                //Verify Hidden
                Int32 hidden = 0;
                var resultHidden = Int32.TryParse(Request.Form["hidden"], out hidden);

                
                //Get Display Event from array
                DisplayEvent displayEvent = new DisplayEvent
                {
                    Id = id,
                    Display_command = Request.Form["displayeventcommand"],
                    Label = Request.Form["label"],
                    Hidden = hidden,
                    Site_id = ConvertInt(Request.Form["siteid"]),
                    Display_Event_id = ConvertInt(Request.Form["displayeventid"])
                };

                var resultSave = await _displayEventsRepository.SaveDisplayEvent(displayEvent);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Display Event Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteDisplayEvent()
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
                    Console.WriteLine("Display Event Not Found");
                    return Ok("Not Found");
                }

                Int64 displayEventId = -1;
                var result = Int64.TryParse(Request.Form["id"], out displayEventId);

                if (!result)
                {
                    Console.WriteLine("Display Event Not Found");
                    return Ok("Not Found");
                }

                var resultDelete = await _displayEventsRepository.DeleteDisplayEvent(ConvertLong(Request.Form["id"]));
                return Ok(resultDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Delete Display Event Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        /*
        [HttpPost("take/display")]
        public async Task<IActionResult> Take()
        {
            String tokenid = "";
            var deviceid = "";
            var displayeventid = "";
            
            try
            {
                long tokenIdValue = 0;
                long deviceIdValue = 0;
                int displayEventIdValue = 0;


                tokenid = Request.Form["tokenid"];
                deviceid = Request.Form["deviceid"];
                displayeventid = Request.Form["displayeventid"];

                //Verify long
                bool result = Int64.TryParse(tokenid, out tokenIdValue);
                
               
                if (!result)
                {
                    return Ok("Error - No Token ID - Please log in.");
                }

                if (result)
                {
                    result = Int64.TryParse(deviceid, out deviceIdValue);
                }

                if (!result)
                {
                    return Ok("Error - No device Id ID.");
                }

                //Verify display event
                if (result)
                {
                    result = Int32.TryParse(displayeventid, out displayEventIdValue);
                }

                if (!result)
                {
                    return Ok("Error - No DisplayEvent ID.");
                }



                //Verify Token
                result = await _tokensRepository.VerifyToken(tokenIdValue);

                if (!result)
                {
                    return Ok("Error - Expired - Please log in");
                }
                

                Console.WriteLine("Display Event Take Request RECEIVED " + deviceid + " " + displayeventid);

                string takeResult = await _localDisplayEventTakeService.Take(ConvertLong(deviceid), ConvertInt(displayeventid));

                return Ok(result + " - Device: " + deviceIdValue + " Display Event: " + displayEventIdValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Display Event Take Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        
    */
        [HttpPost("take")]
        public async Task<IActionResult> Take()
        {
            String tokenid = "";
            var roomid = "";
            var displayeventid = "";
            var option = "";

            try
            {
                long tokenIdValue = 0;
                long roomIdValue = 0;
                long displayEventIdValue = 0;
                int optionValue = 0;


                tokenid = Request.Form["tokenid"];
                roomid = Request.Form["roomid"];
                displayeventid = Request.Form["displayeventid"];
                option = Request.Form["option"];

                //Verify long
                bool result = Int64.TryParse(tokenid, out tokenIdValue);

                if (!result)
                {
                    return Ok("Error - No Token ID - Please log in.");
                }

                //Verify Room ID
                if (result)
                {
                    result = Int64.TryParse(roomid, out roomIdValue);
                }


                if (!result)
                {
                    return Ok("Error - No Room ID.");
                }

                //Verify display event
                if (result)
                {
                    result = Int64.TryParse(displayeventid, out displayEventIdValue);
                }

                //Verify option
                if (result)
                {
                    result = Int32.TryParse(option, out optionValue);
                }

                if (!result)
                {
                    optionValue = 0;
                }

                //Verify Token
                result = await _tokensRepository.VerifyToken(tokenIdValue);

                if (!result)
                {
                    return Ok("Error - Expired - Please log in");
                }

                Console.WriteLine("Call Request RECEIVED " + roomid, " ");

                string takeResult = await _localDisplayEventTakeService.Take(roomIdValue, displayEventIdValue, optionValue);

                return Ok(result + " - Room: " + roomIdValue + " Display Event: " + displayEventIdValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Call Request: " + ex);
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
