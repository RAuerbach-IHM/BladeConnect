using GalaSoft.MvvmLight;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Call : ObservableObject
    {
        public Call()
        {

        }

        private double id = 0;
        private string name = "";
        private string sipName = "";

        [Key]
        public double ID  //0 - 16  (0 = None)
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
        public String Name  //0 - 16  (0 = None)
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
        public String SIPName  //0 - 16  (0 = None)
        {
            get
            {
                return sipName;
            }

            set
            {
                sipName = value;
                RaisePropertyChanged(() => SIPName);
            }
        }
    }
}
