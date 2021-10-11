using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SIPCallDetails : ObservableObject
    {
        public SIPCallDetails()
        {

        }
        private string callFrom = "";
        private string callTo = "";
        private string callerName = "";
        private string originating = "";
        private string receiving = "";
        private string codec = "";

        public string CallFrom
        {
            get
            {
                return callFrom ;
            }

            set
            {
                callFrom = value;
                RaisePropertyChanged(() => CallFrom);
            }
        }
        public string CallTo
        {
            get
            {
                return callTo;
            }

            set
            {
                callTo = value;
                RaisePropertyChanged(() => CallTo);
            }
        }
        public string CallerName
        {
            get
            {
                return callerName;
            }

            set
            {
                callerName = value;
                RaisePropertyChanged(() => CallerName);
            }
        }
        public string Originating
        {
            get
            {
                return originating;
            }

            set
            {
                originating = value;
                RaisePropertyChanged(() => Originating);
            }
        }
        public string Receiving
        {
            get
            {
                return receiving;
            }

            set
            {
                receiving = value;
                RaisePropertyChanged(() => Receiving);
            }
        }
        public string Codec
        {
            get
            {
                return codec;
            }

            set
            {
                codec = value;
                RaisePropertyChanged(() => Codec);
            }
        }
    }
}
