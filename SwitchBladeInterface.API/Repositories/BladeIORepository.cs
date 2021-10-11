using Microsoft.EntityFrameworkCore;
using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories
{
    public class BladeIORepository : IBladeIORepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public BladeIORepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<BladeIO>> GetBladeIOs(string type)
        {
            try
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                return await _context.BladeIOs.Where(i => i.Type == type).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Blade IO - " + ex);
                return new List<BladeIO>();
            }
        }
        public async Task<IEnumerable<BladeIO>> GetBladeIOsByAccount(string type, long accountId)
        {
            try
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                //Get AccountBladeIOsRelations
                List<AccountBladeIOsRelations> relations = await _context.AccountBladeIOsRelations.Where(c => c.account_id == accountId && c.type == type).ToListAsync();

                Console.WriteLine("TEST B " + relations.Count.ToString());

                List<long> bladeIOIDs = new List<long>();
                foreach (AccountBladeIOsRelations relation in relations)
                {
                    bladeIOIDs.Add(relation.bladeIO_id);
                }

                Console.WriteLine("TEST C " + bladeIOIDs.Count.ToString());
                return await _context.BladeIOs.Where(c => bladeIOIDs.Contains(c.ID)).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Blade IO - " + ex);
                return new List<BladeIO>();
            }
        }

        public async Task<IEnumerable<BladeIO>> GetBladeIOsForSwitchBladeChannels()
        {
            try
            {
                return await _context.BladeIOs.Where(c => c.Location.StartsWith("SB") || c.Location.StartsWith("sb") || c.Location.StartsWith("SwtchBld") || c.Location.StartsWith("swtchbld")).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Blade IO - " + ex);
                return new List<BladeIO>();
            }
        }


        public async Task<BladeIO> GetBladeIOByWNID(string type, string wnid)
        {
            try
            {
                if (wnid == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }


                //Get AccountBladeIOsRelations
                //List<AccountBladeIOsRelations> relations = await _context.AccountBladeIOsRelations.Where(c => c.account_id == accountId && c.type == type).ToListAsync();
              
                /*
                List<long> bladeIOIDs = new List<long>();
                foreach (AccountBladeIOsRelations relation in relations)
                {
                    bladeIOIDs.Add(relation.bladeIO_id);
                }
                */

                return await _context.BladeIOs.FirstOrDefaultAsync(c => c.Type == type && c.WN_ID == wnid);
                //return await _context.BladeIOs.FirstOrDefaultAsync(c => bladeIOIDs.Contains(c.ID) && c.WN_ID == wnid);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Blade IO - " + ex);
                return new BladeIO();
            }
        }


        public async Task<bool> SavePersonalBladeIOs(string type, long accountId, List<long> bladeIOIds)
        {
            if (bladeIOIds == null)
            {
                return false;
            }

            try
            {
                Console.WriteLine("DELETE PERSONAL BladeIO A" + bladeIOIds.Count());
                //Delete all
                var itemsToDelete = await _context.AccountBladeIOsRelations.Where(b => b.account_id == accountId && b.type == type).ToListAsync();

                Console.WriteLine("DELETE PERSONAL BladeIO A B" + bladeIOIds.Count());

                foreach (AccountBladeIOsRelations itemToDelete in itemsToDelete)
                {
                    _context.Remove(itemToDelete);
                }

                Console.WriteLine("DELETE PERSONAL BladeIO A C");

                foreach (long bladeioId in bladeIOIds)
                {
                    try
                    {
                        AccountBladeIOsRelations relation = new AccountBladeIOsRelations
                        {
                            bladeIO_id = bladeioId,
                            account_id = accountId,
                            type = type
                        };

                        Console.WriteLine("DELETE PERSONAL BladeIO D");
                        await _context.AddAsync(relation);
                        Console.WriteLine("DELETE PERSONAL BladeIO E");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving personal blade IO. " + ex);
                        return false;
                    }
                }

                Console.WriteLine("DELETE PERSONAL Blade IO F");
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from account bladeIO relations. " + ex);
                return false;
            }
            return true;

        }

        
    }
}