using SwitchBladeInterface.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IServiceSettingsRepository
    {
        Task<ServiceSettings> GetServiceSettings();

        Task<bool> SaveServiceSettings(ServiceSettings serviceSettings);
        Task<bool> SaveServiceSettingsSystem(ServiceSettings serviceSettings);
    }
}
