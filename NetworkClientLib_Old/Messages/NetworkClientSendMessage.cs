using Models;

namespace NetworkClientLib.Messages
{
    public class NetworkClientSendMessage
    {
        public NetworkClientSendMessage(string message, WNIPDevice wnipDevice)
        {
            Message = message;
            WNIPDevice = wnipDevice;
        }
        public string Message
        {
            get;

            private set;
        }
        public WNIPDevice WNIPDevice
        {
            get;

            private set;
        }
    }
}


