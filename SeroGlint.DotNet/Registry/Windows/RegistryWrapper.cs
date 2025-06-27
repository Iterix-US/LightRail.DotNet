using SeroGlint.DotNet.Registry.Windows.Interfaces;
// ReSharper disable ConvertToAutoProperty

namespace SeroGlint.DotNet.Registry.Windows
{
    public class RegistryWrapper : IRegistryWrapper
    {
        private readonly IRegistryKey _currentUser = new RegistryKeyWrapper(Microsoft.Win32.Registry.CurrentUser);
        private readonly IRegistryKey _localMachine = new RegistryKeyWrapper(Microsoft.Win32.Registry.LocalMachine);
        private readonly IRegistryKey _classesRoot = new RegistryKeyWrapper(Microsoft.Win32.Registry.ClassesRoot);
        private readonly IRegistryKey _users = new RegistryKeyWrapper(Microsoft.Win32.Registry.Users);
        private readonly IRegistryKey _performanceData = new RegistryKeyWrapper(Microsoft.Win32.Registry.PerformanceData);
        private readonly IRegistryKey _currentConfig = new RegistryKeyWrapper(Microsoft.Win32.Registry.CurrentConfig);

        public IRegistryKey CurrentUser => _currentUser;
        public IRegistryKey LocalMachine => _localMachine;
        public IRegistryKey ClassesRoot => _classesRoot;
        public IRegistryKey Users => _users;
        public IRegistryKey PerformanceData => _performanceData;
        public IRegistryKey CurrentConfig => _currentConfig;
    }
}
