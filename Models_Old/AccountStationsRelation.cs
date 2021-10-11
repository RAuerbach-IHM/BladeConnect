using GalaSoft.MvvmLight;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class AccountStationsRelation : ObservableObject
    {
        public AccountStationsRelation()
        {
            id = DateTime.Now.Ticks;
            accountID = -1;
            stationID = -1;
        }
        private long id = 0;
        private long accountID;
        private long stationID;

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

        public long StationID
        {
            get
            {
                return stationID;
            }

            set
            {
                stationID = value;
                RaisePropertyChanged(() => StationID);
            }
        }

    }
}
