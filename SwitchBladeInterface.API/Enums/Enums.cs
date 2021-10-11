using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SwitchBladeInterface.API.Enums
{
    public class Enums
    {
        public enum DEVICE_TYPE
        {
            [Description("Undefined")]
            UNDEFINED = 0,
            //[Description("Text File")]
            //TEXT_FILE = 1,
            //[Description("XML File")]
            //XML_FILE = 2,
            //[Description("NexGen")]
            //NEXGEN = 3,
            [Description("Wheatnet Blade")]
            WHEATNET_BLADE = 4,
            [Description("Wheatnet Switchblade")]
            WHEATNET_SWITCHBLADE = 5,
            //[Description("Sealevel")]
            //SEALEVEL = 6,
            [Description("Macro")]
            MACRO = 7,
            [Description("Display")]
            Display = 10,
        }

        public enum ROOM_TYPE
        {
            [Description("Undefined")]
            UNDEFINED = 0,
            [Description("Falcon Studio")]
            FALCON = 1,
            [Description("Atlantis Studio")]
            ATLANTIS = 2,
            [Description("Apollo Studio")]
            APOLLO = 3,
            [Description("Gemini Studio")]
            GEMINI = 4,
            [Description("Mercury Plus Studio")]
            MERCURY_PLUS = 5,
            [Description("Mercury Studio")]
            Mercury = 6,
            [Description("Booth")]
            BOOTH = 7,
            [Description("Conference Room")]
            CONFERENCE = 8,
            [Description("Other")]
            Other = 100,
        }

        public enum DISPLAY_EVENT_OPTION
        {
            [Description("None")]
            NONE = 0,
            [Description("Mic On")]
            MIC_ON = 1,
            [Description("Mic Off")]
            MIC_OFF = 2,
        }

        public enum REMOTE_EVENT_ACTION
        {
            [Description("None")]
            NONE,
            [Description("Log Only")]
            LOG_ONLY,
            [Description("Start")]
            START,
            [Description("Stop")]
            STOP,
            [Description("Start and Stop")]
            START_STOP,
            [Description("Start and Stop (Invert)")]
            START_STOP_INVERT
        }

        public enum CHANNEL_STATUS
        {
            [Description("Unknown")]
            NONE = 0,
            [Description("Idle")]
            IDLE = 1,
            [Description("Busy")]
            BUSY = 2,
            [Description("Ringing In")]
            RINGING_IN = 3,
            [Description("Answered")]
            ANSWERED = 4,
            [Description("On Hold")]
            ON_HOLD = 5,
            [Description("Ringing Out")]
            RINGING_OUT = 6,
            [Description("Recording")]
            RECORDING = 7,
            [Description("File Playing")]
            FILE_PLAYING = 8,
            [Description("File Stopped")]
            FILE_STOPPED = 9,
            [Description("Register Fail")]
            REGISTER_FAIL = 10,
            [Description("Registered")]
            REGISTERED = 11,
            [Description("Unregistered")]
            UNREGISTERED = 12,
            [Description("Not Initialised")]
            NOT_INITIALISED = 13,
            [Description("Initialised")]
            INITIALISED = 14,
            [Description("Hung Up")]
            HUNG_UP = 15,
            [Description("Error")]
            ERROR = 100,
            [Description("Error ")]
            ERROR_2 = 180,
            [Description("Not Found")]
            NOT_FOUND = 1000,
            [Description("Disconnected")]
            DISCONNECTED = 1001,
            [Description("Crosspoint")]
            CROSSPOINT = 2000,
        }

        public enum BUTTON_TYPE
        {
            [Description("Unknown")]
            NONE = 0,
            [Description("SwitchBlade Button")]
            SB_BUTTON = 1,
            [Description("Macro Button")]
            MACRO_BUTTON = 2,
            [Description("Macro Call Element")]
            MACRO_CALL_ELEMENT = 3,
            [Description("XY Audio Route")]
            ROUTE_XY = 4,
            [Description("X Audio Route")]
            ROUTE_X = 5,
            [Description("X AB Audio Route")]
            ROUTE_X_AB = 6,
            [Description("XY Audio Route Macro Element")]
            ROUTE_XY_ELEMENT = 7,
            [Description("X Audio Route Macro Element")]
            ROUTE_X_ELEMENT = 8,
            [Description("X AB Audio Route Macro Element")]
            ROUTE_X_AB_ELEMENT = 9,
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi == null)
                return "";

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            if (description == null)
                throw new ArgumentException();
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).FirstOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }
    }
}
