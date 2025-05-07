# ConfigurationLoader

Provides a utility class for loading and saving configuration files in JSON format, with optional support for dependency injection via `IDirectoryManagement`.

---

## 📦 Namespace

`SeroGlint.DotNet.FileManagement`

---

## 🧰 Constructor

```csharp
var loader = new ConfigurationLoader(); // Uses default File I/O
// or
var loader = new ConfigurationLoader(fileManager); // Uses IDirectoryManagement abstraction
```

---

## 📥 LoadJsonConfiguration\<T\>

Deserializes a JSON file into an object of type `T`.

```csharp
var settings = loader.LoadJsonConfiguration<AppSettings>("path/to/settings.json");
```

- If the file does not exist, a default instance of `T` is returned.
- Uses `IDirectoryManagement.Exists()` and `ReadAllText()` if injected.

---

## 💾 SaveJsonConfiguration\<T\>

Serializes an object and writes it to a file.

```csharp
loader.SaveJsonConfiguration("path/to/config.json", settings);
```

- Automatically creates the target directory and file if they don't exist.
- Logs errors using `ILogger` if provided.
- Throws `FileNotFoundException` on unrecoverable write failure.

---

## 🛡️ Internal/Private Methods

### `SafeguardDirectory(string path)`

Creates a directory if it doesn't exist. Used internally before writing a config file.

### `SafeguardFile(string file)`

Creates the config file if it doesn't already exist. Ensures write safety.

> These methods are excluded from code coverage but crucial for robustness in distributed or new-environment deployments.

---

## ✅ Best Practices

- Use `IDirectoryManagement` in unit tests to mock filesystem operations.
- Pair with a strongly typed settings class for config binding consistency.

---

## 📘 Example

```csharp
public class AppSettings
{
    public string AppName { get; set; }
    public int MaxConnections { get; set; }
}

var config = new ConfigurationLoader();
var settings = config.LoadJsonConfiguration<AppSettings>("appsettings.json");

settings.MaxConnections = 20;
config.SaveJsonConfiguration("appsettings.json", settings);
```
