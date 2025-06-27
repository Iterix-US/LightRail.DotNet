# INamedPipeClient

Interface defining the pipe client for sending messages.

---

## 📦 Namespace

`SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces`

---

## 🧩 Interface Definition

```csharp
public interface INamedPipeClient
{
    Task SendMessage<T>(IPipeEnvelope<T> message);
}
```