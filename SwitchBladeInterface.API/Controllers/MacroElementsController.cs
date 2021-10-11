using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.DTOModels;
using SwitchBladeInterface.API.Entities;
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
    [Route("api/macroelements")]
    public class MacroElementsController : ControllerBase
    {
        private readonly IMacroElementsRepository _macroElementsRepository;
        private readonly IStationsRepository _stationsRepository;
        private readonly ITokensRepository _tokensRepository;
        private readonly ILocalCallService _localCallService;

        public MacroElementsController(IMacroElementsRepository macroElementsRepository, ITokensRepository tokensRepository, IStationsRepository stationsRepository, ILocalCallService localCallService)
        {
            _localCallService = localCallService;
            _tokensRepository = tokensRepository;
            _stationsRepository = stationsRepository ??
                throw new ArgumentNullException(nameof(macroElementsRepository));
            _macroElementsRepository = macroElementsRepository ??
                throw new ArgumentNullException(nameof(macroElementsRepository));
        }

        
        [HttpPost("macropanelid")]
        public async Task<IActionResult> GetMacroElements()
        {
            try
            {
                String tokenid = "";
                var macropanelid = "";
                long tokenIdValue = 0;
                long macroPanelIdValue = 0;

                tokenid = Request.Form["tokenid"];
                macropanelid = Request.Form["macropanelid"];

                //Verify long
                bool result = Int64.TryParse(tokenid, out tokenIdValue);

                if (!result)
                {
                    return Ok("Error - No Token ID - Please log in.");
                }

                //Verify long
                result = Int64.TryParse(macropanelid, out macroPanelIdValue);

                if (!result)
                {
                    return Ok("Error - No Macro Panel ID");
                }

                //Verify Token
                result = await _tokensRepository.VerifyToken(tokenIdValue);

                if (!result)
                {
                    return Ok("Error - Expired - Please log in");
                }

                var macroElementsFromRepository = await _macroElementsRepository.GetMacroElements(macroPanelIdValue);

                List<MacroElementDTO> macroElementsDTO = new List<MacroElementDTO>();
                foreach (MacroElement macroElementFromRepository in macroElementsFromRepository)
                {
                    MacroElementDTO macroElementDTO = new MacroElementDTO
                    {
                        ID = macroElementFromRepository.ID,
                        Audio_Receive_From_Source = macroElementFromRepository.Audio_Receive_From_Source,
                        Audio_Receive_From_Source_B = macroElementFromRepository.Audio_Receive_From_Source_B,
                        Audio_Send_To_Dest = macroElementFromRepository.Audio_Send_To_Dest,
                        Call_From = macroElementFromRepository.Call_From,
                        Call_To = macroElementFromRepository.Call_To,
                        Channel = macroElementFromRepository.Channel,
                        Channel_ID = macroElementFromRepository.Channel_ID,
                        Description = macroElementFromRepository.Description,
                        Index = macroElementFromRepository.Index_Value,
                        Name = macroElementFromRepository.Name,
                        Status = macroElementFromRepository.Status,
                        Type = macroElementFromRepository.Type,
                        Device_ID = macroElementFromRepository.Device_ID,
                        Macro_Button_Id = macroElementFromRepository.Macro_Button_Id
                    };

                    macroElementsDTO.Add(macroElementDTO);
                }

                return Ok(macroElementsDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Macro Panels Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

       
        [HttpGet("call/{panelID}")]  //Not Used??
        public async Task<IActionResult> Call(long panelID)
        {
            try
            {
                String result = await _localCallService.Call(panelID, "");

                return Ok(result + " - Panel" + panelID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Call Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }

        }

        
        [HttpPost("hangup")]  //If from a panel
        public async Task<IActionResult> HangupByMacroPanel()
        {
            String tokenid = "";
            String elementid = "";

            try
            {
                long tokenIdValue = 0;
                long elementIdValue = 0;

                tokenid = Request.Form["tokenid"];
                elementid = Request.Form["macroelementid"];

                //Verify long
                bool result = Int64.TryParse(tokenid, out tokenIdValue);
                if (!result)
                {
                    return Ok("Error - No Token ID - Please log in.");
                }

                result = Int64.TryParse(elementid, out elementIdValue);
                {
                    result = Int64.TryParse(elementid, out elementIdValue);
                }


                if (!result)
                {
                    return Ok("Error - No Panel Element ID.");
                }

                //Verify Token
                result = await _tokensRepository.VerifyToken(tokenIdValue);

                if (!result)
                {
                    return Ok("Error - Expired - Please log in");
                }

                Console.WriteLine("Hangup Request RECEIVED - Macro Element" + elementid);

                //Get Element
                MacroElement macroElement = await _macroElementsRepository.GetMacroElement(elementIdValue);

                //Place Call Disconnect
                if (!string.IsNullOrEmpty(macroElement.Channel_ID))
                {
                    string callResult = await _localCallService.Call(macroElement.Device_ID, macroElement.Channel, "~");
                    Console.WriteLine("SUCCESS " + callResult);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Macro Element Hangup Action Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        [HttpPost("save")]
        public async Task<IActionResult> SaveMacroElement()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                }

                long macroElementIDValue = ConvertLong(Request.Form["id"]);

                if (macroElementIDValue < 0)
                {
                    Console.WriteLine("Macro Element ID Not Valid");
                    return new StatusCodeResult((int)HttpStatusCode.BadRequest);
                }

                
                //Get MacroElement from array
                MacroElement macroElement = new MacroElement
                {
                    ID = macroElementIDValue,

                    Audio_Receive_From_Source = Request.Form["audioreceivefromsource"],
                    Audio_Receive_From_Source_B = Request.Form["audioreceivefromsourceb"],
                    Audio_Send_To_Dest = Request.Form["audiosendtodest"],
                    Call_From = Request.Form["callfrom"],
                    Call_To = Request.Form["callto"],
                    Channel = ConvertInt(Request.Form["channel"]),
                    Channel_ID = Request.Form["channelid"],
                    Description = Request.Form["description"],
                    Device_ID = ConvertLong(Request.Form["deviceid"]),
                    Index_Value = ConvertInt(Request.Form["index"]),
                    Macro_Button_Id = ConvertLong(Request.Form["macropanelid"]),
                    Name = Request.Form["name"],
                    Type = ConvertInt(Request.Form["type"])
                };

                var resultSave = await _macroElementsRepository.SaveMacroElement(macroElement);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Macro Element Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("updateindex")]
        public async Task<IActionResult> UpdateMacroElementIndex()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                }

                long macroElementIDValue = ConvertLong(Request.Form["id"]);

                if (macroElementIDValue < 0)
                {
                    Console.WriteLine("Macro Element ID Not Valid");
                    return new StatusCodeResult((int)HttpStatusCode.BadRequest);
                }

                var resultUpdate = await _macroElementsRepository.UpdateMacroElementIndex(macroElementIDValue, ConvertInt(Request.Form["index"]));
                return Ok(resultUpdate);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Macro Element Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteMacroElement()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);
                }

                var macroElementId = ConvertLong(Request.Form["macroelementid"]);
                if (macroElementId < 0)
                {
                    Console.WriteLine("Macro Element Not Found");
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);
                }

                var resultDelete = await _macroElementsRepository.DeleteMacroElement(macroElementId);
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