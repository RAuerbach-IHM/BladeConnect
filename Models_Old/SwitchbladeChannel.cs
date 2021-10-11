using GalaSoft.MvvmLight;
using System;

namespace Models
{
    public class SwitchbladeChannel : ObservableObject
    {
        public SwitchbladeChannel()
        {

        }
        public SwitchbladeChannel(SwitchbladeChannel sbChannel)
        {
            switchbladeID = sbChannel.SwitchbladeID;
            channelNumber = sbChannel.ChannelNumber;
            channelNumber_Display = sbChannel.ChannelNumber_Display;
            name = sbChannel.Name;
            status = sbChannel.Status;
            sipCallDetails = sbChannel.SIPCallDetails;
            Device = sbChannel.Device;
        }

        private double switchbladeID;
        private int channelNumber = 0;
        private string channelNumber_Display = "1";
        private string name = "";
        private string status = "*";
        private SIPCallDetails sipCallDetails = new SIPCallDetails();

        public WNIPDevice Device { get; set; }
        public Double SwitchbladeID  //0 - 16  (0 = None)
        {
            get
            {
                return switchbladeID;
            }

            set
            {
                switchbladeID = value;
                RaisePropertyChanged(() => SwitchbladeID);
            }
        }

        public int ChannelNumber  //0 - 16  (0 = None)
        {
            get
            {
                return channelNumber;
            }

            set
            {
                channelNumber = value;
                RaisePropertyChanged(() => ChannelNumber);
            }
        }
        public string ChannelNumber_Display  //0 - 16  (0 = None)
        {
            get
            {
                return channelNumber_Display;
            }

            set
            {
                channelNumber_Display = value;
                RaisePropertyChanged(() => ChannelNumber_Display);
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
        public String Status  //0 - 16  (0 = None)
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                RaisePropertyChanged(() => Status);
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
                RaisePropertyChanged(() => SIPCallDetails);
            }
        }
    }
}
