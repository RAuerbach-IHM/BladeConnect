using GalaSoft.MvvmLight;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class PhoneBookItem : ObservableObject
    {
        public PhoneBookItem()
        {
            id = DateTime.Now.Ticks;
            name = "";
            description = "";
            sipAddress = "";
        }

        private long id;
        private string name;
        private string description;
        private string sipAddress;
        private bool hideFromPublic;
        private bool isSelected;
        private bool isIncluded;

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
        public string SIPAddress
        {
            get
            {
                return sipAddress;
            }

            set
            {
                sipAddress = value;
                RaisePropertyChanged(() => SIPAddress);
            }
        }
        public bool HideFromPublic
        {
            get
            {
                return hideFromPublic;
            }

            set
            {
                hideFromPublic = value;
                RaisePropertyChanged(() => HideFromPublic);
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
        public bool IsIncluded
        {
            get
            {
                return isIncluded;
            }

            set
            {
                isIncluded = value;
                RaisePropertyChanged(() => IsIncluded);
            }
        }
    }
}
