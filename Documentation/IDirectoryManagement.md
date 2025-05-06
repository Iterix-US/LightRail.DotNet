# IDirectoryManagement

An interface designed to abstract basic file and directory operations, useful for enabling testability and mocking in file-system dependent logic.

---

## 📦 Namespace

`LightRail.DotNet.Abstractions`

---

## 🧩 Interface Definition

```csharp
public interface IDirectoryManagement : IDisposable
{
    bool Exists(string filePath);
    string ReadAllText(string filePath);
    string WriteAllText(string path, string contents);
    string GetFullPath(string path);
    string GetDirectoryName(string filePath);
    Stream Create(string filePath);
    DirectoryInfo CreateDirectory(string path);
}
```

---

## 🧪 Notes

- This interface is intended to be injected where direct use of `System.IO` would normally occur.
- Particularly useful for testing classes like `ConfigurationLoader` without relying on actual file I/O.
- Each method mirrors a typical operation you'd find in `System.IO.File` or `System.IO.Directory`.

---

## ✅ Typical Use Case

Inject a mock or fake implementation for testing, or create a concrete wrapper over `System.IO` for production use.

```csharp
public class RealDirectoryManager : IDirectoryManagement
{
    public bool Exists(string filePath) => File.Exists(filePath);
    public string ReadAllText(string filePath) => File.ReadAllText(filePath);
    public string WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
    public string GetFullPath(string path) => Path.GetFullPath(path);
    public string GetDirectoryName(string filePath) => Path.GetDirectoryName(filePath);
    public Stream Create(string filePath) => File.Create(filePath);
    public DirectoryInfo CreateDirectory(string path) => Directory.CreateDirectory(path);
    public void Dispose() { /* Optional cleanup */ }
}
```

---

## 💡 Design Insight

The goal of `IDirectoryManagement` is to isolate file system dependencies so that code relying on disk operations can be easily tested, mocked, or redirected.