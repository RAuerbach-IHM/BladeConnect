using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Services.DisplayEventServices
{
    public class DisplayEventsService
    {
        private readonly SwitchBladeInterfaceContext _context;
        private IDisplayEventsRepository _displayEventsRepository;

        public DisplayEventsService(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateDefaultDisplayEvents(int siteId)
        {
            bool result = true;
            
            IDisplayEventsRepository displayEventsRepository = new DisplayEventsRepository(_context);
            _displayEventsRepository = displayEventsRepository;

            DisplayEvent displayEvent;

            for (int i = 0; i < 20; i++)
            {
                //Get Default Display Event
                displayEvent = await _displayEventsRepository.GetDisplayEventBySiteId(i + 1, siteId);


                if (displayEvent == null)
                {
                    //Create new DisplayEvent
                    DisplayEvent newDisplayEvent = new DisplayEvent
                    {
                        Display_command = "event" + (i + 1),
                        Label = "Event " + (i + 1),
                        Hidden = 0,
                        Display_Event_id = i + 1,
                        Site_id = siteId
                    };


                    await _displayEventsRepository.SaveDisplayEvent(newDisplayEvent);
                }
            }

            
            //PROD Event
            //Get Default Display Event
            displayEvent = await _displayEventsRepository.GetDisplayEventBySiteId(1000, siteId);

            if (displayEvent == null)
            {
                //Create new DisplayEvent
                DisplayEvent newDisplayEvent = new DisplayEvent
                {
                    Display_command = "prod",
                    Label = "Production",
                    Site_id = siteId,
                    Display_Event_id = 1000
                };

                await _displayEventsRepository.SaveDisplayEvent(newDisplayEvent);
            }
            return result;
        }


        public async Task<bool> DeleteDisplayEventsBySite(int siteId)
        {
            bool result = false;

            IDisplayEventsRepository displayEventsRepository = new DisplayEventsRepository(_context);
            _displayEventsRepository = displayEventsRepository;

            await _displayEventsRepository.DeleteDisplayEventsBySite(siteId);

            return result;
        }
    }
}
