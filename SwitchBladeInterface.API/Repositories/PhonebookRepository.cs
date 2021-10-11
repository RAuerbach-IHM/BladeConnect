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
    public class PhonebookRepository : IPhonebookRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public PhonebookRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<PhonebookItem>> GetPhonebook()
        {
            try
            {
                List<PhonebookItem> result = await _context.Phonebook.Where(c => c.Hide_From_Public == 0).ToListAsync();

                foreach (PhonebookItem item in result)
                {
                    item.Description = item.Description.Trim();
                    item.Name = item.Name.Trim();
                    item.SIP_Address = item.SIP_Address.Trim();
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Phonebook - " + ex);
                return new List<PhonebookItem>();
            }
        }
        
        public async Task<List<PhonebookItem>> GetPhonebookAll()
        {
            try
            {
                List<PhonebookItem> result = new List<PhonebookItem>();

                result = await _context.Phonebook.ToListAsync();

                foreach (PhonebookItem item in result)
                {
                    item.Description = item.Description.Trim();
                    item.Name = item.Name.Trim();
                    item.SIP_Address = item.SIP_Address.Trim();
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Phonebook - " + ex);
                return new List<PhonebookItem>();
            }
        }


        //TODO
        public async Task<List<PhonebookItem>> GetPersonalPhonebook(long accountId)
        {
            try
            {
                //Get Phonebook Item IDs for account
                var phonebookRelations = await _context.PersonalPhonebooks.Where(p => p.account_id == accountId).ToListAsync();
                
                List<long> phonebookRelationIds = new List<long>();
                foreach (PersonalPhonebook relation in phonebookRelations)
                {
                    phonebookRelationIds.Add(relation.phone_book_id);
                }

                List<PhonebookItem> result = new List<PhonebookItem>();

                result = await _context.Phonebook.Where(b => phonebookRelationIds .Contains(b.ID)).ToListAsync();

                foreach (PhonebookItem item in result)
                {
                    item.Description = item.Description.Trim();
                    item.Name = item.Name.Trim();
                    item.SIP_Address = item.SIP_Address.Trim();
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Phonebook - " + ex);
                return new List<PhonebookItem>();
            }
        }

       
        public async Task<bool> SavePhonebookItem(PhonebookItem phonebookItem)
        {
            if(phonebookItem == null)
            {
                return false;
            }
            try
            {
                var result = await _context.Phonebook.FirstOrDefaultAsync(b => b.ID == phonebookItem.ID);
                if (result != null)  //Update
                {
                    try
                    {
                        result.Description = phonebookItem.Description;
                        result.Hide_From_Public = phonebookItem.Hide_From_Public;
                        result.Name = phonebookItem.Name;
                        result.SIP_Address = phonebookItem.SIP_Address;

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating phonebook item. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        await _context.AddAsync(phonebookItem);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving phonebook item. " + ex);
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error retrieving data from phonebook. " + ex);
                return false;
            }
            return true;

        }

        public async Task<bool> SavePersonalPhonebook(long accountId,  List<long> phonebookItemIds)
        {
            if (phonebookItemIds == null)
            {
                return false;
            }

            try
            {
                //Delete all
                var itemsToDelete = await _context.PersonalPhonebooks.Where(p => p.account_id == accountId).ToListAsync();

                foreach(PersonalPhonebook itemToDelete in itemsToDelete)
                {
                    _context.Remove(itemToDelete);
                }

                foreach (long phonebookItemId in phonebookItemIds) {
                    try
                    {
                        PersonalPhonebook personalPhonebookItem = new PersonalPhonebook
                        {
                            phone_book_id = phonebookItemId,
                            account_id = accountId
                        };

                        await _context.AddAsync(personalPhonebookItem);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving personal phonebook item. " + ex);
                        return false;
                    }
                }

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from personal phonebook. " + ex);
                return false;
            }
            return true;

        }


        public async Task<List<PhonebookItem>> DeletePhonebookItem(long phonebookItemToDeleteId)
        {
            try
            {
                _context.Remove(await _context.Phonebook.FirstAsync(i => i.ID == phonebookItemToDeleteId));
                await _context.SaveChangesAsync();
            } catch(Exception ex)
            {
                Console.WriteLine("Error deleting phonebook item. " + ex);
                return await GetPhonebook();
            }
            return await GetPhonebook();
        }
    }
}
