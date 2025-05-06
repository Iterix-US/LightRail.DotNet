# Enums

This file contains application-wide enumerations used for time categorization and logging configuration.

---

## ⏰ TimeOfDay

Represents a user-friendly classification of time periods throughout the day.

| Value         | Description         | Time Range         |
|---------------|---------------------|--------------------|
| EarlyMorning  | Early Morning       | 12:00 AM – 5:59 AM |
| Dawn          | Dawn                | 6:00 AM – 6:59 AM  |
| Morning       | Morning             | 7:00 AM – 11:59 AM |
| Afternoon     | Afternoon           | 12:00 PM – 4:59 PM |
| Dusk          | Dusk                | 5:00 PM – 5:59 PM  |
| Evening       | Evening             | 6:00 PM – 8:59 PM  |
| Night         | Night               | 9:00 PM – 11:59 PM |

---

## 🪵 LoggingLevel

Represents the levels of logging verbosity used in the application.

| Value         | Numeric | Purpose / Detail                          |
|---------------|---------|-------------------------------------------|
| Verbose       | 0       | Very detailed messages, potentially noisy |
| Debug         | 1       | Diagnostic messages useful during dev     |
| Information   | 2       | Informational messages (normal operation) |
| Warning       | 3       | Indicates something unexpected            |
| Error         | 4       | Error that does not stop the application  |
| Fatal         | 5       | Critical error – application may crash    |

---

## 📁 LogRollInterval

Controls when a log file should be rolled over and archived.

| Value       | Numeric | Description                          |
|-------------|---------|--------------------------------------|
| Indefinite  | 0       | Logging continues into a single file |
| Yearly      | 1       | New file every year                  |
| Monthly     | 2       | New file every month                 |
| Daily       | 3       | New file every day                   |
| Hourly      | 4       | New file every hour                  |
