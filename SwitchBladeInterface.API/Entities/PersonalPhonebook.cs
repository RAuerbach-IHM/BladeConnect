using System.ComponentModel.DataAnnotations;

namespace SwitchBladeInterface.API.Entities
{
    public class PersonalPhonebook
    {
        [Key]
        public int id { get; set; }
        public long account_id { get; set; }
        public long phone_book_id { get; set; }
    }
}
