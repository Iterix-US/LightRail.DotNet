using System;
using System.Collections;
using Microsoft.Win32;

namespace SeroGlint.DotNet.Registry.Windows.Interfaces
{
    public interface IRegistryKey : IDisposable
    {
        int SubKeyCount { get; }
        string Name { get; }

        IEnumerable GetSubKeyNames();
        IRegistryKey OpenSubKey(string name, bool writable = false);
        IRegistryKey CreateSubKey(string name, RegistryKeyPermissionCheck permissionCheck = RegistryKeyPermissionCheck.Default, RegistryOptions options = RegistryOptions.None);
        void DeleteSubKey(string subkey, bool throwOnMissingSubKey = true);
        void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey = true);
        void Close();
        void Flush();
        object GetValue(string name, object defaultValue = null, RegistryValueOptions options = RegistryValueOptions.None);
        void SetValue(string name, object value, RegistryValueKind valueKind);
    }
}
