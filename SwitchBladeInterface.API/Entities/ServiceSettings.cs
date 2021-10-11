using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Entities
{
    public class ServiceSettings
    {
        public int id { get; set; }

        public string market { get; set; }

        public string ip_address { get; set; }

        public int port { get; set; }

        public string version { get; set; }

        public string build { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string description { get; set; }

        public string public_Register_Url { get; set; }
    }
}
