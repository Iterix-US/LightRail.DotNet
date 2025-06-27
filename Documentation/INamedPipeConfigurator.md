# INamedPipeConfigurator

Provides the configuration context for both client and server in named pipe communication.

---

## 📦 Namespace

`SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces`

---

## 🧩 Interface Definition

```csharp
public interface INamedPipeConfigurator
{
    string ServerName { get; }
    string PipeName { get; }
    bool UseEncryption { get; }
    IEncryptionService EncryptionService { get; }
    CancellationTokenSource CancellationTokenSource { get; }
}
```