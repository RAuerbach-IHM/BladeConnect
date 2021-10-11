using SwitchBladeInterface.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IDevicesRepository
    {
        public Task<Device> GetDevice(long id);

        public Task<Device> GetDeviceByType(int type);
        public Task<IEnumerable<Device>> GetDevices();
        public Task<IEnumerable<Device>> GetDevicesByRoomId(long roomId);
        public Task<IEnumerable<Device>> GetDevicesBySiteId(long siteId);
        public Task<bool> SaveDevice(Device device);
        public Task<string> DeleteDevice(long deviceId);
    }
}