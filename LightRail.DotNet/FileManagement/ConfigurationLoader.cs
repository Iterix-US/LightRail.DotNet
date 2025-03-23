using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using LightRail.DotNet.Abstractions;

namespace LightRail.DotNet.FileManagement
{
    public class ConfigurationLoader
    {
        private readonly IDirectoryManagement _fileManager;

        public ConfigurationLoader(IDirectoryManagement fileManager = null)
        {
            _fileManager = fileManager;
        }

        public T LoadJsonConfiguration<T>(string filePath) where T : class
        {
            var fileExists = _fileManager?.Exists(filePath) ?? File.Exists(filePath);

            if (!fileExists)
            {
                return Activator.CreateInstance<T>();
            }

            var json = _fileManager?.ReadAllText(filePath) ?? File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json) ?? Activator.CreateInstance<T>();
        }

        public void SaveJsonConfiguration<T>(string filePath, T configuration)
        {
            var json = JsonSerializer.Serialize(configuration, new JsonSerializerOptions { WriteIndented = true });

            try
            {
                var file = _fileManager?.GetFullPath(filePath) ?? Path.GetFullPath(filePath);
                var path = _fileManager?.GetDirectoryName(filePath) ?? Path.GetDirectoryName(file);

                SafeguardDirectory(path);
                SafeguardFile(file);

                if (_fileManager == null)
                {
                    File.WriteAllText(file, json);
                    return;
                }

                _fileManager.WriteAllText(file, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new FileNotFoundException("Configuration file could not be created", ex);
            }
        }

        private void SafeguardFile(string file)
        {
            var fileExists = _fileManager?.Exists(file) ?? File.Exists(file);

            if (fileExists && _fileManager == null)
            {
                return;
            }

            var fileStream = _fileManager?.Create(file) ?? File.Create(file);

            if (fileStream == null)
            {
                throw new FileNotFoundException($"The file {file} could not be created.");
            }

            fileStream.Dispose();
        }

        private void SafeguardDirectory(string path)
        {
            var directoryExists = _fileManager?.Exists(path) ?? Directory.Exists(path);

            if (directoryExists)
            {
                return;
            }

            var directoryInfo = _fileManager?.CreateDirectory(path) ?? Directory.CreateDirectory(path);

            if (directoryInfo == null)
            {
                throw new DirectoryNotFoundException($"The directory {path} could not be created.");
            }
        }
    }
}
