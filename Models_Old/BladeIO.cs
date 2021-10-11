using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BladeIO : ObservableObject
    {

        private long id = 0;
        private string wnID = "";
        private string name = "";
        private string bladeName = "";
        private string bladeID = "";
        private string type = "Dest";
        private string location = "";
        private string source = "";
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
        public string  WNID
        {
            get
            {
                return wnID;
            }

            set
            {
                wnID = value;
                RaisePropertyChanged(() => WNID);
            }
        }
        public String Name 
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
        public String Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
                RaisePropertyChanged(() => Location);
            }
        }
        public String Source
        {
            get
            {
                return source;
            }

            set
            {
                source = value;
                RaisePropertyChanged(() => Source);
            }
        }
        public String BladeName
        {
            get
            {
                return bladeName;
            }

            set
            {
                bladeName = value;
                RaisePropertyChanged(() => BladeName);
            }
        }
        public String BladeID
        {
            get
            {
                return bladeID;
            }

            set
            {
                bladeID = value;
                RaisePropertyChanged(() => BladeID);
            }
        }
        public String Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                RaisePropertyChanged(() => Type);
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
