using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.DTOModels
{
    public class StationsDTO
    {
        public long ID { get; set; }
   
        public string Name { get; set; }
        
        public string CallLetters { get; set; }
        public string Frequncy { get; set; }
        public string Band { get; set; }

        public string Market { get; set; }        
    }
}
