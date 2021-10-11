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
    public class DisplayEventsRepository : IDisplayEventsRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public DisplayEventsRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<DisplayEvent> GetDisplayEvent(long id)
        {
            try
            {
                return await _context.DisplayEvents.FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                return new DisplayEvent();
            }
        }

        public async Task<DisplayEvent> GetDisplayEvent(int id)
        {
            try
            {
                return await _context.DisplayEvents.FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                return new DisplayEvent();
            }
        }
        public async Task<DisplayEvent> GetDisplayEventBySiteId(long displayEventId, int siteId)
        {
            try
            {
                return await _context.DisplayEvents
                        .FirstOrDefaultAsync(c => c.Display_Event_id == displayEventId && c.Site_id == siteId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Display Event - " + ex);
                return new DisplayEvent();
            }
        }

        public async Task<IEnumerable<DisplayEvent>> GetDisplayEvents(bool includeHidden)
        {
            try
            {
                if (includeHidden)
                    return await _context.DisplayEvents.ToListAsync();
                else
                    return await _context.DisplayEvents.Where(r => r.Hidden == 0).ToListAsync();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Display Events - " + ex);
                return new List<DisplayEvent>();
            }
        }

        public async Task<IEnumerable<DisplayEvent>> GetDisplayEventsBySiteId(int siteId, bool includeHidden)
        {
            try
            {
                if (includeHidden)
                    return await _context.DisplayEvents.Where(r => r.Site_id == siteId).ToListAsync();
                else
                    return await _context.DisplayEvents.Where(r => r.Hidden == 0 && r.Site_id == siteId).ToListAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Display Events - " + ex);
                return new List<DisplayEvent>();
            }
        }

        public async Task<bool> SaveDisplayEvent(DisplayEvent displayEvent)
        {
            if (displayEvent == null)
            {
                return false;
            }
            try
            {
                var result = await _context.DisplayEvents.FirstOrDefaultAsync(a => a.Id == displayEvent.Id);
                if (result != null)  //Update
                {
                    try
                    {
                        result.Display_command = displayEvent.Display_command;
                        result.Label = displayEvent.Label;
                        result.Hidden = displayEvent.Hidden;
                        result.Site_id = displayEvent.Site_id;
                        result.Display_Event_id = displayEvent.Display_Event_id;

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating displayEvent. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        displayEvent.Id = DateTime.UtcNow.Ticks;
                        await _context.AddAsync(displayEvent);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving Display Event. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from Display Event list. " + ex);
                return false;
            }
            return true;

        }

        public async Task<string> DeleteDisplayEvent(long displayEventToDeleteId)
        {
            try
            {
                _context.Remove(await _context.DisplayEvents.FirstAsync(d => d.Id == displayEventToDeleteId));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting display event. " + ex);
                return "Error deleting display event";
            }
            return "";
        }

        public async Task<string> DeleteDisplayEventsBySite(int siteId)
        {
            try
            {
                _context.RemoveRange(_context.DisplayEvents.Where(r => r.Site_id == siteId));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting display event. " + ex);
                return "Error deleting display event";
            }
            return "";
        }
    }
}
