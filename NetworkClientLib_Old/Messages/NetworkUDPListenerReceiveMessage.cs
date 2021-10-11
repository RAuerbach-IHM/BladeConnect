namespace NetworkClientLib.Messages
{
    public class NetworkUDPListenerReceiveMessage
    {
        public NetworkUDPListenerReceiveMessage(long senderID, string category, int channel, string parameter, string value, string[] values)
        {
            SenderID = senderID;
            Category = category;
            Channel = channel;
            Parameter = parameter;
            Value = value;
            Values = values;
        }

        public long SenderID
        {
            get;

            private set;
        }
        public string Category
        {
            get;

            private set;
        }
        public int Channel
        {
            get;

            private set;
        }
        public string Parameter
        {
            get;

            private set;
        }
        public string Value
        {
            get;

            private set;
        }
        public string[] Values
        {
            get;

            private set;
        }
    }
}
