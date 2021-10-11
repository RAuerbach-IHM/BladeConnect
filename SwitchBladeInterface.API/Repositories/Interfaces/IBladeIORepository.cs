using SwitchBladeInterface.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IBladeIORepository
    {
        Task<IEnumerable<BladeIO>> GetBladeIOs(string type);

        Task<IEnumerable<BladeIO>> GetBladeIOsByAccount(string type, long accountId);

        Task<IEnumerable<BladeIO>> GetBladeIOsForSwitchBladeChannels();
        Task<BladeIO> GetBladeIOByWNID(string type, string wnid);

        Task<bool> SavePersonalBladeIOs(string type, long accountId, List<long> bladeIOIds);
    }
}
