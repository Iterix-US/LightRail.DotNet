# BooleanExtensions

Provides utility extension methods for converting `bool` values to common representations like integers or strings.

---

## 📦 Namespace

LightRail.DotNet.Extensions

## 🧪 Notes

All methods are pure and do not require any setup or dependency injection. They operate directly on the bool type via extension methods.

## ✅ Usage

### Boolean Conversions

```csharp
bool value = true;
int result = value.ToInteger(); // result = 1

bool value = false;
int result = value.ToInteger(); // result = 0

bool value = true;
string result = value.ToYesNo(); // result = "Yes"

bool value = false;
string result = value.ToYesNo(); // result = "No"

bool value = true;
string result = value.ToOneZero(); // result = "1"

bool value = false;
string result = value.ToOneZero(); // result = "0"

bool value = true;
string result = value.ToOnOff(); // result = "On"

bool value = false;
string result = value.ToOnOff(); // result = "Off"

bool value = true;
string result = value.ToEnabledDisabled(); // result = "Enabled"

bool value = false;
string result = value.ToEnabledDisabled(); // result = "Disabled"

bool value = true;
string result = value.ToActiveInactive(); // result = "Active"

bool value = false;
string result = value.ToActiveInactive(); // result = "Inactive"
```
