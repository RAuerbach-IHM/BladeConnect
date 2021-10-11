using SwitchBladeInterface.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IRoomsRepository
    {
        public Task<Room> GetRoom(long id);

        public Task<Room> GetRoomBySiteId(long id, int siteId);

        public Task<IEnumerable<Room>> GetRoomsBySiteId(int siteId, bool includeHidden);

        public Task<Room> GetRoomByType(int type);
        public Task<IEnumerable<Room>> GetRooms(bool includeHidden);
        public Task<bool> SaveRoom(Room room);
        public Task<string> DeleteRoom(long roomId);
        public Task<string> DeleteRoomsBySite(int siteId);
    }
}