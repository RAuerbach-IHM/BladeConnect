using SwitchBladeInterface.API.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IPhonebookRepository
    {
        Task<List<PhonebookItem>> GetPhonebook();
        Task<List<PhonebookItem>> GetPhonebookAll();

        Task<List<PhonebookItem>> GetPersonalPhonebook(long accountID);

        Task<bool> SavePersonalPhonebook(long accountId, List<long> phonebookItemIds);

        Task<bool> SavePhonebookItem(PhonebookItem phonebookItem);

        Task<List<PhonebookItem>> DeletePhonebookItem(long phonebookItemToDeleteId);
    }
}
