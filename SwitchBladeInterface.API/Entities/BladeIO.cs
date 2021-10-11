using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Entities
{
    public class BladeIO
    {
        public long ID { get; set; }

        
        public string WN_ID { get; set; }
        
        public String Name { get; set; }
       
        public String Location { get; set; }
       
        public String Source { get; set; }
        
        public String Blade_Name { get; set; }
        
        public String Blade_ID { get; set; }
       
        public String Type { get; set; }
        
        //public bool IsSelected { get; set; }
        
        //public bool IsIncluded { get; set; }
       
    }
}
