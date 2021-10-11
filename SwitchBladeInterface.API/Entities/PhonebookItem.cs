using System.ComponentModel.DataAnnotations;

namespace SwitchBladeInterface.API.Entities
{
    public class PhonebookItem
    {
        [Key]
        public long ID { get; set; }
        
        public string Name { get; set; } 
        public string Description { get; set; }
       
        public string SIP_Address { get; set; }
        
        public int Hide_From_Public { get; set; }
     
        //public bool IsSelected { get; set; }
       
        //public bool IsIncluded { get; set; }

    }
}
