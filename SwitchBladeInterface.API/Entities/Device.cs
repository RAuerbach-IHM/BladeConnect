using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace SwitchBladeInterface.API.Entities
{
    public class Device
    {
        [Key]
        public long ID { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public string Manufacturer { get; set; }

        public string IP_Address { get; set; }
        public string WNIP_Address { get; set; }

        public int Port { get; set; }
        public long Room_Id { get; set; }
        public int Site_Id { get; set; }
        public int Number { get; set; }
    }
}
