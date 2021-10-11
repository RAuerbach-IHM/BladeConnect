using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Entities
{
    public class AccountStationsRelations
    {
        [Key]
        public int id { get; set; }
        public long account_id {get; set;}
        public long station_id{get; set;}
    }
}
