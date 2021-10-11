using Models;
using System.Collections.Generic;

namespace NetworkClientLib.Messages
{
    public class BladeIOUpdateReceivedMessage
    {
        public BladeIOUpdateReceivedMessage(List<BladeIO> bladeIOs)
        {
            BladeIOs = bladeIOs;
        }

        public List<BladeIO> BladeIOs
        {
            get;

            private set;
        }
    }
}
