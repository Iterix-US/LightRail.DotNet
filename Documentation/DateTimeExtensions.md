# DateTimeExtensions

Provides utility extension methods for working with `DateTime` in a more expressive and human-readable way.

---

## 📦 Namespace

`SeroGlint.DotNet.Extensions`

---

## ✅ Usage Examples

### ➕ Add Time Components

```csharp
var future = DateTime.Now.Plus(years: 1, months: 2, days: 10);
// Equivalent to DateTime.Now.AddYears(1).AddMonths(2).AddDays(10)
```

### ➖ Subtract Time Components

```csharp
var past = DateTime.Now.Minus(days: 7, hours: 4);
// Equivalent to DateTime.Now.AddDays(-7).AddHours(-4)
```

### ➕ Add DateTime to DateTime

```csharp
var offset = new DateTime(1, 2, 3, 4, 5, 6); // Dummy offsets
var result = DateTime.Now.Plus(offset);
```

### ➖ Subtract DateTime from DateTime

```csharp
var difference = DateTime.Now.Minus(someOtherDateTime);
```

---

## 📅 Time Classification

```csharp
bool isWeekend = someDate.IsWeekend();
bool isWeekday = someDate.IsWeekday();
```

### 🧠 Friendly Time Ago

```csharp
string ago = DateTime.UtcNow.AddMinutes(-10).ToReadableTimeAgo(); // "10 minutes ago"
```

### 🕒 Get Time of Day

```csharp
TimeOfDay part = DateTime.Now.GetTimeOfDay();
// Returns EarlyMorning, Morning, Afternoon, etc.
```

---

## 🧪 Methods Overview

| Method | Description |
|--------|-------------|
| `Minus(...)` | Subtracts multiple time components from a `DateTime`. |
| `Minus(DateTime)` | Subtracts another `DateTime` value component-wise. |
| `Plus(...)` | Adds multiple time components to a `DateTime`. |
| `Plus(DateTime)` | Adds another `DateTime` value component-wise. |
| `IsWeekend()` | Checks if the date is Saturday or Sunday. |
| `IsWeekday()` | Checks if the date is Monday–Friday. |
| `ToReadableTimeAgo()` | Returns a human-readable "time ago" string. |
| `GetTimeOfDay()` | Returns a `TimeOfDay` enum for morning/evening/etc. |

---

## 📌 Enum Reference

See [`TimeOfDay`](./Enums.md) for possible return values from `GetTimeOfDay()`.
