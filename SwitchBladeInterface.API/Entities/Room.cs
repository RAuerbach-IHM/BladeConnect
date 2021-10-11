using System.ComponentModel.DataAnnotations;

namespace SwitchBladeInterface.API.Entities
{
    public class Room
    {
        [Key]
        public long ID { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public string Designator { get; set; }

        public int Hidden { get; set; }
        public int Hidden_mic { get; set; }

        public int Site_id { get; set; }
        public int Room_id { get; set; }
    }
}
