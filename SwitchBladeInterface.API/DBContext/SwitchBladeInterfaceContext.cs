using Microsoft.EntityFrameworkCore;
using SwitchBladeInterface.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.DBContext
{
    public class SwitchBladeInterfaceContext : DbContext
    {
        public SwitchBladeInterfaceContext(DbContextOptions<SwitchBladeInterfaceContext> options)
           : base(options)
        {
        }

        public DbSet<ServiceSettings> ServiceSettings { get; set; }
        public DbSet<Station> Stations { get; set; }
        
        //public DbSet<SBButton> SBButtons { get; set; }
        public DbSet<Panel> Panels { get; set; }
        //public DbSet<MacroButton> MacroButtons { get; set; }
        public DbSet<MacroPanel> MacroPanels { get; set; }

        public DbSet<MacroElement> MacroElements { get; set; }
        public DbSet<Device> Devices { get; set; }

        public DbSet<BladeIO> BladeIOs { get; set; }

        public DbSet<ChannelInfo> ChannelInfo { get; set; }
        public DbSet<PhonebookItem> Phonebook { get; set; }
        public DbSet<PersonalPhonebook> PersonalPhonebooks { get; set; }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Site> Sites { get; set; }

        public DbSet<DisplayEvent> DisplayEvents { get; set; }


        public DbSet<AccountStationsRelations> AccountStationsRelations { get; set; }
        public DbSet<AccountBladeIOsRelations> AccountBladeIOsRelations { get; set; }

        internal Task RemoveAsync(List<PersonalPhonebook> itemsToDelete)
        {
            throw new NotImplementedException();
        }
    }
}
