using System;
using System.IO;

namespace LightRail.DotNet.Abstractions
{
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
}
