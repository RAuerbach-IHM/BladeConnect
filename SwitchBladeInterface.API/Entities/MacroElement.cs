using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Entities
{
    public class MacroElement
    {
        public long ID { get; set; }
        public int Index_Value { get; set; }
        public long Macro_Button_Id { get; set; }

        public int Type { get; set; }

        //public long Station_ID { get; set; }

        public long Device_ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //public bool Is_Connected { get; set; }

        //public string Color { get; set; }

        public string Call_From { get; set; }

        public string Call_To { get; set; }

        //public DateTime Start_Time { get; set; }

        //public DateTime End_Time { get; set; }

        public int Channel { get; set; }

        public string Channel_ID { get; set; }

        public string Status { get; set; }

        public string Audio_Send_To_Dest { get; set; }

        public string Audio_Receive_From_Source { get; set; }

        public string Audio_Receive_From_Source_B { get; set; }
    }
}