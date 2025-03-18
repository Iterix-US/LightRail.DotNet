namespace LightRail.DotNet.Extensions
{
    public static class BooleanExtensions
    {
        public static int ToInteger(this bool boolean)
        {
            return boolean ? 1 : 0;
        }

        public static string ToYesNo(this bool boolean)
        {
            return boolean ? "Yes" : "No";
        }

        public static string ToOneZero(this bool boolean)
        {
            return boolean ? "1" : "0";
        }

        public static string ToTrueFalse(this bool boolean)
        {
            return boolean ? "True" : "False";
        }

        public static string ToOnOff(this bool boolean)
        {
            return boolean ? "On" : "Off";
        }

        public static string ToEnabledDisabled(this bool boolean)
        {
            return boolean ? "Enabled" : "Disabled";
        }

        public static string ToActiveInactive(this bool boolean)
        {
            return boolean ? "Active" : "Inactive";
        }
    }
}
