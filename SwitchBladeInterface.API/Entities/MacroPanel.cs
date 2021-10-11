using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Entities
{
    public class MacroPanel
    {
        public long ID { get; set; }
        public int Index_Value { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public long Station_Id { get; set; }

        public DateTime Start_Time { get; set; }

        public DateTime End_Time { get; set; }
    }
}