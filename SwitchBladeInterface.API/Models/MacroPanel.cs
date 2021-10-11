using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Models
{
    public class MacroPanel
    {
        public MacroPanel()
        {
            id = DateTime.Now.Ticks;
            index = 0;
            station_id = -1;
            name = "New Macro Panel";
            color = "Transparent";
            startTime = DateTime.Now;
            endTime = DateTime.Now.AddDays(1);
            description = "";

            macroElements = new List<MacroElement>();
        }

        public MacroPanel(MacroPanel macroPanel)
        {
            id = macroPanel.ID;
            index = macroPanel.Index;
            station_id = macroPanel.Station_ID;
            name = macroPanel.Name;
            color = macroPanel.Color;
            startTime = macroPanel.StartTime;
            endTime = macroPanel.EndTime;
            description = macroPanel.Description;

            macroElements = macroPanel.MacroElements;
        }

        private long id;
        private int index;
        private long station_id;
        private string name = "";
        private string color = "";
        private DateTime startTime;
        private DateTime endTime;
        private string description = "";

        private IEnumerable<MacroElement> macroElements = new List<MacroElement>();
        [Key]
        public long ID
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                //RaisePropertyChanged(() => ID);
            }
        }
        public int Index { get; set; }

        public long Station_ID
        {
            get
            {
                return station_id;
            }

            set
            {
                station_id = value;
                //RaisePropertyChanged(() => Station_ID);
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                //RaisePropertyChanged(() => Name);
            }
        }
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                //RaisePropertyChanged(() => Description);
            }
        }

        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                //RaisePropertyChanged(() => Color);
            }
        }

        public DateTime StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                startTime = value;
                ///RaisePropertyChanged(() => StartTime);
            }
        }
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                endTime = value;
                //RaisePropertyChanged(() => EndTime);
            }
        }


        public IEnumerable<MacroElement> MacroElements
        {
            get
            {
                return macroElements;
            }

            set
            {
                macroElements = value;
                //RaisePropertyChanged(() => MacroElements);
            }
        }
    }
}