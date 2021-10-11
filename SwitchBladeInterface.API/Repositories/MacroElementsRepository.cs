using Microsoft.EntityFrameworkCore;
using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories
{
    public class MacroElementsRepository : IMacroElementsRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public MacroElementsRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<IEnumerable<MacroElement>> GetMacroElements(long macroPanelId)
        {
            try
            {
                return await _context.MacroElements
                            .Where(p => p.Macro_Button_Id == macroPanelId)
                            .OrderBy(p => p.Index_Value).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Macro Elements - " + ex);
                return new List<MacroElement>();
            }
        }

        public async Task<MacroElement> GetMacroElement(long id)
        {
            try
            {
                return await _context.MacroElements.FirstOrDefaultAsync(p => p.ID == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving Panel - " + ex);
                return new MacroElement();
            }
        }

        public async Task<bool> SaveMacroElement(MacroElement macroElement)
        {
            if (macroElement == null)
            {
                return false;
            }
            try
            {
                var result = await _context.MacroElements.FirstOrDefaultAsync(p => p.ID == macroElement.ID);
                if (result != null)  //Update
                {
                    try
                    {
                        result.Audio_Receive_From_Source = macroElement.Audio_Receive_From_Source;
                        result.Audio_Receive_From_Source_B = macroElement.Audio_Receive_From_Source_B;
                        result.Audio_Send_To_Dest = macroElement.Audio_Send_To_Dest;
                        result.Call_From = macroElement.Call_From;
                        result.Call_To = macroElement.Call_To;
                        result.Channel = macroElement.Channel;
                        result.Channel_ID = macroElement.Channel_ID;
                        result.Description = macroElement.Description;
                        result.ID = macroElement.ID;
                        result.Index_Value = macroElement.Index_Value;
                        result.Name = macroElement.Name;
                        result.Type = macroElement.Type;
                        result.Device_ID = macroElement.Device_ID;
                        result.Macro_Button_Id = macroElement.Macro_Button_Id;

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating macro element. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        macroElement.Index_Value = 100;
                        await _context.AddAsync(macroElement);
                        await _context.SaveChangesAsync();

                        //Update Indexes
                        await ReindexMacroElements(macroElement.Macro_Button_Id);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving macro element. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from macro elements. " + ex);
                return false;
            }
            return true;

        }

        public async Task<bool> UpdateMacroElementIndex(long id, int index)
        {
            if(id == 0)
            {
                return false;
            }
            try
            {
                var result = await _context.MacroElements.FirstOrDefaultAsync(p => p.ID == id);
                if (result != null)  //Update
                {
                    try
                    {
                        result.Index_Value = index;
                        
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating macro element. " + ex);
                        return false;
                    }
                }
                else  //Not Found
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from macro elements. " + ex);
                return false;
            }
            return true;

        }
        public async Task<string> DeleteMacroElement(long macroElementToDeleteId)
        {
            try
            {
                //GetMacroElement
                var macroElementToBeDeleted = await GetMacroElement(macroElementToDeleteId);

                if(macroElementToBeDeleted == null)
                {
                    Console.WriteLine("Error deleting macro element. Not Found");
                    return "Error - Macro Element Not Found";
                }

                _context.Remove(await _context.MacroElements.FirstAsync(e => e.ID == macroElementToDeleteId));
                await _context.SaveChangesAsync();

                await ReindexMacroElements(macroElementToBeDeleted.Macro_Button_Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting macro element. " + ex);
                return "Error deleting macro element";
            }
            return "";
        }

        private async Task<string> ReindexMacroElements(long macroPanelID)
        {
            try
            {
                //Get Macro Elements
                var macroElements = await GetMacroElements(macroPanelID);
                int index = 0;
                foreach(MacroElement macroElement in macroElements)
                {
                    int newIndex = index;
                    await UpdateMacroElementIndex(macroElement.ID, newIndex);
                    index += 1;
                }

                return "";
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error indexing macro element. " + ex);
                return "Error indexing macro element";
            }
        }
    }
}

