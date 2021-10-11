using System.ComponentModel.DataAnnotations;

namespace SwitchBladeInterface.API.Entities
{
    public class DisplayEvent
    {
        [Key]
        public long Id { get; set; }

        public string Display_command { get; set; }

        public string Label { get; set; }
        public int Hidden { get; set; }

        public int Site_id { get; set; }
        public int Display_Event_id { get; set; }
    }
}
