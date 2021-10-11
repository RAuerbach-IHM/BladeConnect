using Microsoft.AspNetCore.Mvc;
using SwitchBladeInterface.API.Repositories.Interfaces;
using SwitchBladeInterface.API.Services.LocalServices;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Controllers
{

    [ApiController]
    [Route("api/channelinfo")]
    public class ChannelInfoController : ControllerBase
    {
        private readonly IChannelInfoRepository _channelInfoRepository;
        private readonly ILocalChannelInfoService _localChannelInfoService;
        public ChannelInfoController(IChannelInfoRepository channelInfoRepository, ILocalChannelInfoService localChannelInfoService)
        {
            _channelInfoRepository = channelInfoRepository ??
                throw new ArgumentNullException(nameof(channelInfoRepository));

            _localChannelInfoService = localChannelInfoService ??
                throw new ArgumentNullException(nameof(localChannelInfoService));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetChannelInfo()
        {
            try
            {
                var channelInfoFromRepository = await _channelInfoRepository.GetChannelInfo();
                //Console.WriteLine(channelInfoFromRepository.Count());
                //Console.WriteLine(channelInfoFromRepository[2].Channel_Number);
                //Console.WriteLine(channelInfoFromRepository[2].Name);

                return Ok(channelInfoFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad ChannelInfo Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }


        //Sends request to device to update channel info and status
        [HttpGet("refresh")]
        public async Task<IActionResult> RequestRefreshChannelInfo()
        {
            try
            {
                var result = await _localChannelInfoService.RequestUpdateFromDeviceAllChannels();
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Refresh ChannelInfo Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpGet("single/{deviceid}/{channel}")]
        public async Task<IActionResult> GetChannelInfoByID(string deviceid, string channel)
        {
            try
            { 
                var channelInfoFromRepository = await _channelInfoRepository.GetChannelInfo(ConvertLong(deviceid), ConvertInt(channel));
                //Console.WriteLine(channelInfoFromRepository.Count());
                //Console.WriteLine(channelInfoFromRepository[2].Channel_Number);
                //Console.WriteLine(channelInfoFromRepository[2].Name);

                return Ok(channelInfoFromRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad ChannelInfo Request: " + ex);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
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
