using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CompositeButton : ViewModelBase
    {
        private SBButton sbButton;
        private MacroButton macroButton;
        private SBButton xyRoutePanel;
        private int index;
        private int type;

        public SBButton SBButton
        {
            get
            {
                return sbButton;
            }

            set
            {
                sbButton = value;
                RaisePropertyChanged(() => SBButton);
            }
        }
        public MacroButton MacroButton
        {
            get
            {
                return macroButton;
            }

            set
            {
                macroButton = value;
                RaisePropertyChanged(() => MacroButton);
            }
        }
        public SBButton XYRoutePanel
        {
            get
            {
                return xyRoutePanel;
            }

            set
            {
                xyRoutePanel = value;
                RaisePropertyChanged(() => XYRoutePanel);
            }
        }
        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
                RaisePropertyChanged(() => Index);
            }
        }
        public int Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                RaisePropertyChanged(() => Type);
            }
        }
    }
}
