# IntegerExtensions

Provides helper methods for `int` values that simplify common numerical checks such as bounds or divisibility.

---

## 📦 Namespace

`LightRail.DotNet.Extensions`

---

## 🧪 Notes

All methods are extension methods on the built-in `int` type. They provide fluent, readable alternatives to common conditional checks.

---

## ✅ Usage

### Check if a value is between two bounds (inclusive)

```csharp
int number = 15;

bool result1 = number.IsBetween(10, 20); // true
bool result2 = number.IsBetween(16, 20); // false
```

### Check if a value is a multiple of another

```csharp
int number = 12;

bool result1 = number.IsMultipleOf(3); // true
bool result2 = number.IsMultipleOf(5); // false
```

---

## 📌 Summary

| Method|Description |
|--|--|
| `IsBetween` | Returns true if value is between the given min and max. |
| `IsMultipleOf` | Returns true if value is divisible evenly by another.   |
