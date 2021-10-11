using Microsoft.EntityFrameworkCore;
using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories
{
    public class ServiceSettingsRepository : IServiceSettingsRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public ServiceSettingsRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<ServiceSettings> GetServiceSettings()
        {
            try
            {
                return await _context.ServiceSettings
                        .FirstOrDefaultAsync(s => s.id == 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Service Settings - " + ex);
                return new ServiceSettings();
            }
        }
        
        public async Task<bool> SaveServiceSettings(ServiceSettings serviceSettings)
        {
            if (serviceSettings == null)
            {
                return false;
            }
            try
            {
                var result = await _context.ServiceSettings.FirstOrDefaultAsync(s => s.id == 1);
                if (result != null)  //Update
                {
                    try
                    {
                        result.ip_address = serviceSettings.ip_address;
                        result.city = serviceSettings.city;
                        result.state = serviceSettings.state;
                        result.market = serviceSettings.market;
                        result.port = serviceSettings.port;
                        result.description = serviceSettings.description;
                        result.public_Register_Url = serviceSettings.public_Register_Url;
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating service settings. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        serviceSettings.id = 1;
                        await _context.AddAsync(serviceSettings);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving service settings. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from service settings. " + ex);
                return false;
            }
            return true;

        }

        public async Task<bool> SaveServiceSettingsSystem(ServiceSettings serviceSettings)
        {
            if (serviceSettings == null)
            {
                return false;
            }
            try
            {
                var result = await _context.ServiceSettings.FirstOrDefaultAsync(s => s.id == 1);
                if (result != null)  //Update
                {
                    try
                    {
                        result.version = serviceSettings.version;
                        result.build = serviceSettings.build;
                        
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating service settings. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        await _context.AddAsync(serviceSettings);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving service settings. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from service settings. " + ex);
                return false;
            }
            return true;

        }
    }
}
