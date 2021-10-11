using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Entities
{
    public class Token
    {
        public long id { get; set; }

        public string user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        public Int64 expiration { get; set; }

        public long account_id { get; set; }
        public int role { get; set; }
    }
}
