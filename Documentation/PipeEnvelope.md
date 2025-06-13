# PipeEnvelope<T>

Wraps a strongly typed message with metadata for IPC.

---

## 📦 Namespace

`SeroGlint.DotNet.NamedPipes.Packaging`

---

## 🧩 Class Definition

```csharp
public class PipeEnvelope<T> : IPipeEnvelope
```

---

## 🔧 Properties

- `Guid MessageId` — Unique identifier.
- `DateTime Timestamp` — When the message was created.
- `string TypeName` — Fully qualified type name.
- `T Payload` — The actual message payload.

---

## 🔧 Methods

- `byte[] Serialize()` — Converts envelope to UTF-8 byte array.
- `static PipeEnvelope<TTarget> Deserialize<TTarget>(string json, ILogger logger = null)` — Restores envelope from JSON.

---

## 📝 Remarks

- Used to encapsulate structured data and simplify encryption/decryption.