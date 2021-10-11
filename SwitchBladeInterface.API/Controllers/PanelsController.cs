using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.DTOModels;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using SwitchBladeInterface.API.Services.LocalServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static SwitchBladeInterface.API.Enums.Enums;

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/panels")]
    public class PanelsController : ControllerBase
    {
        private readonly IPanelsRepository _panelsRepository;
        private readonly IStationsRepository _stationsRepository;
        private readonly ITokensRepository _tokensRepository;
        private readonly ILocalCallService _localCallService;
        private readonly IBladeIORepository _bladeIORepository;

        public PanelsController(IPanelsRepository panelsRepository, ITokensRepository tokensRepository, IStationsRepository stationsRepository, ILocalCallService localCallService, IBladeIORepository bladeIORepository)
        {
            _localCallService = localCallService;
            _tokensRepository = tokensRepository;
            _stationsRepository = stationsRepository ??
                throw new ArgumentNullException(nameof(panelsRepository));
            _panelsRepository = panelsRepository ??
                throw new ArgumentNullException(nameof(panelsRepository));

            _bladeIORepository = bladeIORepository ??
                throw new ArgumentNullException(nameof(bladeIORepository));
        }

        
        [HttpPost("station")]
        public async Task<IActionResult> GetPanels()
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

                var panelsFromRepository = await _panelsRepository.GetPanels(station.Id);

                

                return Ok(panelsFromRepository);
            }
            catch(Exception ex) {
                Console.WriteLine("Bad Panels Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("stationid")]
        public async Task<IActionResult> GetPanelsByStationID()
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

                var panelsFromRepository = await _panelsRepository.GetPanels(stationIdValue);

                List<PanelDTO> panelsDTO = new List<PanelDTO>();
                foreach (Panel panelFromRepository in panelsFromRepository)
                {
                    PanelDTO panelDTO = new PanelDTO();
                    panelDTO.ID = panelFromRepository.ID;
                    panelDTO.Audio_Receive_From_Source = panelFromRepository.Audio_Receive_From_Source;
                    panelDTO.Audio_Receive_From_Source_B = panelFromRepository.Audio_Receive_From_Source_B;
                    panelDTO.Audio_Send_To_Dest = panelFromRepository.Audio_Send_To_Dest;
                    panelDTO.Call_From = panelFromRepository.Call_From;
                    panelDTO.Call_To = panelFromRepository.Call_To;
                    panelDTO.Channel = panelFromRepository.Channel;
                    panelDTO.Channel_ID = panelFromRepository.Channel_ID;
                    panelDTO.Color = panelFromRepository.Color;
                    panelDTO.Description = panelFromRepository.Description;
                    panelDTO.End_Time = panelFromRepository.End_Time;
                    panelDTO.Index = panelFromRepository.Index_Value;
                    panelDTO.Name = panelFromRepository.Name;
                    panelDTO.Start_Time = panelFromRepository.Start_Time;
                    panelDTO.Station_ID = panelFromRepository.Station_ID;
                    panelDTO.Status = panelFromRepository.Status;
                    panelDTO.Type = panelFromRepository.Type;
                    panelDTO.WNIP_Device_ID = panelFromRepository.WNIP_Device_ID;

                    //Get Source for Dest and Dest Display
                    if (!string.IsNullOrEmpty(panelDTO.Audio_Send_To_Dest))
                    {
                        //Get AccountID
                        Token token = await _tokensRepository.GetToken(tokenIdValue);

                        var destination = await _bladeIORepository.GetBladeIOByWNID("Dest", panelDTO.Audio_Send_To_Dest);
                        if (destination != null)
                        {
                            panelDTO.Source_For_Dest = destination.Source;

                            //Get Destination Displays
                            panelDTO.Audio_Send_To_Dest_Display = String.Concat(destination.Name, " (", panelDTO.Source_For_Dest, ")");

                        }
                        else
                        {
                            panelDTO.Source_For_Dest = "";
                            panelDTO.Audio_Send_To_Dest_Display = "";
                        }
                    } else
                    {
                        panelDTO.Audio_Send_To_Dest = "";
                        panelDTO.Audio_Send_To_Dest_Display = "";
                    }

                    //Get Source Display
                    if (!string.IsNullOrEmpty(panelDTO.Audio_Receive_From_Source))
                    {
                        var source = await _bladeIORepository.GetBladeIOByWNID("Source", panelDTO.Audio_Receive_From_Source);

                        if (source != null)
                        {
                            panelDTO.Audio_Receive_From_Source_Display = String.Concat(source.Name, " (", panelDTO.Audio_Receive_From_Source, ")");
                        }
                        else
                        {
                            panelDTO.Audio_Receive_From_Source_Display = panelDTO.Audio_Receive_From_Source;
                        }
                    }
                    else
                    {
                        panelDTO.Audio_Receive_From_Source_Display = "";
                    }

                    //Get Source B Display
                    if (!string.IsNullOrEmpty(panelDTO.Audio_Receive_From_Source_B))
                    {
                        var source_B = await _bladeIORepository.GetBladeIOByWNID("Source", panelDTO.Audio_Receive_From_Source_B);
                        if (source_B != null)
                        {
                            panelDTO.Audio_Receive_From_Source_B_Display = String.Concat(source_B.Name, " (", panelDTO.Audio_Receive_From_Source_B, ")");
                        }
                        else
                        {
                            panelDTO.Audio_Receive_From_Source_B_Display = panelDTO.Audio_Receive_From_Source_B;
                        }
                    }
                    else
                    {
                        panelDTO.Audio_Receive_From_Source_B_Display = "";
                    }

                    //Get Source For Dest Display
                    if (!string.IsNullOrEmpty(panelDTO.Source_For_Dest))
                    {
                        var sourceForDest = await _bladeIORepository.GetBladeIOByWNID("Source", panelDTO.Source_For_Dest);

                        if (sourceForDest != null)
                        {
                            panelDTO.Source_For_Dest_Display = String.Concat(sourceForDest.Name, " (", panelDTO.Source_For_Dest, ")");
                        }
                        else
                        {
                            panelDTO.Source_For_Dest_Display = panelDTO.Source_For_Dest;
                        }
                        
                    }
                    else
                    {
                        panelDTO.Source_For_Dest_Display = "";
                    }
                    
                    panelsDTO.Add(panelDTO);
                }

                return Ok(panelsDTO);

            } catch(Exception ex)
            {
                Console.WriteLine("Bad Panels Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("panelid")]
        public async Task<IActionResult> GetPanelsByPanelID()
        {
            String tokenid = "";
            var panelid = "";
            long tokenIdValue = 0;
            long panelIdValue = 0;

            try
            {
                tokenid = Request.Form["tokenid"];
                panelid = Request.Form["panelid"];

                //Verify long
                bool result = Int64.TryParse(tokenid, out tokenIdValue);

                if (!result)
                {
                    return Ok("Error - No Token ID - Please log in.");
                }

                panelIdValue = ConvertLong(panelid);
                
                if (panelIdValue < 1)
                {
                    return Ok("Error - No Panel ID - Please log in.");
                }

                //Verify Token
                result = await _tokensRepository.VerifyToken(tokenIdValue);

                if (!result)
                {
                    return Ok("Error - Expired - Please log in");
                }

                List<Panel> panelsFromRepository = new List<Panel>();
                var panel = await _panelsRepository.GetPanel(panelIdValue);

                panelsFromRepository.Add(panel);

                List<PanelDTO> panelsDTO = new List<PanelDTO>();
                foreach (Panel panelFromRepository in panelsFromRepository)
                {
                    PanelDTO panelDTO = new PanelDTO();
                    panelDTO.ID = panelFromRepository.ID;
                    panelDTO.Audio_Receive_From_Source = panelFromRepository.Audio_Receive_From_Source;
                    panelDTO.Audio_Receive_From_Source_B = panelFromRepository.Audio_Receive_From_Source_B;
                    panelDTO.Audio_Send_To_Dest = panelFromRepository.Audio_Send_To_Dest;
                    panelDTO.Call_From = panelFromRepository.Call_From;
                    panelDTO.Call_To = panelFromRepository.Call_To;
                    panelDTO.Channel = panelFromRepository.Channel;
                    panelDTO.Channel_ID = panelFromRepository.Channel_ID;
                    panelDTO.Color = panelFromRepository.Color;
                    panelDTO.Description = panelFromRepository.Description;
                    panelDTO.End_Time = panelFromRepository.End_Time;
                    panelDTO.Index = panelFromRepository.Index_Value;
                    panelDTO.Name = panelFromRepository.Name;
                    panelDTO.Start_Time = panelFromRepository.Start_Time;
                    panelDTO.Station_ID = panelFromRepository.Station_ID;
                    panelDTO.Status = panelFromRepository.Status;
                    panelDTO.Type = panelFromRepository.Type;
                    panelDTO.WNIP_Device_ID = panelFromRepository.WNIP_Device_ID;

                    //Get Source for Dest and Dest Display
                    if (!string.IsNullOrEmpty(panelDTO.Audio_Send_To_Dest))
                    {
                        //Get AccountID
                        Token token = await _tokensRepository.GetToken(tokenIdValue);

                        var destination = await _bladeIORepository.GetBladeIOByWNID("Dest", panelDTO.Audio_Send_To_Dest);
                        if (destination != null)
                        {
                            panelDTO.Source_For_Dest = destination.Source;

                            //Get Destination Displays
                            if (destination != null)
                            {
                                panelDTO.Audio_Send_To_Dest_Display = String.Concat(destination.Name, " (", panelDTO.Audio_Send_To_Dest, ")");
                            }
                            else
                            {
                                panelDTO.Audio_Send_To_Dest_Display = panelDTO.Audio_Send_To_Dest;
                            }

                        }
                        else
                        {
                            panelDTO.Source_For_Dest = "";
                            panelDTO.Audio_Send_To_Dest_Display = "";
                        }
                    }
                    else
                    {
                        panelDTO.Audio_Send_To_Dest = "";
                        panelDTO.Audio_Send_To_Dest_Display = "";
                    }

                    //Get Source Display
                    if (!string.IsNullOrEmpty(panelDTO.Audio_Receive_From_Source))
                    {
                        var source = await _bladeIORepository.GetBladeIOByWNID("Source", panelDTO.Audio_Receive_From_Source);
                        
                        if (source != null)
                        {
                            panelDTO.Audio_Receive_From_Source_Display = String.Concat(source.Name, " (", panelDTO.Audio_Receive_From_Source, ")");
                        }
                        else
                        {
                            panelDTO.Audio_Receive_From_Source_Display = panelDTO.Audio_Receive_From_Source;
                        }
                        
                    }
                    else
                    {
                        panelDTO.Audio_Receive_From_Source_Display = "";
                    }

                    //Get Source B Display
                    if (!string.IsNullOrEmpty(panelDTO.Audio_Receive_From_Source_B))
                    {
                        var source_B = await _bladeIORepository.GetBladeIOByWNID("Source", panelDTO.Audio_Receive_From_Source_B);
                        
                        if (source_B != null)
                        {
                            panelDTO.Audio_Receive_From_Source_B_Display = String.Concat(source_B.Name, " (", panelDTO.Audio_Receive_From_Source_B, ")");
                        }
                        else
                        {
                            panelDTO.Audio_Receive_From_Source_B_Display = panelDTO.Audio_Receive_From_Source_B;
                        }
                    }
                    else
                    {
                        panelDTO.Audio_Receive_From_Source_B_Display = "";
                    }

                    //Get Source For Dest Display
                    if (!string.IsNullOrEmpty(panelDTO.Source_For_Dest))
                    {
                        var sourceForDest = await _bladeIORepository.GetBladeIOByWNID("Source", panelDTO.Source_For_Dest);
                        
                        if (sourceForDest != null)
                        {
                            panelDTO.Source_For_Dest_Display = String.Concat(sourceForDest.Name, " (", panelDTO.Source_For_Dest, ")");
                        }
                        else
                        {
                            panelDTO.Source_For_Dest_Display = panelDTO.Source_For_Dest;
                        }

                    }
                    else
                    {
                        panelDTO.Source_For_Dest_Display = "";
                    }

                    panelsDTO.Add(panelDTO);
                }

                return Ok(panelsDTO);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Panel Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SavePanel()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")  //Must be Admin
                {
                    return Ok(resultToken);
                }

                Int64 panelId = ConvertLong(Request.Form["id"]);
                
              
                //Get Panel from array
                Panel panel = new Panel
                {
                    ID = panelId,
                    Audio_Receive_From_Source = Request.Form["audioreceivefromsource"],
                    Audio_Receive_From_Source_B = Request.Form["audioreceivefromsourceb"],
                    Audio_Send_To_Dest = Request.Form["audiosendtodest"],
                    Call_From = Request.Form["callfrom"],
                    Call_To = Request.Form["callto"],
                    Channel = ConvertInt(Request.Form["channel"]),
                    Channel_ID = Request.Form["channelid"],
                    Color= Request.Form["color"],
                    Description = Request.Form["description"],
                    WNIP_Device_ID = ConvertLong(Request.Form["deviceid"]),
                    End_Time = ConvertDateTime(Request.Form["endtime"]),
                    Index_Value = ConvertInt(Request.Form["index"]),
                    Name = Request.Form["name"],
                    Start_Time = ConvertDateTime(Request.Form["starttime"]),
                    Station_ID = ConvertLong(Request.Form["stationid"]),
                    Type = ConvertInt(Request.Form["type"])
                };

                var resultSave = await _panelsRepository.SavePanel(panel);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Panel Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeletePanel()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                }

                long panelId = ConvertLong(Request.Form["panelid"]);

                if(panelId < 0)
                {
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);
                }
                
                var resultDelete = await _panelsRepository.DeletePanel(panelId);
                return Ok(resultDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Panels Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
        }

        
       

        [HttpPost("call/panelid")]  //If from a panel (Web and iOS)
        public async Task<IActionResult> CallByPanel()
        {
            String tokenid = "";
            String panelid = "";
           
            try
            {
                long tokenIdValue = 0;
                long panelIdValue = 0;
            
                tokenid = Request.Form["tokenid"];
                panelid = Request.Form["panelid"];
                string callTo = Request.Form["callto"];

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
                Panel panel = await _panelsRepository.GetPanel(panelIdValue);

                switch (panel.Type)
                {
                    case (int)BUTTON_TYPE.MACRO_BUTTON:
                        //Place Calls
                        //string callResult = await _localCallService.Call(panelIdValue, callTo);

                        //return Ok(result + " - Device: " + panel.WNIP_Device_ID + " Channel: " + panel.Channel + " Call To: " + callTo);

                        //Set Crosspoints

                        break;
                    
                    case (int)BUTTON_TYPE.SB_BUTTON:
                        //Place Calls
                        var resultCall = await _localCallService.Call(panelIdValue, callTo);

                        if(callTo == "~")
                        {
                            break;
                        }
                        //Set Crosspoints
                        String takeSourceResult = await _localCallService.Take(panel.Audio_Receive_From_Source, panel.Channel_ID);
                        String takeDestResult = await _localCallService.Take(panel.Channel_ID, panel.Audio_Send_To_Dest);

                        break;
                    case (int)BUTTON_TYPE.ROUTE_X:
                        //Set Crosspoints
                        String takeResultX = await _localCallService.Take(panel.Audio_Receive_From_Source, panel.Audio_Send_To_Dest);
                        break;
                    case (int)BUTTON_TYPE.ROUTE_XY:
                        //Set Crosspoints
                        String takeResultXY = await _localCallService.Take(panel.Audio_Receive_From_Source, panel.Audio_Send_To_Dest);
                        break;
                    case (int)BUTTON_TYPE.ROUTE_X_AB:
                        
                        //Set Crosspoints
                        break;
                    default:
                        break;
                }
               
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Panel Action Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

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

                string callResult = await _localCallService.Call(deviceIdValue, ConvertInt(channel), callTo);

                return Ok(result + " - Device: " + deviceIdValue + " Channel: " + channel + " Call To: " + callTo);
            } catch(Exception ex)
            {
                Console.WriteLine("Bad Call Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

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
            } catch(Exception ex) {
                Console.WriteLine("Bad Take Request: " + ex);
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
