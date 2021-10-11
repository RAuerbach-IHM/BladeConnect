using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.NetworkServices
{
    public class ParseDataReceivedService
    {
        public ParseDataReceivedService()
        {


        }
        public void Parse(long senderID, string message)
        {
            /*     if (message.StartsWith("<NAK"))
                     return;

                 Console.WriteLine("TCP Received: " + message);

                 bool result = false;
                 string[] events;
                 List<BladeIO> bladeIOs = new List<BladeIO>();

                 //<DSTEVENT:xxxxxx|Name:yyyy>

                 if (message.StartsWith("<DSTEVENT") || message.StartsWith("<SRCEVENT"))
                 {
                     events = message.Split('<');

                     foreach (string evt in events)
                     {
                         Console.WriteLine(evt);
                         BladeIO bladeIO = new BladeIO();

                         if (message.StartsWith("<DSTEVENT"))
                             bladeIO.Type = "Dest";
                         if (message.StartsWith("<SRCEVENT"))
                             bladeIO.Type = "Source";

                         string[] newEvt = evt.Split('>');
                         if (newEvt.Length > 0)
                         {
                             string[] inputs = newEvt[0].Split('|');

                             if (inputs.Length > 1)
                             {
                                 string[] input_0 = inputs[0].Split(':');

                                 if (input_0.Length > 1)
                                     bladeIO.WNID = input_0[1];

                                 string[] input_1 = inputs[1].Split(',');

                                 foreach (string item in input_1)
                                 {
                                     string[] input_1a = item.Split(':');

                                     if (input_1a[0] == "NAME")
                                         bladeIO.Name = input_1a[1];

                                     if (input_1a[0] == "LOCATION")
                                         bladeIO.Location = input_1a[1];

                                     if (input_1a[0] == "SRC")
                                         bladeIO.Source = input_1a[1];
                                 }
                             }

                         }
                         if (string.IsNullOrEmpty(bladeIO.WNID))
                             continue;

                         bladeIOs.Add(bladeIO);
                     }

                     BladeIOUpdateReceivedMessage bladeIOUpdateReceivedMessage = new BladeIOUpdateReceivedMessage(bladeIOs);
                     Messenger.Default.Send(bladeIOUpdateReceivedMessage);
                 }
                   */
        }

    }
}