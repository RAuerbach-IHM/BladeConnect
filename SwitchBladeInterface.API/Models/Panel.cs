using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Models
{
    public class Panel
    {
        public Panel()
        {
            id = DateTime.Now.Ticks;
            index = 0;
            station_id = -1;
            device_id = -1;
            name = "New Panel";
            isConnected = false;
            color = "Transparent";
            callFrom = "";
            callFromDisplay = "";
            callTo = "";
            startTime = DateTime.Now;
            endTime = DateTime.Now.AddDays(1);
            wnipDevice = new WNIPDevice();
            description = "";
            channel = 0;
            channelID = "";
            status = "";
            audioSendToDest = "";
            audioReceiveFromSource = "";
            audioReceiveFromSourceB = "";
            audioSendToDest_Display = "";
            audioReceiveFromSource_Display = "";
            audioReceiveFromSource_DisplayB = "";
            bladeIOSourceForDest = "";
            bladeIOSourceForDest_Display = "";
        }

        public Panel(Panel panel)
        {
            id = panel.ID;
            index = panel.Index;
            station_id = panel.Station_ID;
            device_id = panel.Device_ID;
            name = panel.Name;
            isConnected = panel.IsConnected;
            color = panel.Color;
            callFrom = panel.CallFrom;
            callFromDisplay = panel.CallFromDisplay;
            callTo = panel.CallTo;
            startTime = panel.StartTime;
            endTime = panel.EndTime;
            wnipDevice = panel.WNIPDevice;
            description = panel.Description;
            channel = panel.Channel;
            channelID = panel.ChannelID;
            status = panel.Status;
            audioSendToDest = panel.AudioSendToDest;
            audioReceiveFromSource = panel.AudioReceiveFromSource;
            audioReceiveFromSourceB = panel.AudioReceiveFromSourceB;
            audioSendToDest_Display = panel.AudioSendToDest_Display;
            audioReceiveFromSource_Display = panel.AudioReceiveFromSource_Display;
            audioReceiveFromSource_DisplayB = panel.AudioReceiveFromSource_DisplayB;
            bladeIOSourceForDest = panel.BladeIOSourceForDest;
            bladeIOSourceForDest_Display = panel.BladeIOSourceForDest_Display;
        }

        private long id;
        private int index = 0;
        private int type = 0;
        private long station_id;
        private long device_id;
        private string name = "";
        private bool isConnected = false;
        private string color = "";
        private string callFrom = "";
        private string callFromDisplay = "";
        private string callTo = "";
        private DateTime startTime;
        private DateTime endTime;
        private WNIPDevice wnipDevice;
        private string description = "";
        private int channel = -1;
        private string channelID = "";
        private string status = "";
        private SIPCallDetails sipCallDetails = new SIPCallDetails();
        private string audioSendToDest = "";
        private string audioReceiveFromSource = "";
        private string audioReceiveFromSourceB = "";
        private string audioSendToDest_Display = "";
        private string audioReceiveFromSource_Display = "";
        private string audioReceiveFromSource_DisplayB = "";
        private string bladeIOSourceForDest = "";
        private string bladeIOSourceForDest_Display = "";

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
        public int Index { get; set; }

        public int Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                //RaisePropertyChanged(() => Type);
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
                //RaisePropertyChanged(() => Station_ID);
            }
        }
        public long Device_ID
        {
            get
            {
                return device_id;
            }

            set
            {
                device_id = value;
                //RaisePropertyChanged(() => Device_ID);
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
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                //RaisePropertyChanged(() => Description);
            }
        }
        public WNIPDevice WNIPDevice
        {
            get
            {
                return wnipDevice;
            }

            set
            {
                wnipDevice = value;
                //RaisePropertyChanged(() => WNIPDevice);
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
                //RaisePropertyChanged(() => IsConnected);
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
                //RaisePropertyChanged(() => Color);
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
                ///RaisePropertyChanged(() => CallFrom);
            }
        }

        public string CallFromDisplay
        {
            get
            {
                return callFromDisplay;
            }

            set
            {
                callFromDisplay = value;
                //RaisePropertyChanged(() => CallFromDisplay);
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
                //RaisePropertyChanged(() => CallTo);
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
                //RaisePropertyChanged(() => StartTime);
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
                //RaisePropertyChanged(() => EndTime);
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
                //RaisePropertyChanged(() => Channel);
            }
        }
        public string ChannelID
        {
            get
            {
                return channelID;
            }

            set
            {
                channelID = value;
                //RaisePropertyChanged(() => ChannelID);
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
                //RaisePropertyChanged(() => Status);
            }
        }

        public SIPCallDetails SIPCallDetails
        {
            get
            {
                return sipCallDetails;
            }

            set
            {
                sipCallDetails = value;
                //RaisePropertyChanged(() => SIPCallDetails);
            }
        }
        public string BladeIOSourceForDest
        {
            get
            {
                return bladeIOSourceForDest;
            }

            set
            {
                bladeIOSourceForDest = value;
                //RaisePropertyChanged(() => BladeIOSourceForDest);
            }
        }
        public string BladeIOSourceForDest_Display
        {
            get
            {
                return bladeIOSourceForDest_Display;
            }

            set
            {
                bladeIOSourceForDest_Display = value;
                //RaisePropertyChanged(() => BladeIOSourceForDest_Display);
            }
        }

        public string AudioSendToDest
        {
            get
            {
                return audioSendToDest;
            }

            set
            {
                audioSendToDest = value;
                //RaisePropertyChanged(() => AudioSendToDest);
            }
        }
        public string AudioReceiveFromSource
        {
            get
            {
                return audioReceiveFromSource;
            }

            set
            {
                audioReceiveFromSource = value;
                //RaisePropertyChanged(() => AudioReceiveFromSource);
            }
        }
        public string AudioReceiveFromSourceB
        {
            get
            {
                return audioReceiveFromSourceB;
            }

            set
            {
                audioReceiveFromSourceB = value;
                //RaisePropertyChanged(() => AudioReceiveFromSourceB);
            }
        }
        public string AudioSendToDest_Display
        {
            get
            {
                return audioSendToDest_Display;
            }

            set
            {
                audioSendToDest_Display = value;
                //RaisePropertyChanged(() => AudioSendToDest_Display);
            }
        }
        public string AudioReceiveFromSource_Display
        {
            get
            {
                return audioReceiveFromSource_Display;
            }

            set
            {
                audioReceiveFromSource_Display = value;
                //RaisePropertyChanged(() => AudioReceiveFromSource_Display);
            }
        }
        public string AudioReceiveFromSource_DisplayB
        {
            get
            {
                return audioReceiveFromSource_DisplayB;
            }

            set
            {
                audioReceiveFromSource_DisplayB = value;
                //RaisePropertyChanged(() => AudioReceiveFromSource_DisplayB);
            }
        }
    }
}