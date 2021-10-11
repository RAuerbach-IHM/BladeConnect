using System;
using System.ComponentModel.DataAnnotations;
using static SwitchBladeInterface.API.Enums.Enums;

namespace SwitchBladeInterface.API.Models
{
    public class WNIPDevice
    {
        public WNIPDevice()
        {
            id = DateTime.Now.Ticks;
            status = true;
            IsDirty = false;
            index = 0;
            manufacturer = "Wheatstone";
            name = "WNIP Device";
            deviceType = DEVICE_TYPE.UNDEFINED;
            ip = "10.0.0.0";
            port = 55776;
            numOptos = 128;
            numRelays = 128;
            wnip = "192.168.87.99";
        }
        private long id = 0;
        private bool status = true;
        private int index = 0;
        private string manufacturer = "";
        private string name = "";
        private DEVICE_TYPE deviceType = DEVICE_TYPE.UNDEFINED;
        private string ip = "";
        private string wnip = "";
        private int port = 0;
        private int numOptos = 0;
        private int numRelays = 0;
        private int sourceID = -1;

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


        public bool Status
        {
            get
            {
                return status;
            }

            set
            {
                if (value != Status)
                    IsDirty = true;

                status = value;
                //RaisePropertyChanged(() => Status);
            }
        }

        public bool IsDirty { get; set; }

        public bool IsSelected { get; set; }

        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
                //RaisePropertyChanged(() => Index);
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

        public DEVICE_TYPE DeviceType
        {
            get
            {
                return deviceType;
            }

            set
            {
                deviceType = value;
                //RaisePropertyChanged(() => DeviceType);
            }
        }

        public string Manufacturer
        {
            get
            {
                return manufacturer;
            }

            set
            {
                manufacturer = value;
                //RaisePropertyChanged(() => Manufacturer);
            }
        }

        public string IP
        {
            get
            {
                return ip;
            }

            set
            {
                ip = value;
                //RaisePropertyChanged(() => IP);
            }
        }

        public string WNIP
        {
            get
            {
                return wnip;
            }

            set
            {
                wnip = value;
                //RaisePropertyChanged(() => WNIP);
            }
        }
        public int Port
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
                //RaisePropertyChanged(() => Port);
            }
        }

        public int NumOptos  //0 - 16  (0 = None)
        {
            get
            {
                return numOptos;
            }

            set
            {
                numOptos = value;
                //RaisePropertyChanged(() => NumOptos);
            }
        }

        public int NumRelays  //0 - 16  (0 = None)
        {
            get
            {
                return numRelays;
            }

            set
            {
                numRelays = value;
                //RaisePropertyChanged(() => NumRelays);
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
                //RaisePropertyChanged(() => SourceID);
            }
        }
    }
}