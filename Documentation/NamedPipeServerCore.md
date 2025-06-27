# NamedPipeServerCore

Handles the lifecycle and messaging of a named pipe server.

---

## 📦 Namespace

`SeroGlint.DotNet.NamedPipes.Servers`

---

## 🧩 Class Definition

```csharp
public class NamedPipeServerCore : INamedPipeServerCore, IDisposable
```

---

## ✅ Responsibilities

- Accepts client connections via named pipes.
- Deserializes and processes messages.
- Triggers message and response events.
- Supports optional encryption and cancellation.

---

## 🧪 Events

- `MessageReceived` — raised when a valid message is received.
- `ResponseRequested` — raised to send a response back to the client.

---

## 🔧 Key Methods

- `Task StartAsync()` — Starts the server and listens for client connections.
- `void Dispose()` — Logs disposal.

---

## 📝 Remarks

- Designed for extensibility and testability.
- Will throw `OperationCanceledException` on cancellation.
- Wraps all errors in descriptive `Exception` or `InvalidOperationException` types.