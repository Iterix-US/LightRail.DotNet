# StringExtensions

Provides extension methods for the `string` data type in C#. These methods simplify parsing, comparison, encoding, and serialization/deserialization of string values.

---

## 📦 Namespace

`SeroGlint.DotNet.Extensions`

---

## ✅ Common Usage

### 🔍 Case-Insensitive Operations

```csharp
"hello world".ContainsIgnoreCase("HELLO"); // true
"Example".EqualsIgnoreCase("example");     // true
"url/path".StartsWithIgnoreCase("URL");    // true
"config.json".EndsWithIgnoreCase("JSON");  // true
```

### 🎨 Title Case

```csharp
"this is a test".ToTitleCase(); // "This Is A Test"
```

### 🔢 String to Primitive Type

```csharp
"123".ToInt32(out int i);       // true, i = 123
"9999".ToInt64(out long l);     // true, l = 9999
"3.14".ToDouble(out double d);  // true, d = 3.14
"42.5".ToDecimal(out decimal m);// true, m = 42.5m
"2024-05-01".ToDateTime(out var dt); // true
"true".ToBoolean(out var b);    // true, b = true
```

### 🔁 JSON / XML (De)Serialization

```csharp
var user = "{"Name":"John"}".FromJsonToType<User>(); // JSON to object

string xml = "<User><Name>John</Name></User>";
var user = xml.FromXmlToType<User>(); // XML to object
```

### 🎭 Enum Helpers

```csharp
"Active".IsValidEnumValue(out StatusEnum result); // true if StatusEnum has value "Active"
"Enabled".GetEnumFromDescription<StatusEnum>(); // StatusEnum.Enabled if it has [Description("Enabled")]
```

### 🌐 Encoding and Hashing

```csharp
"hello world".EncodeAsHttp(); // "hello%20world"
"mypassword".ToSha256();      // Base64-encoded SHA256 hash
"data".ToHexString();         // "64617461" (hex encoding)
```

---

## 🧪 Notes

- XML and JSON methods use `System.Text.Json` and `System.Xml.Serialization`.
- The `FromXmlToType` method supports optional logging for error tracking.
- Enum description mapping relies on `[Description("...")]` attributes.