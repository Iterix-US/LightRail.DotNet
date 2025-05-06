# EnumExtensions

Provides extension methods to enhance working with enums, including value listing, description retrieval, and conversions for logging systems (Serilog and NLog).

---

## 📦 Namespace

`LightRail.DotNet.Extensions`

---

## 🧪 Notes

- The extension methods are static and operate directly on `enum` types.
- Includes useful mapping helpers to Serilog and NLog logging levels.

---

## ✅ Usage

### Get the description of an enum value

```csharp
public enum Status
{
    [Description("Item is active")]
    Active,
    [Description("Item is inactive")]
    Inactive
}

var description = Status.Active.GetDescription(); // "Item is active"
```

### Get all enum values as a list

```csharp
var values = Status.Active.GetAllValues(); // List<Status> containing Active and Inactive
```

### Convert LoggingLevel to Serilog LogEventLevel

```csharp
LoggingLevel level = LoggingLevel.Warning;
var serilogLevel = level.ToSerilogLevel(); // LogEventLevel.Warning
```

### Convert LogRollInterval to Serilog RollingInterval

```csharp
LogRollInterval interval = LogRollInterval.Daily;
var serilogInterval = interval.ToSerilogRollingInterval(); // RollingInterval.Day
```

### Convert LoggingLevel to NLog LogLevel

```csharp
LoggingLevel level = LoggingLevel.Debug;
var nlogLevel = level.ToNLogLevel(); // LogLevel.Debug
```

### Convert LogRollInterval to NLog FileArchivePeriod

```csharp
LogRollInterval interval = LogRollInterval.Monthly;
var nlogArchive = interval.ToNLogArchivePeriod(); // FileArchivePeriod.Month
```

---

## 📘 Related Types

- `LoggingLevel`
- `LogRollInterval`
- `LogEventLevel` (Serilog)
- `LogLevel` (NLog)
- `RollingInterval` (Serilog)
- `FileArchivePeriod` (NLog)