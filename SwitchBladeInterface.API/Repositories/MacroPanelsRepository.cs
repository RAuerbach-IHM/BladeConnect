using Microsoft.EntityFrameworkCore;
using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SwitchBladeInterface.API.Repositories.Interfaces;

namespace SwitchBladeInterface.API.Repositories
{
    public class MacroPanelsRepository : IMacroPanelsRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public MacroPanelsRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<IEnumerable<MacroPanel>> GetMacroPanels(long stationId)
        {
            try
            {
                return await _context.MacroPanels
                            .Where(p => p.Station_Id == stationId)
                            .OrderBy(p => p.Index_Value).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Macro Panels - " + ex);
                return new List<MacroPanel>();
            }
        }

        public async Task<MacroPanel> GetMacroPanel(long id)
        {
            try
            {
                return await _context.MacroPanels.FirstOrDefaultAsync(p => p.ID == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Macro Panel - " + ex);
                return new MacroPanel();
            }
        }

        public async Task<bool> SaveMacroPanel(MacroPanel macroPanel)
        {
            if (macroPanel == null)
            {
                return false;
            }
            try
            {
                var result = await _context.MacroPanels.FirstOrDefaultAsync(p => p.ID == macroPanel.ID);
                if (result != null)  //Update
                {
                    try
                    {
                        result.Color = macroPanel.Color;
                        result.End_Time = macroPanel.End_Time;
                        result.ID = macroPanel.ID;
                        result.Index_Value = macroPanel.Index_Value;
                        result.Name = macroPanel.Name;
                        result.Start_Time = macroPanel.Start_Time;
                        result.Station_Id = macroPanel.Station_Id;
                        result.Description = macroPanel.Description;

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating macro panel. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        await _context.AddAsync(macroPanel);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving macro panel. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from macro panels. " + ex);
                return false;
            }
            return true;

        }

        public async Task<string> DeleteMacroPanel(long macroPanelToDeleteId)
        {
            try
            {
                //Delete Macro Panel
                _context.Remove(await _context.MacroPanels.FirstAsync(p => p.ID == macroPanelToDeleteId));

                //Delete MacroElements
                var macroElements = await _context.MacroElements.Where(e => e.Macro_Button_Id == macroPanelToDeleteId).ToListAsync();
                foreach (MacroElement  macroElement in macroElements)
                {
                    _context.Remove(macroElement);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting macro panel. " + ex);
                return "Error deleting macro panel";
            }
            return "";
        }

    }
}
