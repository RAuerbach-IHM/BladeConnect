using SwitchBladeInterface.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IAccountsRepository
    {
        Task<Account> GetAccount(String username);
        Task<List<Account>> GetAccounts();
        Task<bool> SaveAccount(Account account);
        Task<bool> SaveAccountStationsRelations(long accountId, List<long> relationIds);
        Task<bool> SetAccountPassword(Account account);
        Task<List<Account>> DeleteAccount(long accountId);
    }
}
