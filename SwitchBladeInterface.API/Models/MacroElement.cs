using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Models
{
    public class MacroElement
    {
        public MacroElement()
        {
            id = DateTime.Now.Ticks;
            index = 0;
            device_id = -1;
            name = "New Macro Button Element";
            isConnected = false;
            callFrom = "";
            callFromDisplay = "";
            callTo = "";
            wnipDevice = new WNIPDevice();
            description = "";
            channel = 0;
            channelID = "";
            status = "";
            audioSendToDest = "";
            audioReceiveFromSource = "";
        }

        public MacroElement(MacroElement macroElement)
        {
            id = macroElement.ID;
            index = macroElement.Index;
            device_id = macroElement.Device_ID;
            name = macroElement.Name;
            isConnected = macroElement.IsConnected;
            callFrom = macroElement.CallFrom;
            callFromDisplay = macroElement.CallFromDisplay;
            callTo = macroElement.CallTo;
            wnipDevice = macroElement.WNIPDevice;
            description = macroElement.Description;
            channel = macroElement.Channel;
            channelID = macroElement.ChannelID;
            status = macroElement.Status;
            audioSendToDest = macroElement.AudioSendToDest;
            audioReceiveFromSource = macroElement.AudioReceiveFromSource;
        }

        private int index = 0;
        private long id;
        private long device_id;
        private string name = "";
        private bool isConnected = false;
        private string callFrom = "";
        private string callFromDisplay = "";
        private string callTo = "";
        private WNIPDevice wnipDevice;
        private string description = "";
        private int channel = -1;
        private string channelID = "";
        private string status = "";
        private SIPCallDetails sipCallDetails = new SIPCallDetails();
        private string audioSendToDest = "";
        private string audioReceiveFromSource = "";

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

        public string CallFrom
        {
            get
            {
                return callFrom;
            }

            set
            {
                callFrom = value;
                //RaisePropertyChanged(() => CallFrom);
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
        public int Index { get; set; }
       
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
    }
}