using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class ChannelInfo
    {
        public ChannelInfo()
        {

        }

        private long deviceID;
        private int channelNumber = 0;
        private string name = "";
        private int status = 0;
        private string callTo = "";
        private string callFrom = "";
        private string callerName = "";
        private string originating = "0";
        private string receiving = "0";
        private string codec = "";

        [DataMember][Key]
        public long DeviceID 
        {
            get
            {
                return deviceID;
            }

            set
            {
                deviceID = value;
            }
        }

        [DataMember]
        public int ChannelNumber
        {
            get
            {
                return channelNumber;
            }

            set
            {
                channelNumber = value;
            }
        }

        [DataMember]
        public String Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        [DataMember]
        public int Status 
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

        [DataMember]
        public string CallFrom 
        {
            get
            {
                return callFrom;
            }

            set
            {
                callFrom = value;
            }
        }
        [DataMember]
        public string CallTo
        {
            get
            {
                return callTo;
            }

            set
            {
                callTo = value;
            }
        }
        [DataMember]
        public string CallerName
        {
            get
            {
                return callerName;
            }

            set
            {
                callerName = value;
            }
        }
        [DataMember]
        public string Originating
        {
            get
            {
                return originating;
            }

            set
            {
                originating = value;
            }
        }
        [DataMember]
        public string Receiving
        {
            get
            {
                return receiving;
            }

            set
            {
                receiving = value;
            }
        }
        [DataMember]
        public string Codec
        {
            get
            {
                return codec;
            }

            set
            {
                codec = value;
            }
        }

        [DataMember]
        public string SourceForDest { get; set; }
    }
}
