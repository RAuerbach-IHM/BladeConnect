using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Entities
{
    public class ChannelInfo
    {
        [Key]
        public long id { get; set; }

        public long Device_ID { get; set; }
        
        public int Channel_Number { get; set; }
        
        public string Name { get; set; }
      
        public int Status { get; set; }

        public string Call_From { get; set; }
       
        public string Call_To { get; set; }

        public string Caller_Name { get; set; }
       
        public string Originating { get; set; }
       
        public string Receiving { get; set; }
       
        public string Codec { get; set; }
    }
}