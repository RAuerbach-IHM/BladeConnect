using System;
using System.Threading.Tasks;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.NetworkServices;
using SwitchBladeInterface.API.Repositories.Interfaces;
using static SwitchBladeInterface.API.Enums.Enums;

namespace SwitchBladeInterface.API.Services.LocalServices
{
    public class LocalCallService : ILocalCallService
    {
        private readonly IDevicesRepository _devicesRepository;
        private readonly IPanelsRepository _panelsRepository;
        private readonly IChannelInfoRepository _channelInfoRepository;

        public LocalCallService(IDevicesRepository devicesRepository, IPanelsRepository panelsRepository, IChannelInfoRepository channelInfoRepository)
        {
            _devicesRepository = devicesRepository;
            _panelsRepository = panelsRepository;
            _channelInfoRepository = channelInfoRepository;
        }

        NetworkClientService networkClientService = new NetworkClientService();
        public async Task<String> Call(long panelID, string callTo)  //From a Panel
        {
            try
            {
                //Get Panel (SBButton)
                Panel panel = await _panelsRepository.GetPanel(panelID);

                Console.WriteLine("Device ID " + panel.WNIP_Device_ID);
                Console.WriteLine("Channel " + panel.Channel);
                Console.WriteLine("Call To " + callTo);

                if(string.IsNullOrEmpty(callTo))
                {
                    callTo = panel.Call_To;
                }

                if(string.IsNullOrEmpty(callTo))
                {
                    return "No 'Call To' information";
                }

                //Get Device
                var device = await _devicesRepository.GetDevice(panel.WNIP_Device_ID);

                var channelInfo = await _channelInfoRepository.GetChannelInfo(panel.WNIP_Device_ID, panel.Channel);
                
                Console.WriteLine("Channel Info " + channelInfo.Channel_Number);

                //Form Message String
                string from = panel.Channel.ToString("00");
               
                string message = "<PHONE:" + from + "|MakeACall:[" + callTo + "]>";

                Console.WriteLine("MESSAGE " + message);

                if (callTo == "~")
                {
                    //HANG UP
                    message = "<PHONE:" + from + "|HangUp:1>";
                }
                else
                {
                    //Safety Stop if channel is not IDLE or already in use
                    if(!GetCanCallOut(channelInfo.Status))
                    {
                        Console.WriteLine("Channel In Use");
                        return "Channel In Use";
                    }
                }

              
                UDPClientService udpClientService = new UDPClientService();
                udpClientService.Send(device, message);

                return "Call Request Sent - Device: " + device.ID + " Message: " + message;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error On Call Out Request - " + ex);
                return ("Error On Call Out Request - " + ex);
            }
        }

        public async Task<String> Call(long deviceID, int channel, string callTo)  //MAIN
        {
            Console.WriteLine("Device ID " + deviceID);
            Console.WriteLine("Channel " + channel);
            Console.WriteLine("Call To " + callTo);

            if (string.IsNullOrEmpty(callTo))
            {
                return "No 'Call To' information";
            }

            try
            {
                var device = await _devicesRepository.GetDeviceByType((int)DEVICE_TYPE.WHEATNET_SWITCHBLADE);
                deviceID = device.ID;

                var channelInfo = await _channelInfoRepository.GetChannelInfo(deviceID, channel);

                Console.WriteLine("Channel Info " + channelInfo.Channel_Number);

                //Form Message String
                string from = channel.ToString("00");
                
                string message = "<PHONE:" + from + "|MakeACall:[" + callTo + "]>";

                Console.WriteLine("MESSAGE " + message);

                if (callTo == "~")
                {
                    //HANG UP
                    message = "<PHONE:" + from + "|HangUp:1>";
                }
                else
                {
                    //Safety Stop if channel is not IDLE or already in use
                    if (!GetCanCallOut(channelInfo.Status))
                    {
                        Console.WriteLine("Channel In Use");
                        return "Channel In Use";
                    }
                }

               
                UDPClientService udpClientService = new UDPClientService();
                udpClientService.Send(device, message);

                return "Call Request Sent - Device: " + device.ID + " Message: " + message;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error On Call Out Request - " + ex);
                return ("Error On Call Out Request - " + ex);
            }
        }

        public async Task<String> Take(String source, String destination)   //From Custom XY Route Panel (iOS)
        {
            try
            {
                //Get Default Device
                Device wnipDevice = await _devicesRepository.GetDeviceByType((int)DEVICE_TYPE.WHEATNET_BLADE);

                if (wnipDevice == null)
                    return "Error - Device not found";

                //Connect to Switchblade
                networkClientService.StartClient(wnipDevice);
                string audioSendToDest = destination;
                string audioReceiveFromSource = source;

                if (string.IsNullOrEmpty(audioSendToDest))
                    return "Error - desination not found";

                if (string.IsNullOrEmpty(audioReceiveFromSource))
                    return "Error - Source not found";

                //Audio Send To Dest
                String message = string.Concat("<DST:", audioSendToDest, "|SRC:", audioReceiveFromSource, ">");

                networkClientService.SendMessage(message);

                return "Take request sent";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error On Crosspoint Change Request- " + ex);
                return ("Error On Crosspoint Change Request - " + ex);
            }
        }

        private bool GetCanCallOut(int channelStatus)
        {
            bool result = false;
            CHANNEL_STATUS status = (CHANNEL_STATUS)channelStatus;

            switch (status)
            {
                case CHANNEL_STATUS.NONE:
                    break;
                case CHANNEL_STATUS.IDLE:
                    result = true;
                    break;
                case CHANNEL_STATUS.BUSY:
                    break;
                case CHANNEL_STATUS.RINGING_IN:
                    break;
                case CHANNEL_STATUS.ANSWERED:
                    break;
                case CHANNEL_STATUS.ON_HOLD:
                    break;
                case CHANNEL_STATUS.RINGING_OUT:
                    break;
                case CHANNEL_STATUS.RECORDING:
                    break;
                case CHANNEL_STATUS.FILE_PLAYING:
                    break;
                case CHANNEL_STATUS.FILE_STOPPED:
                    break;
                case CHANNEL_STATUS.REGISTER_FAIL:
                    break;
                case CHANNEL_STATUS.REGISTERED:
                    result = true;
                    break;
                case CHANNEL_STATUS.UNREGISTERED:
                    break;
                case CHANNEL_STATUS.NOT_INITIALISED:
                    break;
                case CHANNEL_STATUS.INITIALISED:
                    result = true;
                    break;
                case CHANNEL_STATUS.HUNG_UP:
                    result = true;
                    break;
                case CHANNEL_STATUS.ERROR:
                    result = true;
                    break;
                case CHANNEL_STATUS.ERROR_2:
                    result = true;
                    break;
                case CHANNEL_STATUS.NOT_FOUND:
                    break;
                case CHANNEL_STATUS.DISCONNECTED:
                    result = true;
                    break;
                case CHANNEL_STATUS.CROSSPOINT:
                    break;
                default:
                    break;
            }
            //(channelInfo.Status != ((int)CHANNEL_STATUS.IDLE) && channelInfo.Status != ((int)CHANNEL_STATUS.HUNG_UP));

            return result;
        }
    }
}
