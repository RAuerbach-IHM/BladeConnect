using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace EW
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
            DISCONNECTED = 1001
        }

        public enum BUTTON_TYPE
        {
            [Description("Unknown")]
            NONE = 0,
            [Description("SwitchBlade Button")]
            SB_BUTTON = 1,
            [Description("Macro Button")]
            MACRO_BUTTON = 2,
            [Description("Macro Element")]
            MACRO_ELEMENT = 3,
            [Description("XY Audio Route")]
            ROUTE_XY = 4,
            [Description("X Audio Route")]
            ROUTE_X = 5,
            [Description("X AB Audio Route")]
            ROUTE_X_AB = 6,
            [Description("Macro Audio Route")]
            ROUTE_MACRO = 7,
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
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }
    }
}
