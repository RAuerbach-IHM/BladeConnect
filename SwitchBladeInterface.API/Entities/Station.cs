using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Entities
{
    public class Station
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Call_Letters { get; set; }

        public string Frequency { get; set; }
        public string Band { get; set; }

        public string Market { get; set; }
    }
}
