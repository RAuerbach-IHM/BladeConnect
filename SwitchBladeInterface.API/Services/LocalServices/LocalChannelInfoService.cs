using SwitchBladeInterface.API.NetworkServices;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using static SwitchBladeInterface.API.Enums.Enums;

namespace SwitchBladeInterface.API.Services.LocalServices
{
    public class LocalChannelInfoService : ILocalChannelInfoService
    {
        private readonly IDevicesRepository _devicesRepository;
        
        public LocalChannelInfoService(IDevicesRepository devicesRepository)
        {
            _devicesRepository = devicesRepository;
        }

        
        public async Task<String> RequestUpdateFromDeviceAllChannels()
        {
            string message;
            try
            {
                //Get Devices
                var devices = await _devicesRepository.GetDevices();


                foreach (var device in devices)
                {
                    //if not a SwitchBlade, move on
                    if (device.Type != (int)DEVICE_TYPE.WHEATNET_SWITCHBLADE)
                        break;

                    UDPClientService udpClientService = new UDPClientService();


                    for (int i = 0; i < 24; i++)
                    {
                        string ch = (i + 1).ToString("00");
                        message = "<PHONE:" + ch + "?Status>";
                        udpClientService.Send(device, message);
                    }

                    for (int i = 0; i < 24; i++)
                    {
                        string ch = (i + 1).ToString("00");
                        message = "<PHONE:" + ch + "?UserName>";
                        udpClientService.Send(device, message);
                    }

                    for (int i = 0; i < 24; i++)
                    {
                        string ch = (i + 1).ToString("00");
                        message = "<PHONE:" + ch + "?SIPCallDetails>";
                        udpClientService.Send(device, message);
                    }
                }
                Console.WriteLine("Channel Info Request Sent");
                return "Channel Info Update Request Sent";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error On Channel Info Request - " + ex);
                return ("Error On Channel Info Request - " + ex);
            }
        }
    }
}