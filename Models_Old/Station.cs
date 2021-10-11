using GalaSoft.MvvmLight;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Station : ObservableObject
    {
        public Station()
        {
            id = DateTime.Now.Ticks;
            name = "New Station";
            market = "";
            callLetters = "";
        }

        public Station(Station newStation)
        {
            CallLetters = newStation.CallLetters;
            ID = newStation.ID;
            IsSelected = newStation.IsSelected;
            Market = newStation.Market;
            Name = newStation.Name;
        }

        private long id = 0;
        private string name = "New Station";
        private string market = "";
        private string callLetters = "";
        private bool isSelected = false;

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

        public string Name  //0 - 16  (0 = None)
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

        public string CallLetters  //0 - 16  (0 = None)
        {
            get
            {
                return callLetters;
            }

            set
            {
                callLetters = value;
                RaisePropertyChanged(() => CallLetters);
            }
        }

        public string Market  //0 - 16  (0 = None)
        {
            get
            {
                return market;
            }

            set
            {
                market = value;
                RaisePropertyChanged(() => Market);
            }
        }
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }
    }
}
