using Microsoft.EntityFrameworkCore;
using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using SwitchBladeInterface.API.Services.DisplayEventServices;
using SwitchBladeInterface.API.Services.RoomServices;
using SwitchBladeInterface.API.Services.SiteServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories
{
    public class SitesRepository : ISitesRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public SitesRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<Site> GetSite(int id)
        {
            try
            {
                return await _context.Sites
                        .FirstOrDefaultAsync(c => c.ID == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Site - " + ex);
                return new Site();
            }
        }
        

        public async Task<IEnumerable<Site>> GetSites()
        {
            try
            {
                return await _context.Sites.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Sites - " + ex);
                return new List<Site>();
            }
        }

        public async Task<IEnumerable<Site>> GetSites(string ipAddress)
        {
            SiteService siteService = new SiteService();
            List<Site> sitesToReturn = new List<Site>();
            
            try
            {
                var sites = await _context.Sites.ToListAsync();

                foreach(var site in sites)
                {
                    if (siteService.IsBetweenOrEqual(ipAddress, site.Ip_Range_Low, site.Ip_Range_High))
                        sitesToReturn.Add(site);
                }
                return sitesToReturn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Sites - " + ex);
                return new List<Site>();
            }
        }

        public async Task<bool> SaveSite(Site site)
        {
            if (site == null)
            {
                return false;
            }
            try
            {
                var result = await _context.Sites.FirstOrDefaultAsync(r => r.ID == site.ID);
                if (result != null)  //Update
                {
                    try
                    {
                        result.Market = site.Market;
                        result.Ip_Range_Low = site.Ip_Range_Low;
                        result.Ip_Range_High = site.Ip_Range_High;
                        result.Site_Code = site.Site_Code;
                        result.State = site.State;
                        result.City = site.City;

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating site. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        await _context.AddAsync(site);
                        await _context.SaveChangesAsync();

                        //Create Rooms
                        RoomsService roomService = new RoomsService(_context);
                        await roomService.CreateDefaultRooms(site.ID);

                        //Create Display Events
                        DisplayEventsService displayEventsService = new DisplayEventsService(_context);
                        await displayEventsService.CreateDefaultDisplayEvents(site.ID);

                        //Create Accounts?

                        //Create Devices / Displays?

                        //Create Stations ?

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving site. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from site list. " + ex);
                return false;
            }
            return true;

        }

        public async Task<string> DeleteSite(int siteToDeleteId)
        {
            try
            {
                _context.Remove(await _context.Sites.FirstAsync(r => r.ID == siteToDeleteId));
                await _context.SaveChangesAsync();

                //Delete Rooms
                RoomsService roomService = new RoomsService(_context);
                await roomService.DeleteRoomsBySite(siteToDeleteId);

                //Delete DisplayEvents
                DisplayEventsService displayEventsService = new DisplayEventsService(_context);
                await displayEventsService.DeleteDisplayEventsBySite(siteToDeleteId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting site. " + ex);
                return "Error deleting site";
            }
            return "";
        }
    }
}