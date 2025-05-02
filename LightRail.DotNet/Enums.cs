using System.ComponentModel;

namespace LightRail.DotNet
{
    /// <summary>
    /// Friendly names for the time of day.
    /// EarlyMorning = 12:00 AM - 5:59 AM
    /// Dawn = 6:00 AM - 6:59 AM
    /// Morning = 7:00 AM - 11:59 AM
    /// Afternoon = 12:00 PM - 4:59 PM
    /// Dusk = 5:00 PM - 5:59 PM
    /// Evening = 6:00 PM - 8:59 PM
    /// Night = 9:00 PM - 11:59 PM
    /// </summary>
    public enum TimeOfDay
    {
        [Description("Early Morning")] EarlyMorning, // 12:00 AM - 5:59 AM
        [Description("Dawn")] Dawn, // 6:00 AM - 6:59 AM
        [Description("Morning")] Morning, // 7:00 AM - 11:59 AM
        [Description("Afternoon")] Afternoon, // 12:00 PM - 4:59 PM
        [Description("Dusk")] Dusk, // 5:00 PM - 5:59 PM
        [Description("Evening")] Evening, // 6:00 PM - 8:59 PM
        [Description("Night")] Night, // 9:00 PM - 11:59 PM
    }

    /// <summary>
    /// Represents the logging levels used in the application.
    /// </summary>
    public enum LoggingLevel
    {
        Verbose = 0,
        Debug = 1,
        Information = 2,
        Warning = 3,
        Error = 4,
        Fatal = 5
    }

    /// <summary>
    /// Represents the intervals at which log files can be rolled over.
    /// </summary>
    public enum LogRollInterval
    {
        Indefinite = 0,
        Yearly = 1,
        Monthly = 2,
        Daily = 3,
        Hourly = 4
    }
}