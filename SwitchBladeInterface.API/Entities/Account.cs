using System;
using System.ComponentModel.DataAnnotations;

namespace SwitchBladeInterface.API.Entities
{
    public class Account
    {
        [Key]
        public long id { get; set; }

        public int role { get; set;  }

        public String first_name { get; set; }

        public String last_name { get; set; }
        public String user_name { get; set; }

        public byte[] password { get; set; }

        public int password_required { get; set; }
        public int show_public_phonebook { get; set; }
        public int show_personal_phonebook { get; set; }
    }
}
