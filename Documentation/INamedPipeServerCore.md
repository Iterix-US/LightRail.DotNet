# INamedPipeServerCore

Defines the server-side functionality for handling named pipe communication.

---

## 📦 Namespace

`SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces`

---

## 🧩 Interface Definition

```csharp
public interface INamedPipeServerCore
{
    Task StartAsync();
    event PipeMessageReceivedHandler MessageReceived;
    event PipeResponseRequestedHandler ResponseRequested;
}
```