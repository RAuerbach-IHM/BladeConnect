using SwitchBladeInterface.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IDisplayEventsRepository
    {
        Task<DisplayEvent> GetDisplayEvent(long Id);
        Task<DisplayEvent> GetDisplayEvent(int id);
        public Task<DisplayEvent> GetDisplayEventBySiteId(long id, int siteId);
        Task<IEnumerable<DisplayEvent>> GetDisplayEvents(bool includeHidden);
        Task<IEnumerable<DisplayEvent>> GetDisplayEventsBySiteId(int siteId, bool includeHidden);
        Task<bool> SaveDisplayEvent(DisplayEvent displayEvent);
        public Task<string> DeleteDisplayEvent(long id);
        public Task<string> DeleteDisplayEventsBySite(int siteId);
    }
}
