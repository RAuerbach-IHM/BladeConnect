using SwitchBladeInterface.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IPanelsRepository
    {
        public Task<IEnumerable<Panel>> GetPanels(long stationId);
        public Task<Panel> GetPanel(long id);

        public Task<bool> SavePanel(Panel panel);
        public Task<string> DeletePanel(long panelId);

    }
}
