using GalaSoft.MvvmLight;
using System;
using System.ComponentModel.DataAnnotations;
using static EW.Enums;

namespace Models
{
    [Serializable]
    public class RemoteEvent : ObservableObject
    {
        public RemoteEvent()
        {
            ID = DateTime.Now.Ticks;
            name = "Remote Event";
            eventReceived = "";
            sourceID = -1;
            action = REMOTE_EVENT_ACTION.NONE;
            deviceType = DEVICE_TYPE.WHEATNET_BLADE;
        }
        public RemoteEvent(WNIPDevice wnipDevice)
        {
            ID = DateTime.Now.Ticks;
            ID_WNIP = wnipDevice.ID;
            name = GetEnumDescription(wnipDevice.DeviceType) + " Remote Event";
            eventReceived = "";

            if (wnipDevice.DeviceType == DEVICE_TYPE.WHEATNET_BLADE)
            {
                slio = "SLIO 1";
                eventReceived = "SLIOEVENT:1";
            }
            else
            if (wnipDevice.DeviceType == DEVICE_TYPE.WHEATNET_SWITCHBLADE)
            {
                slio = "Opto 1";
                eventReceived = "SLIOEVENT:1";
            }

            deviceType = wnipDevice.DeviceType;
            sourceID = -1;
            action = REMOTE_EVENT_ACTION.NONE;
        }

        public RemoteEvent(RemoteEvent re)
        {
            ID = re.ID;
            ID_WNIP = re.ID_WNIP;
            name = re.Name;
            eventReceived = re.EventReceived;
            slio = re.SLIO;
            sourceID = re.SourceID;
            action = re.Action;
        }

        private string name;
        private string eventReceived;
        private string slio;
        private int sourceID;
        private REMOTE_EVENT_ACTION action;
        private DEVICE_TYPE deviceType;

        [Key]
        public long ID { get; set; }
        public long ID_WNIP { get; set; }

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
        public string SLIO
        {
            get
            {
                return slio;
            }

            set
            {
                slio = value;
                RaisePropertyChanged(() => SLIO);
            }
        }
        public string EventReceived
        {
            get
            {
                return eventReceived;
            }

            set
            {
                eventReceived = value;
                RaisePropertyChanged(() => EventReceived);
            }
        }
        public int SourceID
        {
            get
            {
                return sourceID;
            }

            set
            {
                sourceID = value;
                RaisePropertyChanged(() => SourceID);
            }
        }
        public REMOTE_EVENT_ACTION Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
                RaisePropertyChanged(() => Action);
            }
        }
        public DEVICE_TYPE DeviceType
        {
            get
            {
                return deviceType;
            }

            set
            {
                deviceType = value;
                RaisePropertyChanged(() => DeviceType);
            }
        }
    }
}

