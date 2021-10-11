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
    public class RoomsRepository : IRoomsRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public RoomsRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<Room> GetRoom(long id)
        {
            try
            {
                return await _context.Rooms
                        .FirstOrDefaultAsync(c => c.ID == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Room - " + ex);
                return new Room();
            }
        }

        public async Task<Room> GetRoomBySiteId(long roomId, int siteId)
        {
            try
            {
                return await _context.Rooms
                        .FirstOrDefaultAsync(c => c.Room_id == roomId && c.Site_id == siteId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Room - " + ex);
                return new Room();
            }
        }
        public async Task<Room> GetRoomByType(int type)
        {
            try
            {
                return await _context.Rooms
                        .FirstOrDefaultAsync(c => c.Type == type);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Room - " + ex);
                return new Room();
            }
        }

        public async Task<IEnumerable<Room>> GetRooms(bool includeHidden)
        {
            try
            {
                if(includeHidden)
                    return await _context.Rooms.ToListAsync();
                else
                    return await _context.Rooms.Where(r => r.Hidden == 0).ToListAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Rooms - " + ex);
                return new List<Room>();
            }
        }

        public async Task<IEnumerable<Room>> GetRoomsBySiteId(int siteId, bool includeHidden)
        {
            try
            {
                if (includeHidden)
                    return await _context.Rooms.Where(r => r.Site_id == siteId).ToListAsync();
                else
                    return await _context.Rooms.Where(r => r.Hidden == 0 && r.Site_id == siteId).ToListAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Rooms - " + ex);
                return new List<Room>();
            }
        }

        public async Task<bool> SaveRoom(Room room)
        {
            if (room == null)
            {
                return false;
            }
            try
            {
                var result = await _context.Rooms.FirstOrDefaultAsync(r => r.ID == room.ID ||  (r.Room_id == room.Room_id && r.Site_id == room.Site_id) );
                if (result != null)  //Update
                {
                    try
                    {
                        result.Name = room.Name;
                        result.Type = room.Type;
                        result.Designator = room.Designator;
                        result.Hidden = room.Hidden;
                        result.Hidden_mic = room.Hidden_mic;
                        result.Site_id = room.Site_id;
                        result.Room_id = room.Room_id;

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating room. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        room.ID = DateTime.UtcNow.Ticks;
                        await _context.AddAsync(room);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving room. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from room list. " + ex);
                return false;
            }
            return true;

        }

        public async Task<string> DeleteRoom(long roomToDeleteId)
        {
            try
            {
                _context.Remove(await _context.Rooms.FirstAsync(r => r.ID == roomToDeleteId));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting room. " + ex);
                return "Error deleting room";
            }
            return "";
        }

        public async Task<string> DeleteRoomsBySite(int siteId )
        {
            try
            {
                _context.RemoveRange(_context.Rooms.Where(r => r.Site_id == siteId));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting room. " + ex);
                return "Error deleting room";
            }
            return "";
        }

    }
}
