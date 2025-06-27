# NamedPipeClient

Sends serialized and optionally encrypted messages to a named pipe server.

---

## 📦 Namespace

`SeroGlint.DotNet.NamedPipes`

---

## 🧩 Class Definition

```csharp
public class NamedPipeClient : INamedPipeClient
```

---

## ✅ Responsibilities

- Validates pipe settings.
- Serializes and optionally encrypts messages.
- Connects to named pipe server and sends messages.

---

## 🔧 Key Method

- `Task SendMessage<T>(IPipeEnvelope<T> message)`

---

## 📝 Remarks

- Uses `CancellationTokenSource` for timeout and cancellation.
- Logging occurs for each step including validation, encryption, and errors.