using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using static SwitchBladeInterface.API.Enums.Enums;

namespace SwitchBladeInterface.API.Services.RoomServices
{
    public class RoomsService
    {
        private readonly SwitchBladeInterfaceContext _context;
        private IRoomsRepository _roomsRepository;

        public RoomsService(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateDefaultRooms(int siteId)
        {
            bool result = true;

            //var context = new SwitchBladeInterfaceContext();
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var type = (int)ROOM_TYPE.ATLANTIS;

            IRoomsRepository roomsRepository = new RoomsRepository(_context);
            _roomsRepository = roomsRepository;

            for (int i = 0; i < 26; i++)
            {
                //Get Room
                Room room = await _roomsRepository.GetRoomBySiteId(i + 1, siteId);

                if (i > 1)
                    type = (int)ROOM_TYPE.APOLLO;
                if (i > 6)
                    type = (int)ROOM_TYPE.MERCURY_PLUS;
                if (type > 10)
                    type = (int)ROOM_TYPE.Mercury;


                if (room == null)
                {
                    //Create new Room
                    Room newRoom = new Room
                    {
                        Designator = alpha[i].ToString(),
                        Hidden = 0,
                        Hidden_mic = 0,
                        Name = "Studio " + alpha[i].ToString(),
                        Type = type,
                        Site_id = siteId,
                        Room_id = i + 1
                    };


                    if (!await _roomsRepository.SaveRoom(newRoom))
                        result = false;

                    
                }
            }

            type = (int)ROOM_TYPE.BOOTH;
            for (int i = 0; i < 26; i++)
            {
                //Get Room
                Room room = await _roomsRepository.GetRoomBySiteId(i + 26 + 1, siteId);

                if (room == null)
                {
                    //Create new Room
                    Room newRoom = new Room
                    {
                        Designator = alpha[i].ToString(),
                        Hidden = 0,
                        Hidden_mic = 0,
                        Name = "Booth " + alpha[i].ToString(),
                        Type = type,
                        Site_id = siteId,
                        Room_id = i + 26 + 1
                    };

                    if (!await _roomsRepository.SaveRoom(newRoom))
                        result = false;
                }
            }

            
            return result;
        }


        public async Task<bool> DeleteRoomsBySite(int siteId)
        {
            bool result = false;

            IRoomsRepository roomsRepository = new RoomsRepository(_context);
            _roomsRepository = roomsRepository;

            await _roomsRepository.DeleteRoomsBySite(siteId);

            return result;
        }
    }
}
