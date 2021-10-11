using GalaSoft.MvvmLight.Messaging;
using NetworkClientLib.Messages;
using System;

namespace NetworkClientLib
{
    public class ParseUDPListener
    {
        public ParseUDPListener()
        {

        }

        //string input = "";


        public void Parse(long senderID, string message)
        {

            if (message.Contains("BLADE"))
                Console.WriteLine("RECEIVED - " + message);

            //<PHONE:01|Status:01>
            //<PHONE:01|Username:name>
            //<PHONE:01|SIPCallDetails:...>

            string[] input = message.Split('|');
            string[] phone = input[0].Split(':');
            string[] content = input[1].Split(':');
            string[] contentDetails = input[1].Split(',');

            string category = input[0].TrimStart('<');

            int channel;
            bool success = Int32.TryParse(phone[1], out channel);
            if (!success)
                channel = -1;

            string parameter = content[0];

            string value = content[1].TrimEnd('>');

            //if (content[0].StartsWith("SIP"))
            //    ;

            NetworkUDPListenerReceiveMessage networkUDPListenerReceiveMessage = new NetworkUDPListenerReceiveMessage(senderID, category, channel, parameter, value, contentDetails);
            Messenger.Default.Send(networkUDPListenerReceiveMessage);           

        }
    }
}
