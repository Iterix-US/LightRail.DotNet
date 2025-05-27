# SeroGlint.DotNet

**A powerful yet lightweight toolkit that simplifies, accelerates, and enhances your .NET development.**

---

## 🚦 **What is SeroGlint.DotNet?**

At Iterix, we know that the repetitive tasks in software development slow teams down.  
**SeroGlint.DotNet** is our solution—a carefully designed library filled with elegant extension methods, converters, serialization utilities, and common tools tailored specifically for .NET developers. Our mission is straightforward: eliminate boilerplate, reduce friction, and enable your team to build better software, faster.

---

## ⚙️ **Core Features**

- **Primitive Extensions**  
  Simple yet powerful enhancements for strings, dates, numeric types, and more, to streamline your day-to-day coding.

- **Serialization Helpers (XML & JSON)**  
  Consistent, hassle-free serialization and deserialization methods that help you manage data effortlessly.

- **Structured Logging Builders**  
  Drop-in builders for Serilog and NLog via a unified API, built for testability and DI registration.

- **Robust & Tested**  
  Fully unit-tested to guarantee reliability, ensuring you always have dependable tools at your fingertips.

- **Cross-Platform Ready**  
  Built for compatibility with .NET 8+, .NET Standard, and future platforms—ready to deploy anywhere you need it.

---

## 🚀 **Quick Examples**

**Serialize an object to XML quickly:**

```csharp
var user = new User { Id = 1, Name = "Iterix" };
var xml = user.ToXml();
```

**Easily deserialize JSON strings:**

```csharp
var jsonString = "{"Id":1,"Name":"Iterix"}";
var user = jsonString.FromJson<User>();
```

**Date extensions that make sense:**

```csharp
var future = DateTime.Now.Plus(years: 1, months: 3);
var past = DateTime.Now.Minus(years: 1, months: 3, days: 4, hours: 10, minutes: 15, seconds: 34, milliseconds: 120);
```

---

## 📂 **API Documentation**

You can find detailed documentation for each class or utility in the [`Documentation`](./Documentation) folder:

### 🔧 Core Extensions

- [BooleanExtensions](./Documentation/BooleanExtensions.md)
- [ByteExtensions](./Documentation/ByteExtensions.md)
- [DateTimeExtensions](./Documentation/DateTimeExtensions.md)
- [DecimalExtensions](./Documentation/DecimalExtensions.md)
- [EnumExtensions](./Documentation/EnumExtensions.md)
- [IntegerExtensions](./Documentation/IntegerExtensions.md)
- [ObjectExtensions](./Documentation/ObjectExtensions.md)
- [StringExtensions](./Documentation/StringExtensions.md)

### 🔐 Configuration & Utilities

- [ConfigurationLoader](./Documentation/ConfigurationLoader.md)
- [PasswordUtility](./Documentation/PasswordUtility.md)
- [Enums](./Documentation/Enums.md)
- [IDirectoryManagement](./Documentation/IDirectoryManagement.md)
- [DirectoryManagementWrapper](./Documentation/DirectoryManagementWrapper.md)

### 📝 Logging System

- [LoggerFactoryBuilder](./Documentation/LoggerFactoryBuilder.md)
- [SerilogBuilder](./Documentation/SerilogBuilder.md)
- [NLogBuilder](./Documentation/NLogBuilder.md)
- [ILoggerBuilder](./Documentation/ILoggerBuilder.md)
- [LoggerBuilderBase](./Documentation/LoggerBuilderBase.md)

---

## 🧪 **Reliability & Quality Assurance**

We take quality seriously. Each component is thoroughly unit tested, and we continuously strive for 100% test coverage for our logic. Reliable software is at the core of our philosophy—and yours.

---

## 📜 **License**

SeroGlint.DotNet is proudly licensed under the **Mozilla Public License 2.0 (MPL-2.0)**, ensuring open usage while clearly defining how contributions and modifications should happen.  
See the [LICENSE](LICENSE) file for full details.

---

## 🗣️ **Feedback & Contributions**

Have a suggestion or improvement? **We love collaboration!**

- Submit issues or pull requests on GitHub
- Visit our website [iterix.net](https://iterix.net/) for additional insights or contact our team directly.

We welcome your input and appreciate your contributions.

---

## 🔗 **Who We Are**

Learn more about us at [iterix.net](https://iterix.net).

---

## 🛣️ **Future Features**

- Interface + wrapper classes for externally dependent tooling
  1. Allows for unit testing to be more fluid
  2. Does impose a memory usage overhead, but may be worth the cost to ensure functional stability
  3. Mainly for namespaces like Registry, File, Directory, etc.
- AI-assisted translations
  1. Still thinking about this one.
  2. I'm usually pretty hesitent about implementing AI tooling
  3. Would need to be able to cache/store translations locally or to a local area network device in case of internet outage
