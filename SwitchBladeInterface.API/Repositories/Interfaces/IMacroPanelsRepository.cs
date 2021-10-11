using SwitchBladeInterface.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IMacroPanelsRepository
    {
        public Task<IEnumerable<MacroPanel>> GetMacroPanels(long stationId);
        public Task<MacroPanel> GetMacroPanel(long id);

        public Task<bool> SaveMacroPanel(MacroPanel macroPanel);
        public Task<string> DeleteMacroPanel(long macroPanelId);
    }
}
