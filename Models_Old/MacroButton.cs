using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MacroButton : ObservableObject
    {
        public MacroButton()
        {
            id = DateTime.Now.Ticks;
            station_id = -1;
            name = "New Button";
            color = "Transparent";
            startTime = DateTime.Now;
            endTime = DateTime.Now.AddDays(1);
            description = "";

            macroElements = new ObservableCollection<MacroElement>();
        }

        public MacroButton(MacroButton macroButton)
        {
            id = macroButton.ID;
            station_id = macroButton.Station_ID;
            name = macroButton.Name;
            color = macroButton.Color;
            startTime = macroButton.StartTime;
            endTime = macroButton.EndTime;
            description = macroButton.Description;

            macroElements = macroButton.MacroElements;
        }

        private long id;
        private long station_id;
        private string name = "";
        private string color = "";
        private DateTime startTime;
        private DateTime endTime;
        private string description = "";

        private ObservableCollection<MacroElement> macroElements = new ObservableCollection<MacroElement>();
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
                RaisePropertyChanged(() => ID);
            }
        }

        public long Station_ID
        {
            get
            {
                return station_id;
            }

            set
            {
                station_id = value;
                RaisePropertyChanged(() => Station_ID);
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
                RaisePropertyChanged(() => Name);
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
                RaisePropertyChanged(() => Description);
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
                RaisePropertyChanged(() => Color);
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
                RaisePropertyChanged(() => StartTime);
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
                RaisePropertyChanged(() => EndTime);
            }
        }


        public ObservableCollection<MacroElement> MacroElements
        {
            get
            {
                return macroElements;
            }

            set
            {
                macroElements = value;
                RaisePropertyChanged(() => MacroElements);
            }
        }
    }
}