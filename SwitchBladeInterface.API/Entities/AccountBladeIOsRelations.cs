using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Entities
{
    public class AccountBladeIOsRelations
    {
        [Key]
        public int id { get; set; }
        public String type { get; set; }
        public long account_id { get; set; }
        public long bladeIO_id { get; set; }
    }
}
