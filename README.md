
# LightRail.DotNet

**A powerful yet lightweight toolkit that simplifies, accelerates, and enhances your .NET development.**

---

### 🚦 **What is LightRail.DotNet?**

At Iterix, we know that the repetitive tasks in software development slow teams down.  
**LightRail.DotNet** is our solution—a carefully designed library filled with elegant extension methods, converters, serialization utilities, and common tools tailored specifically for .NET developers. Our mission is straightforward: eliminate boilerplate, reduce friction, and enable your team to build better software, faster.

---

### ⚙️ **Core Features:**

- **Primitive Extensions**:  
  Simple yet powerful enhancements for strings, dates, numeric types, and more, to streamline your day-to-day coding.

- **Serialization Helpers (XML & JSON)**:  
  Consistent, hassle-free serialization and deserialization methods that help you manage data effortlessly.

- **Robust & Tested**:  
  Fully unit-tested to guarantee reliability, ensuring you always have dependable tools at your fingertips.

- **Cross-Platform Ready**:  
  Built for compatibility with .NET 8+, .NET Standard, and future platforms—ready to deploy anywhere you need it.

---

### 🧰 **Installation (NuGet - Coming Soon):**

We’re preparing a NuGet package to simplify installation—stay tuned. Until then, you can build from source:

```bash
git clone https://github.com/Iterix-US/LightRail.DotNet.git
cd LightRail.DotNet
dotnet build
```

---

### 🚀 **Quick Examples:**

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
var past = DateTime.Now.Minus(years: 1, months: 3, days: 4, hours: 10, minutes: 15, seconds: 34: milliseconds: 120);
```

---

### 🧪 **Reliability & Quality Assurance**

We take quality seriously. Each component is thoroughly unit tested, and we continuously strive for 100% test coverage for our logic. Reliable software is at the core of our philosophy—and yours.

---

### 📜 **License**

LightRail.DotNet is proudly licensed under the **Mozilla Public License 2.0 (MPL-2.0)**, ensuring open usage while clearly defining how contributions and modifications should happen.  
See the [LICENSE](LICENSE) file for full details.

---

### 🗣️ **Feedback & Contributions**

Have a suggestion or improvement? **We love collaboration!**

- Feel free to submit issues or pull requests.
- Visit our website [iterix.net](https://iterix.net/) for additional insights or contact our team directly.

We welcome your input and appreciate your contributions.

---

### 🔗 **Who We Are**

At **Iterix**, we build solutions that turn complexity into clarity. Our focus is crafting exceptional software, helping your teams deliver faster, better, and with confidence.

Learn more about us at [iterix.net](https://iterix.net).

**Iterix:** *One step at a time.*
