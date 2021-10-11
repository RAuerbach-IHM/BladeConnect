using Microsoft.EntityFrameworkCore;
using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public AccountsRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Account> GetAccount(string username)
        {
            try
            {
                return await _context.Accounts.FirstOrDefaultAsync(a => a.user_name.Trim() == username.Trim());
            } catch(Exception ex)
            {
                return new Account();
            }
        }
        public async Task<List<Account>> GetAccounts()
        {
            try
            {
                List<Account> result = new List<Account>();
                result = await _context.Accounts.ToListAsync();
   
                foreach (Account item in result)
                {
                    if (string.IsNullOrEmpty(item.first_name))
                    {
                        item.first_name = item.first_name.Trim();
                    }
                    if (string.IsNullOrEmpty(item.last_name))
                    {
                        item.last_name = item.last_name.Trim();
                    }
                    if (string.IsNullOrEmpty(item.user_name))
                    {
                        item.user_name = item.user_name.Trim();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Accounts - " + ex);
                return new List<Account>();
            }
        }

        public async Task<bool> SaveAccount(Account account)
        {
            if (account == null)
            {
                return false;
            }
            try
            {
                var result = await _context.Accounts.FirstOrDefaultAsync(a => a.id == account.id);
                if (result != null)  //Update
                {
                    try
                    {
                        result.first_name = account.first_name;
                        result.last_name = account.last_name;
                        result.role = account.role;
                        result.show_personal_phonebook = account.show_personal_phonebook;
                        result.show_public_phonebook = account.show_public_phonebook;
                        result.password_required = account.password_required;
                        //result.password = account.password;
                       
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating account. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        await _context.AddAsync(account);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving account. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from account list. " + ex);
                return false;
            }
            return true;

        }

        public async Task<bool> SaveAccountStationsRelations(long accountId, List<long> relationIds)
        {
            if (relationIds == null)
            {
                return false;
            }

            try
            {
                Console.WriteLine("DELETE PERSONAL Stations A" + relationIds.Count());
                //Delete all
                var itemsToDelete = await _context.AccountStationsRelations.Where(r => r.account_id == accountId).ToListAsync();

                Console.WriteLine("DELETE PERSONAL Stations B" + relationIds.Count());

                foreach (AccountStationsRelations itemToDelete in itemsToDelete)
                {
                    _context.Remove(itemToDelete);
                }

                Console.WriteLine("DELETE PERSONAL Stations C");

                foreach (long relationId in relationIds)
                {
                    try
                    {
                        AccountStationsRelations relation = new AccountStationsRelations
                        {
                            station_id = relationId,
                            account_id = accountId
                        };

                        Console.WriteLine("DELETE PERSONAL Station D");
                        await _context.AddAsync(relation);
                        Console.WriteLine("DELETE PERSONAL Station E");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving account stations relations item. " + ex);
                        return false;
                    }
                }

                Console.WriteLine("DELETE PERSONAL Station F");
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from account stations relations. " + ex);
                return false;
            }
            return true;

        }


        public async Task<List<Account>> DeleteAccount(long accountToDeleteId)
        {
            if(accountToDeleteId == 200 || accountToDeleteId == 100)
            {
                return await GetAccounts();
            }
            
            try
            {
                //Account
                _context.Remove(await _context.Accounts.FirstAsync(a => a.id == accountToDeleteId));

                //Personal Phonebook
                var personalPhonebooks = await _context.PersonalPhonebooks.Where(p => p.account_id == accountToDeleteId).ToListAsync();
                foreach(PersonalPhonebook personalPhonebook in personalPhonebooks ) { 
                    _context.Remove(personalPhonebook);
                }

                //Personal BladeIOs
                var personalBladeIOs = await _context.AccountBladeIOsRelations.Where(b => b.account_id == accountToDeleteId).ToListAsync();
                foreach (AccountBladeIOsRelations personalBladeIO in personalBladeIOs)
                {
                    _context.Remove(personalBladeIO);
                }

                //Stations
                var stations = await _context.AccountStationsRelations.Where(s => s.account_id == accountToDeleteId).ToListAsync();
                foreach (AccountStationsRelations station in stations)
                {
                    _context.Remove(station);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting account. " + ex);
                return await GetAccounts();
            }
            return await GetAccounts();
        }

        public async Task<bool> SetAccountPassword(Account account)
        {
            if (account == null)
            {
                return false;
            }
            if(account.id == 200)
            {
                return false;
            }
            try
            {
                var result = await _context.Accounts.FirstOrDefaultAsync(a => a.id == account.id);
                if (result != null)  //Update
                {
                    try
                    {
                        result.password = account.password;
                        result.password_required = account.password_required;

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating account. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from account list. " + ex);
                return false;
            }
            return true;

        }
    }
}
