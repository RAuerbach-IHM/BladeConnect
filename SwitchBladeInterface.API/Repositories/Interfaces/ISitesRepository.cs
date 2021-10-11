using SwitchBladeInterface.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface ISitesRepository
    {
        public Task<Site> GetSite(int id);
        public Task<IEnumerable<Site>> GetSites(string ipAddress);
        public Task<IEnumerable<Site>> GetSites();
        public Task<bool> SaveSite(Site site);
        public Task<string> DeleteSite(int siteId);
    }
}
