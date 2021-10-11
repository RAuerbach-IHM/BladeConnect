using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.DTOModels
{
    public class MacroElementDTO
    {
        public long ID { get; set; }
        public int Index { get; set; }
        public long Macro_Button_Id { get; set; }

        public int Type { get; set; }
        public long Device_ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Call_From { get; set; }

        public string Call_To { get; set; }

        public int Channel { get; set; }

        public string Channel_ID { get; set; }

        public string Status { get; set; }

        public string Audio_Send_To_Dest { get; set; }

        public string Audio_Receive_From_Source { get; set; }

        public string Audio_Receive_From_Source_B { get; set; }
    }
}
