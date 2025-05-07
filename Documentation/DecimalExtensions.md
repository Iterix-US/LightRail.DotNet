# DecimalExtensions

Provides utility extension methods for `decimal` values to evaluate, convert, and manipulate numeric data more expressively.

---

## 📦 Namespace

`SeroGlint.DotNet.Extensions`

---

## ✅ Usage

### Sign & Value Checks

```csharp
decimal value = 15.3m;

bool isPositive = value.IsPositive();   // true
bool isNegative = value.IsNegative();   // false
bool isZero = value.IsZero();           // false
bool isBetween = value.IsBetween(10m, 20m); // true
bool isMultiple = value.IsMultipleOf(5m);  // false
```

### Even, Odd, and Integer Checks

```csharp
decimal value = 8m;

bool isEven = value.IsEven();           // true
bool isOdd = value.IsOdd();             // false
bool isInteger = value.IsInteger();     // true
```

### Integer Conversions

```csharp
decimal val = 12.7m;

int ceil = val.ToIntegerCeiling();      // 13
int floor = val.ToIntegerFloor();       // 12
int round = val.ToIntegerRound();       // 13
int trunc = val.ToIntegerTruncate();    // 12
```

### Formatted Decimal String

```csharp
decimal number = 123.45678m;

string twoDecimals = number.ToDecimalString();      // "123.46"
string fourDecimals = number.ToDecimalString(4);    // "123.4568"
```