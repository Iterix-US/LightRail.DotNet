# ObjectExtensions

Provides serialization extensions for objects in C#, allowing for quick conversion to JSON and XML formats.

---

## 📦 Namespace

`LightRail.DotNet.Extensions`

## 🧪 Notes

- These methods are especially helpful for diagnostics, logging, or data transport scenarios.
- XML serialization allows for optional indentation and error logging via `ILogger`.

---

## ✅ Usage

### Serialize object to JSON

```csharp
var person = new Person { Name = "John", Age = 30 };
string json = person.ToJson(); // {"Name":"John","Age":30}
```

### Serialize object to XML

```csharp
var person = new Person { Name = "John", Age = 30 };
string xml = person.ToXml(); // Produces XML string for the object
```

### Serialize with indentation and error logging

```csharp
var logger = Substitute.For<ILogger>();
var person = new Person { Name = "John", Age = 30 };
string xml = person.ToXml(indent: true, logger: logger);
```

---

## 🔒 Exceptions

- `ToXml<T>()` throws `InvalidOperationException` if the object cannot be serialized to XML.
- If a logger is provided, it logs the error before throwing.

---

## 🧼 Dependencies

- `System.Text.Json`
- `System.Xml`
- `Microsoft.Extensions.Logging`