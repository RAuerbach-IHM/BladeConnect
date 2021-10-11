using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using SwitchBladeInterface.API.Services.LocalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static SwitchBladeInterface.API.Enums.Enums;

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/macropanels")]
    public class MacroPanelsController : ControllerBase
    {
        private readonly IMacroPanelsRepository _macroPanelsRepository;
        private readonly IMacroElementsRepository _macroElementsRepository;
        private readonly IStationsRepository _stationsRepository;
        private readonly ITokensRepository _tokensRepository;
        private readonly ILocalCallService _localCallService;

        public MacroPanelsController(IMacroPanelsRepository macroPanelsRepository, IMacroElementsRepository macroElementsRepository, ITokensRepository tokensRepository, IStationsRepository stationsRepository, ILocalCallService localCallService)
        {
            _localCallService = localCallService;
            _tokensRepository = tokensRepository;
            _stationsRepository = stationsRepository ??
                throw new ArgumentNullException(nameof(macroPanelsRepository));
            _macroPanelsRepository = macroPanelsRepository ??
                throw new ArgumentNullException(nameof(macroPanelsRepository));
            _macroElementsRepository = macroElementsRepository ??
                throw new ArgumentNullException(nameof(macroElementsRepository));
        }


        [HttpGet("{stationId}")]
        public async Task<IActionResult> GetMacroPanels(long stationId)
        {
            try
            {
                var macroPanelsFromRepository = await _macroPanelsRepository.GetMacroPanels(stationId);
                return Ok(macroPanelsFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Macro Panels Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("station")]
        public async Task<IActionResult> GetMacroPanels()
        {
            try
            {
                String tokenid = "";
                var stationCallLetters = "";
                long tokenIdValue = 0;

                tokenid = Request.Form["tokenid"];
                stationCallLetters = Request.Form["callletters"];

                //Verify long
                bool result = Int64.TryParse(tokenid, out tokenIdValue);

                if (!result)
                {
                    return Ok("Error - No Token ID - Please log in.");
                }

                //Verify Token
                result = await _tokensRepository.VerifyToken(tokenIdValue);

                if (!result)
                {
                    return Ok("Error - Expired - Please log in");
                }

                //Get Station
                Station station = await _stationsRepository.GetStation(stationCallLetters);

                if (station.Id < 1)
                {
                    return Ok("Error - Station Call Letters Not Found.");
                }

                var macroPanelsFromRepository = await _macroPanelsRepository.GetMacroPanels(station.Id);

                return Ok(macroPanelsFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Macro Panels Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("stationid")]
        public async Task<IActionResult> GetMacroPanelsByStationID()
        {
            String tokenid = "";
            var stationid = "";
            long tokenIdValue = 0;
            long stationIdValue = 0;

            try
            {
                tokenid = Request.Form["tokenid"];
                stationid = Request.Form["stationid"];

                //Verify long
                bool result = Int64.TryParse(tokenid, out tokenIdValue);

                if (!result)
                {
                    return Ok("Error - No Token ID - Please log in.");
                }

                //Verify long
                result = Int64.TryParse(stationid, out stationIdValue);

                if (!result)
                {
                    return Ok("Error - No Station ID - Please log in.");
                }

                //Verify Token
                result = await _tokensRepository.VerifyToken(tokenIdValue);

                if (!result)
                {
                    return Ok("Error - Expired - Please log in");
                }

                var macroPanelsFromRepository = await _macroPanelsRepository.GetMacroPanels(stationIdValue);

                return Ok(macroPanelsFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Macro Panels Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }


        [HttpPost("call")]  //If from a panel
        public async Task<IActionResult> CallByMacroPanel()
        {
            String tokenid = "";
            String panelid = "";
            
            try
            {
                long tokenIdValue = 0;
                long panelIdValue = 0;

                tokenid = Request.Form["tokenid"];
                panelid = Request.Form["macropanelid"];

                //Verify long
                bool result = Int64.TryParse(tokenid, out tokenIdValue);
                if (!result)
                {
                    return Ok("Error - No Token ID - Please log in.");
                }

                result = Int64.TryParse(panelid, out panelIdValue);
                {
                    result = Int64.TryParse(panelid, out panelIdValue);
                }


                if (!result)
                {
                    return Ok("Error - No Panel ID.");
                }

                //Verify Token
                result = await _tokensRepository.VerifyToken(tokenIdValue);

                if (!result)
                {
                    return Ok("Error - Expired - Please log in");
                }

                Console.WriteLine("Call Request RECEIVED " + panelid);

                //Get Panel
                MacroPanel macroPanel = await _macroPanelsRepository.GetMacroPanel(panelIdValue);

                //Get Panels
                IEnumerable<MacroElement> macroElements = await _macroElementsRepository.GetMacroElements(macroPanel.ID);

                foreach (MacroElement macroElement in macroElements)
                {
                    //Place Calls
                    //Set Crosspoints For Call Elements
                    if (macroElement.Type == (int)BUTTON_TYPE.MACRO_CALL_ELEMENT)
                    {
                        if (!string.IsNullOrEmpty(macroElement.Call_To) && !string.IsNullOrEmpty(macroElement.Channel_ID))
                        {
                            string callResult = await _localCallService.Call(macroElement.Device_ID, macroElement.Channel, macroElement.Call_To);
                            Console.WriteLine("SUCCESS " + callResult);
                        }
                        
                        String takeResult;

                        //Source
                        if (!string.IsNullOrEmpty(macroElement.Audio_Receive_From_Source) && !string.IsNullOrEmpty(macroElement.Channel_ID))
                        {
                            takeResult = await _localCallService.Take( macroElement.Audio_Receive_From_Source, macroElement.Channel_ID.ToString());// (macroElement.Device_ID, macroElement.Channel_ID, macroElement.Call_To);
                            Console.WriteLine("CALL TAKE SOURCE SUCCESS " + macroElement.Audio_Receive_From_Source + " " + macroElement.Channel_ID);
                            Console.WriteLine(takeResult);
                        }

                        //Destination
                        if (!string.IsNullOrEmpty(macroElement.Audio_Send_To_Dest) && !string.IsNullOrEmpty(macroElement.Channel_ID))
                        {
                            takeResult = await _localCallService.Take(macroElement.Channel_ID, macroElement.Audio_Send_To_Dest);
                            Console.WriteLine("CALL TAKE DEST SUCCESS " + macroElement.Channel_ID + " " + macroElement.Channel_ID);
                            Console.WriteLine(takeResult);
                        }
                    }
                    //Set Crosspoints For XY Elements
                    if (macroElement.Type == (int)BUTTON_TYPE.ROUTE_XY_ELEMENT || macroElement.Type == (int)BUTTON_TYPE.ROUTE_X_ELEMENT)
                    {
                        String takeResult;
                        Console.WriteLine("XY CROSSPOINT CHANGE SUCCESS " + macroElement.Audio_Receive_From_Source + " " + macroElement.Audio_Send_To_Dest);

                        //Source To Destination
                        if (!string.IsNullOrEmpty(macroElement.Audio_Receive_From_Source) && !string.IsNullOrEmpty(macroElement.Audio_Send_To_Dest))
                        {
                            takeResult = await _localCallService.Take(macroElement.Audio_Receive_From_Source, macroElement.Audio_Send_To_Dest );// (macroElement.Device_ID, macroElement.Channel_ID, macroElement.Call_To);
                            Console.WriteLine(takeResult);
                        }
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Macro Panel Call Action Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }


        [HttpPost("hangup")]  //If from a panel
        public async Task<IActionResult> HangupByMacroPanel()
        {
            String tokenid = "";
            String panelid = "";

            try
            {
                long tokenIdValue = 0;
                long panelIdValue = 0;

                tokenid = Request.Form["tokenid"];
                panelid = Request.Form["macropanelid"];

                //Verify long
                bool result = Int64.TryParse(tokenid, out tokenIdValue);
                if (!result)
                {
                    return Ok("Error - No Token ID - Please log in.");
                }

                result = Int64.TryParse(panelid, out panelIdValue);
                {
                    result = Int64.TryParse(panelid, out panelIdValue);
                }


                if (!result)
                {
                    return Ok("Error - No Panel ID.");
                }

                //Verify Token
                result = await _tokensRepository.VerifyToken(tokenIdValue);

                if (!result)
                {
                    return Ok("Error - Expired - Please log in");
                }

                Console.WriteLine("Hangup Request RECEIVED " + panelid);

                //Get Panel
                MacroPanel macroPanel = await _macroPanelsRepository.GetMacroPanel(panelIdValue);

                //Get Panels
                IEnumerable<MacroElement> macroElements = await _macroElementsRepository.GetMacroElements(macroPanel.ID);

                foreach (MacroElement macroElement in macroElements)
                {
                    //Place Call Disconnect
                    if (!string.IsNullOrEmpty(macroElement.Channel_ID))
                    {
                        string callResult = await _localCallService.Call(macroElement.Device_ID, macroElement.Channel, "~");
                        Console.WriteLine("SUCCESS " + callResult);
                    } 
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Macro Panel Hangup Action Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        /*
        [HttpGet("call/{panelID}")]
        public async Task<IActionResult> Call(long panelID)
        {
            try
            {
                String result = await _localCallService.Call(panelID);

                return Ok(result + " - Panel" + panelID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Call Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }

        }
        */

        /*
        [HttpPost("call")]  //If NOT from a panel
        public async Task<IActionResult> Call()
        {
            String tokenid = "";
            var deviceid = "";
            var channel = "";
            var callTo = "";

            try
            {
                long tokenIdValue = 0;
                long deviceIdValue = 0;


                tokenid = Request.Form["tokenid"];
                deviceid = Request.Form["deviceid"];
                channel = Request.Form["channel"];
                callTo = Request.Form["callto"];

                //Verify long
                bool result = Int64.TryParse(tokenid, out tokenIdValue);
                if (result)
                {
                    result = Int64.TryParse(deviceid, out deviceIdValue);
                }


                if (!result)
                {
                    return Ok("Error - No Token ID - Please log in.");
                }

                //Verify Token
                result = await _tokensRepository.VerifyToken(tokenIdValue);

                if (!result)
                {
                    return Ok("Error - Expired - Please log in");
                }

                Console.WriteLine("Call Request RECEIVED " + deviceid, " ", channel, " ", callTo);


                String callResult = await _localCallService.Call(deviceIdValue, channel, callTo);

                return Ok(result + " - Device: " + deviceIdValue + " Channel: " + channel + " Call To: " + callTo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Call Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        */

        /*
        [HttpPost("crosspointchange")]
        public async Task<IActionResult> Take()
        {
            String tokenid = "";
            var source = "";
            var destination = "";

            try
            {
                long tokenIdValue = 0;

                tokenid = Request.Form["tokenid"];
                source = Request.Form["source"];
                destination = Request.Form["destination"];

                //Verify long
                bool result = Int64.TryParse(tokenid, out tokenIdValue);

                if (!result)
                {
                    return Ok("Error - No Token ID - Please log in.");
                }

                //Verify Token
                result = await _tokensRepository.VerifyToken(tokenIdValue);

                if (!result)
                {
                    return Ok("Error - Expired - Please log in");
                }
                //Send Take
                String takeResult = await _localCallService.Take(source, destination);

                return Ok(result + " - Source: " + source + " Dest: " + destination);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Take Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        */

        [HttpPost("save")]
        public async Task<IActionResult> SaveMacroPanel()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                }

                long macroPanelIDValue = ConvertLong(Request.Form["id"]);

                if (macroPanelIDValue < 0)
                {
                    Console.WriteLine("Macro Panel ID Not Valid");
                    return new StatusCodeResult((int)HttpStatusCode.BadRequest);
                }

                //Get Macropanel from array
                MacroPanel macroPanel = new MacroPanel
                {
                    ID = macroPanelIDValue,

                    Color = Request.Form["color"],
                    End_Time = ConvertDateTime(Request.Form["endtime"]),
                    Index_Value = ConvertInt(Request.Form["index"]),
                    Name = Request.Form["name"],
                    Start_Time = ConvertDateTime(Request.Form["starttime"]),
                    Station_Id = ConvertLong(Request.Form["stationid"]),
                    Description = Request.Form["description"]
                };

                var resultSave = await _macroPanelsRepository.SaveMacroPanel (macroPanel);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Macro Panel Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteMacroPanel()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);
                }

                var macroPanelId = ConvertLong(Request.Form["macropanelid"]);
                if (macroPanelId < 0)
                {
                    Console.WriteLine("Macro Panel Not Found");
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);
                }

                var resultDelete = await _macroPanelsRepository.DeleteMacroPanel(macroPanelId);
                return Ok(resultDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Delete Maco Panel Request: " + ex);
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
        private DateTime ConvertDateTime(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return DateTime.Now;
            }

            Int64 output = -1;
            var result = Int64.TryParse(input, out output);

            if (!result)
            {
                return DateTime.Now;
            }

            DateTime dt = new DateTime(output);
            return dt;
        }
    }
}
