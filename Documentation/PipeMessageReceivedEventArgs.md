# PipeMessageReceivedEventArgs

Carries the raw and deserialized pipe message from the server.

---

## 📦 Namespace

`SeroGlint.DotNet.NamedPipes.Packaging`

---

## 🧩 Class Definition

```csharp
public class PipeMessageReceivedEventArgs : EventArgs
```

---

## 🔧 Properties

- `string RawMessage`
- `IPipeEnvelope Envelope`

---

## 📝 Remarks

- Triggered after a successful deserialization.