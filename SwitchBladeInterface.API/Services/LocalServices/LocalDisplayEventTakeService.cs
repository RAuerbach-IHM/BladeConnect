using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.NetworkServices;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using static SwitchBladeInterface.API.Enums.Enums;

namespace SwitchBladeInterface.API.Services.LocalServices
{
    public class LocalDisplayEventTakeService : ILocalDisplayEventTakeService
    {
        private readonly IDevicesRepository _devicesRepository;
        private readonly IDisplayEventsRepository _displayEventsRepository;
        private readonly IRoomsRepository _roomsRepository;


        public LocalDisplayEventTakeService(IDevicesRepository devicesRepository, IDisplayEventsRepository displayEventsRepository, IRoomsRepository roomsRepository)
        {
            _devicesRepository = devicesRepository;
            _displayEventsRepository = displayEventsRepository;
            _roomsRepository = roomsRepository;
        }

        /*
        public async Task<String> Take(long displayId, int displayEventId)
        {
            try
            {
                Console.WriteLine("Device ID " + displayId);
                Console.WriteLine("Display Event ID " + displayEventId);

                //Get Display Device
                var device = await _devicesRepository.GetDevice(displayId);

                //Get Display Event
                var displayEvent = await _displayEventsRepository.GetDisplayEvent(displayEventId);

                UDPClientService udpClientService = new UDPClientService();
                udpClientService.Send(device, displayEvent.display_command);

                return "display Event Request Sent - Device: " + device.ID + " Message: " + displayEvent.display_command;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error On Display Event Request - " + ex);
                return ("Error On Display Event Request - " + ex);
            }
        }
        */
        public async Task<String> Take(long roomId, long displayEventId, int option)
        {
            try
            {
                Console.WriteLine("Room ID " + roomId);
                Console.WriteLine("Display Event ID " + displayEventId);


                //Get Room
                var room = await _roomsRepository.GetRoom(roomId);

                //Get Devices in room
                var devices = await _devicesRepository.GetDevicesByRoomId(roomId);

                //Get Display Event
                var displayEvent = await _displayEventsRepository.GetDisplayEvent(displayEventId);
                String displayEventCommand = displayEvent.Display_command;

                //Create DisplayEvent with options
                if(option == (int)DISPLAY_EVENT_OPTION.MIC_ON)
                {
                    displayEventCommand = String.Concat("on", displayEvent.Display_command);
                } else if (option == (int)DISPLAY_EVENT_OPTION.MIC_OFF)
                {
                    displayEventCommand = String.Concat("off", displayEvent.Display_command);
                }

                //Change display for each device in room
                UDPClientService udpClientService = new UDPClientService();
                foreach( Device device in devices)
                {
                    udpClientService.Send(device, displayEventCommand);
                }
                

                return "Display Event Requests Sent - Room: " + room.ID + " Message: " + displayEvent.Display_command;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error On Display Event Request - " + ex);
                return ("Error On Display Event Request - " + ex);
            }
        }
    }
}
