
using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using SwitchBladeInterface.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SwitchBladeInterface.API.Repositories
{
    public class PanelsRepository : IPanelsRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public PanelsRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<IEnumerable<Panel>> GetPanels(long stationId)
        {
            try
            {
                return await _context.Panels
                            .Where(p => p.Station_ID == stationId)
                            .OrderBy(p => p.Index_Value).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Panels - " + ex);
                return new List<Panel>();
            }
        }

        public async Task<Panel> GetPanel(long id)
        {
            try
            {
                return await _context.Panels.FirstOrDefaultAsync(p => p.ID == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Panel - " + ex);
                return new Panel();
            }
        }
        
        public async Task<bool> Save()
        {
            try
            {
                return (await _context.SaveChangesAsync() >= 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Saving Panel - " + ex);
                return false;
            }
        }
        public async Task<bool> SavePanel(Panel panel)
        {
            if (panel == null)
            {
                return false;
            }
            try
            {
                var result = await _context.Panels.FirstOrDefaultAsync(p => p.ID == panel.ID);
                if (result != null)  //Update
                {
                    try
                    {
                        result.Audio_Receive_From_Source = panel.Audio_Receive_From_Source;
                        result.Audio_Receive_From_Source_B = panel.Audio_Receive_From_Source_B;
                        result.Audio_Send_To_Dest = panel.Audio_Send_To_Dest;
                        result.Call_From = panel.Call_From;
                        result.Call_To = panel.Call_To;
                        result.Channel = panel.Channel;
                        result.Channel_ID = panel.Channel_ID;
                        result.Color = panel.Color;
                        result.Description = panel.Description;
                        result.End_Time = panel.End_Time;
                        result.ID = panel.ID;
                        result.Index_Value = panel.Index_Value;
                        result.Name = panel.Name;
                        result.Start_Time = panel.Start_Time;
                        result.Station_ID = panel.Station_ID;
                        result.Type = panel.Type;
                        result.WNIP_Device_ID = panel.WNIP_Device_ID;

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating panel. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        await _context.AddAsync(panel);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving panel. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from panels. " + ex);
                return false;
            }
            return true;

        }

        public async Task<string> DeletePanel(long panelToDeleteId)
        {
            try
            {
                _context.Remove(await _context.Panels.FirstAsync(p => p.ID == panelToDeleteId));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting panel. " + ex);
                return "Error deleting panel";
            }
            return "";
        }
    }
}
