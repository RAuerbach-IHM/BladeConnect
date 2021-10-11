using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using static SwitchBladeInterface.API.Enums.Enums;

namespace SwitchBladeInterface.API.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DevicesController : ControllerBase
    {
        private readonly ITokensRepository _tokensRepository;
        private readonly IDevicesRepository _devicesRepository;

        private int switchbladePort = 26047;
        private int bladePort = 55776;
        private int displayPort = 5000;
        public DevicesController(ITokensRepository tokensRepository, IDevicesRepository devicesRepository)
        {
            _tokensRepository = tokensRepository ??
                throw new ArgumentNullException(nameof(tokensRepository));

            _devicesRepository = devicesRepository ??
                throw new ArgumentNullException(nameof(devicesRepository));
        }
        
        [HttpPost()]
        public async Task<IActionResult> GetDevices()
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

                var devicesFromRepository = await _devicesRepository.GetDevicesBySiteId(ConvertInt(Request.Form["siteid"]));
                return Ok(devicesFromRepository);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("Bad Devices Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("deviceid")]
        public async Task<IActionResult> GetDeviceByID()
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

                Int64 deviceIdValue = ConvertLong(Request.Form["deviceid"]);


                //Get Device
                var deviceFromRepository = await _devicesRepository.GetDevice(deviceIdValue);
                
                return Ok(deviceFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Device Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }


        [HttpPost("save")]
        public async Task<IActionResult> SaveDevice()
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
                    Console.WriteLine("Device Not Valid");
                    return Ok("Device Not Valid");
                }

                Int64 deviceId = -1;
                var result = Int64.TryParse(Request.Form["id"], out deviceId);

                if (!result)
                {
                    Console.WriteLine("Device ID Not Valid");
                    return Ok("Device ID Not Valid");
                }

                Int64 roomId = -1;
                var resultRoomId = Int64.TryParse(Request.Form["roomid"], out roomId);

                if (!resultRoomId)
                {
                    //Console.WriteLine("Room ID Not Valid");
                    //return Ok("Device ID Not Valid");
                    roomId = 0;
                }

                //Verify Port
                //Int32 port = -1;
                //var resultPort = Int32.TryParse(Request.Form["port"], out port);

                //if (!resultPort)
                //{
                //    Console.WriteLine("Device Port Not Valid");
                //    return Ok("Device Port Not Valid");
                //}

                //Verify Type
                Int32 type= -1;
                var resultType = Int32.TryParse(Request.Form["type"], out type);

                if (!resultType)
                {
                    Console.WriteLine("Device Type Not Valid");
                    return Ok("Device Type Not Valid");
                }

                //Verify Room Number
                Int32 number = 0;
                var resultNumber = Int32.TryParse(Request.Form["number"], out number);

                /*
                if (!resultNumber)
                {
                    Console.WriteLine("Device Type Not Valid");
                    return Ok("Device Type Not Valid");
                }
                */

                int port = 0;
                if (type == (int)DEVICE_TYPE.WHEATNET_SWITCHBLADE)
                    port = switchbladePort;
                else if (type == (int)DEVICE_TYPE.WHEATNET_BLADE)
                    port = bladePort;
                else if (type == (int)DEVICE_TYPE.Display)
                    port = displayPort;

                //Get Device from array
                Device device = new Device
                {
                    ID = deviceId,
                    Manufacturer = Request.Form["manufacturer"],
                    Name = Request.Form["name"],
                    Port = port,
                    Type = type,
                    WNIP_Address = Request.Form["wnipaddress"],
                    IP_Address = Request.Form["ipaddress"],
                    Number = number,
                    Room_Id = roomId,
                    Site_Id = ConvertInt(Request.Form["siteid"])
                };

                var resultSave = await _devicesRepository.SaveDevice(device);
                return Ok(resultSave);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Save Device Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteDevice()
        {
            try
            {
                string resultToken = await VerifyAdminToken(Request.Form["tokenid"]);

                if (resultToken != "")
                {
                    return Ok(resultToken);
                }

                if (string.IsNullOrEmpty(Request.Form["deviceid"]))
                {
                    Console.WriteLine("Device Not Found");
                    return Ok("Not Found");
                }

                Int64 deviceId = -1;
                var result = Int64.TryParse(Request.Form["deviceid"], out deviceId);

                if (!result)
                {
                    Console.WriteLine("Device Not Found");
                    return Ok("Not Found");
                }

                var resultDelete = await _devicesRepository.DeleteDevice(deviceId);
                return Ok(resultDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Delete Device Request: " + ex);
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
