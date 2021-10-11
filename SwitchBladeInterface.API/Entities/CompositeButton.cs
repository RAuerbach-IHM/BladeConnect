
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Entities
{
    public class CompositeButton
    {
        [Key]
        public long Id { get; set; }
        //public SBButton SBButton { get; set; }

        //public MacroButton MacroButton { get; set; }

        //public SBButton XYRoutePanel { get; set; }

        public int Index { get; set; }

        public int Type { get; set; }
    }
}
