# PipeResponseRequestedEventArgs

Carries the data to send as a response from a named pipe server.

---

## 📦 Namespace

`SeroGlint.DotNet.NamedPipes.Packaging`

---

## 🧩 Class Definition

```csharp
public class PipeResponseRequestedEventArgs : EventArgs
```

---

## 🔧 Properties

- `Guid CorrelationId`
- `object ResponseObject`
- `NamedPipeServerStream Server`

---

## 📝 Remarks

- Used when the server wishes to respond to a message.