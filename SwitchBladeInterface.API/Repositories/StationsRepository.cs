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
    public class StationsRepository : IStationsRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public StationsRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Station> GetStation(String callLetters) {
            try
            {
                return await _context.Stations.FirstOrDefaultAsync(s => s.Call_Letters == callLetters);
            }catch(Exception ex)
            {
                Console.WriteLine("Error Retrieving Station - " + ex);
                return new Station();
            }
        }


        public async Task<IEnumerable<Station>> GetStationsByAccount(long accountId)
        {
            try { 
                //Get AccountStationsRelations
                List<AccountStationsRelations> relations = await _context.AccountStationsRelations.Where(c => c.account_id == accountId).ToListAsync();

                List<long> stationIDs = new List<long>();
                foreach(AccountStationsRelations relation in relations)
                {
                    stationIDs.Add(relation.station_id);
                }

                return await _context.Stations.Where(c => stationIDs.Contains(c.Id)).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Stations - " + ex);
                return new List<Station>();
            }

        }
        public async Task<IEnumerable<Station>> GetStations()
        {
            try
            {
                return await _context.Stations.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Stations - " + ex);
                return new List<Station>();
            }

        }

        public async Task<bool> SaveStation(Station station)
        {
            if (station == null)
            {
                return false;
            }
            try
            {
                var result = await _context.Stations.FirstOrDefaultAsync(s => s.Id == station.Id);
                if (result != null)  //Update
                {
                    try
                    {
                        result.Call_Letters = station.Call_Letters;
                        result.Market = station.Market;
                        result.Name = station.Name;
                        result.Frequency = station.Frequency;
                        result.Band = station.Band;

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating station. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        await _context.AddAsync(station);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving station. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from station list. " + ex);
                return false;
            }
            return true;

        }

        public async Task<string> DeleteStation(long stationToDeleteId)
        {
            try
            {
                _context.Remove(await _context.Stations.FirstAsync(s => s.Id == stationToDeleteId));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting station. " + ex);
                return "Error deleting station";
            }
            return "";
        }
    }
}
