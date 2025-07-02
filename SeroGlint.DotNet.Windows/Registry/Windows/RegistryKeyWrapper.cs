using System.Collections;
using Microsoft.Win32;
using SeroGlint.DotNet.Windows.Registry.Windows.Interfaces;

namespace SeroGlint.DotNet.Windows.Registry.Windows
{
    public class RegistryKeyWrapper : IRegistryKey
    {
        private readonly RegistryKey _inner;
        public int SubKeyCount => _inner.SubKeyCount;
        public string Name => _inner.Name;

        public RegistryKeyWrapper(RegistryKey registryKey)
        {
            _inner = registryKey;
        }

        public IEnumerable GetSubKeyNames()
        {
            return _inner.GetSubKeyNames();
        }

        public IRegistryKey OpenSubKey(string name, bool writable = false)
        {
            var subKey = _inner.OpenSubKey(name, writable);
            return subKey != null ? new RegistryKeyWrapper(subKey) : null;
        }

        public IRegistryKey CreateSubKey(string name, RegistryKeyPermissionCheck permissionCheck = RegistryKeyPermissionCheck.Default,
            RegistryOptions options = RegistryOptions.None)
        {
            var subKey = _inner.CreateSubKey(name, permissionCheck, options);
            return subKey != null ? new RegistryKeyWrapper(subKey) : null;
        }

        public object GetValue(string name, object defaultValue = null, RegistryValueOptions options = RegistryValueOptions.None)
        {
            return _inner.GetValue(name, defaultValue, options);
        }

        public void DeleteSubKey(string subkey, bool throwOnMissingSubKey = true)
        {
            _inner.DeleteSubKey(subkey, throwOnMissingSubKey);
        }

        public void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey = true)
        {
            _inner.DeleteSubKeyTree(subkey, throwOnMissingSubKey);
        }

        public void Close()
        {
            _inner.Close();
        }

        public void Flush()
        {
            _inner.Flush();
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public void SetValue(string name, object value, RegistryValueKind valueKind)
        {
            _inner.SetValue(name, value, valueKind);
        }
    }
}
