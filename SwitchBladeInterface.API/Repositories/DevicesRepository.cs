using Microsoft.EntityFrameworkCore;
using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories
{
    public class DevicesRepository : IDevicesRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public DevicesRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        
        public async Task<Device> GetDevice(long id)
        {
            try
            {
                return await _context.Devices
                        .FirstOrDefaultAsync(c => c.ID == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Device - " + ex);
                return new Device();
            }
        }
        public async Task<Device> GetDeviceByType(int type)
        {
            try
            {
                return await _context.Devices
                        .FirstOrDefaultAsync(c => c.Type == type);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Device - " + ex);
                return new Device();
            }
        }

        public async Task<IEnumerable<Device>> GetDevices()
        {
            try
            {
                return await _context.Devices.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Devices - " + ex);
                return new List<Device>();
            }
        }

        public async Task<IEnumerable<Device>> GetDevicesByRoomId(long roomId)
        {
            try
            {
                return await _context.Devices.Where(c => c.Room_Id == roomId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Devices - " + ex);
                return new List<Device>();
            }
        }

        public async Task<IEnumerable<Device>> GetDevicesBySiteId(long siteId)
        {
            try
            {
                return await _context.Devices.Where(c => c.Site_Id == siteId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Devices - " + ex);
                return new List<Device>();
            }
        }

        public async Task<bool> SaveDevice(Device device)
        {
            if (device == null)
            {
                return false;
            }
            try
            {
                var result = await _context.Devices.FirstOrDefaultAsync(d => d.ID == device.ID);
                if (result != null)  //Update
                {
                    try
                    {
                        result.Manufacturer = device.Manufacturer;
                        result.Name = device.Name;
                        result.Port = device.Port;
                        result.Type = device.Type;
                        result.WNIP_Address = device.WNIP_Address;
                        result.IP_Address = device.IP_Address;
                        result.Number = device.Number;
                        result.Room_Id = device.Room_Id;

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating device. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        await _context.AddAsync(device);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving device. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from device list. " + ex);
                return false;
            }
            return true;

        }

        public async Task<string> DeleteDevice(long deviceToDeleteId)
        {
            try
            {
                _context.Remove(await _context.Devices.FirstAsync(d => d.ID == deviceToDeleteId));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting device. " + ex);
                return "Error deleting device";
            }
            return "";
        }
    }
}
