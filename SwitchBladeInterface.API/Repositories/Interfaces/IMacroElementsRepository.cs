using SwitchBladeInterface.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IMacroElementsRepository
    {
        public Task<IEnumerable<MacroElement>> GetMacroElements(long macroElementId);
        public Task<MacroElement> GetMacroElement(long id);

        public Task<bool> SaveMacroElement(MacroElement macroElement);
        public Task<bool> UpdateMacroElementIndex(long id, int index);
        public Task<string> DeleteMacroElement(long macroElementId);
    }
}
