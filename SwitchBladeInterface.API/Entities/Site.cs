using System.ComponentModel.DataAnnotations;

namespace SwitchBladeInterface.API.Entities
{
    public class Site
    {
        [Key]
        public int ID { get; set; }

        public string Site_Code { get; set; }
        public string Market { get; set; }

        public string Ip_Range_Low { get; set; }

        public string Ip_Range_High { get; set; }

        public string State { get; set; }

        public string City { get; set; }
    }
}
