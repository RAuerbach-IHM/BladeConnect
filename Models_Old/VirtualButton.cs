using GalaSoft.MvvmLight;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class VirtualButton : ObservableObject
    {
        public VirtualButton()
        {
            id = DateTime.Now.Ticks;
            name = "New Button";
            isConnected = false;
            color = "SteelBlue";
            callFrom = "";
            callTo = "";
            startTime = DateTime.Now;
            endTime = DateTime.Now.AddDays(1);
            ipAddress = "";
            port = 0;
            description = "";
            channel = 0;
            status = "";
            originatingSBButtonName = "";
            originatingSBButtonID = 0;
        }

        public VirtualButton(VirtualButton vb)
        {
            id = vb.ID;
            name = vb.Name;
            isConnected = vb.IsConnected;
            color = vb.Color;
            callFrom = vb.CallFrom;
            callTo = vb.callTo;
            startTime = vb.StartTime;
            endTime = vb.EndTime;
            ipAddress = vb.IPAddress;
            port = vb.Port;
            description = vb.Description;
            channel = vb.Channel;
            status = vb.Status;
            originatingSBButtonName = vb.OriginatingSBButtonName;
            originatingSBButtonID = vb.OriginatingSBButtonID;
        }

        //Get Virtual Button from SBButton
        public VirtualButton(SBButton sbButton, string connectionIPAddress, int connectionPort)
        {
            id = DateTime.Now.Ticks;
            name = sbButton.Name;
            isConnected = sbButton.IsConnected;
            color = sbButton.Color;
            callFrom = sbButton.CallFrom;
            callTo = sbButton.CallTo;
            startTime = sbButton.StartTime;
            endTime = sbButton.EndTime;
            ipAddress = connectionIPAddress;
            port = connectionPort;
            description = sbButton.Description;
            channel = sbButton.Channel;
            status = sbButton.Status;
            originatingSBButtonName = sbButton.Name;
            originatingSBButtonID = sbButton.ID;
        }

        private long id;
        private string name = "";
        private bool isConnected = false;
        private string color = "";
        private string callFrom = "";
        private string callTo = "";
        private DateTime startTime;
        private DateTime endTime;
        private string ipAddress;
        private int port;
        private string description = "";
        private int channel;
        private string status = "";
        private string originatingSBButtonName = "";
        private long originatingSBButtonID;

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

        public string OriginatingSBButtonName
        {
            get
            {
                return originatingSBButtonName;
            }

            set
            {
                originatingSBButtonName = value;
                RaisePropertyChanged(() => OriginatingSBButtonName);
            }
        }

        public long OriginatingSBButtonID
        {
            get
            {
                return originatingSBButtonID;
            }

            set
            {
                originatingSBButtonID = value;
                RaisePropertyChanged(() => OriginatingSBButtonID);
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

        public string IPAddress
        {
            get
            {
                return ipAddress;
            }

            set
            {
                ipAddress = value;
                RaisePropertyChanged(() => IPAddress);
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
                RaisePropertyChanged(() => Port);
            }
        }

        public bool IsConnected
        {
            get
            {
                return isConnected;
            }

            set
            {
                isConnected = value;
                RaisePropertyChanged(() => IsConnected);
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

        public string CallFrom
        {
            get
            {
                return callFrom;
            }

            set
            {
                callFrom = value;
                RaisePropertyChanged(() => CallFrom);
            }
        }

        public string CallTo
        {
            get
            {
                return callTo;
            }

            set
            {
                callTo = value;
                RaisePropertyChanged(() => CallTo);
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

        public int Channel
        {
            get
            {
                return channel;
            }

            set
            {
                channel = value;
                RaisePropertyChanged(() => Channel);
            }
        }

        
        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }
        
    }
}