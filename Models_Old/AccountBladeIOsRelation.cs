using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AccountBladeIOsRelation : ObservableObject
    {
        public AccountBladeIOsRelation()
        {
            id = DateTime.Now.Ticks;
            accountID = -1;
            bladeIOID = -1;
        }
        private long id = 0;
        private long accountID;
        private long bladeIOID;

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

        public long AccountID
        {
            get
            {
                return accountID;
            }

            set
            {
                accountID = value;
                RaisePropertyChanged(() => AccountID);
            }
        }

        public long BladeIOID
        {
            get
            {
                return bladeIOID;
            }

            set
            {
                bladeIOID = value;
                RaisePropertyChanged(() => BladeIOID);
            }
        }

    }
}
