using System.ComponentModel;

namespace LightRail.DotNet
{
    /// <summary>
    /// Friendly names for the time of day.
    /// </summary>
    public enum TimeOfDay
    {
        [Description("Early Morning")]
        EarlyMorning,  // 12:00 AM - 5:59 AM
        [Description("Dawn")]
        Dawn,          // 6:00 AM - 6:59 AM
        [Description("Morning")]
        Morning,       // 7:00 AM - 11:59 AM
        [Description("Afternoon")]
        Afternoon,     // 12:00 PM - 4:59 PM
        [Description("Dusk")]
        Dusk,          // 5:00 PM - 5:59 PM
        [Description("Evening")]
        Evening,       // 6:00 PM - 8:59 PM
        [Description("Night")]
        Night,         // 9:00 PM - 11:59 PM
    }
}
