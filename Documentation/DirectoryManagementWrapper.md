
# DirectoryManagementWrapper

A concrete implementation of `IDirectoryManagement` that wraps `System.IO` operations with added logging capabilities. Intended for use in production code where traceability and safe file operations are essential.

---

## 📦 Namespace

`SeroGlint.DotNet.FileManagement`

---

## 🧱 Class Definition

```csharp
[ExcludeFromCodeCoverage]
public class DirectoryManagementWrapper : IDirectoryManagement
{
    public DirectoryManagementWrapper(ILogger logger = null);

    bool Exists(string filePath);
    string ReadAllText(string filePath);
    string WriteAllText(string filePath, string contents);
    string GetFullPath(string path);
    string GetDirectoryName(string filePath);
    Stream Create(string filePath, FileMode fileMode = FileMode.Create, FileAccess fileAccess = FileAccess.ReadWrite, FileShare fileShare = FileShare.None);
    DirectoryInfo CreateDirectory(string path);
    bool DirectoryExists(string path);
    void Dispose();
}
```

---

## 🧪 Behavior Overview

- Each method logs informative messages through `ILogger` if supplied.
- Null or whitespace parameters trigger warnings and fail gracefully.
- Reads, writes, and checks are wrapped in basic validation to ensure resilience during file operations.

---

## 🔧 Usage Example

```csharp
var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("FileOps");
IDirectoryManagement directoryManager = new DirectoryManagementWrapper(logger);

var fullPath = directoryManager.GetFullPath("myfolder/data.txt");

if (!directoryManager.Exists(fullPath))
{
    directoryManager.WriteAllText(fullPath, "Sample content");
}

var content = directoryManager.ReadAllText(fullPath);
```

---

## ⚠️ Error Handling and Logging

- **Missing path/invalid input:** Returns empty values or false, never throws unless critical (e.g., `FileStream` creation).
- **Logs warnings:** For empty parameters or non-existent paths.
- **Logs info:** For operations like read, write, exists check, etc.

---

## 💡 Design Insight

`DirectoryManagementWrapper` acts as a logging-enhanced bridge to file system operations while fulfilling the `IDirectoryManagement` contract. Its purpose is twofold:

- **Traceable Production Behavior:** File access operations are auditable through the provided logger.
- **Testability & Clean Abstraction:** In test scenarios, this class can be replaced with a mock or stub, keeping production logic isolated and injectable.
