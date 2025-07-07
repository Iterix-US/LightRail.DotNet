using System.Security.Principal;
using SeroGlint.DotNet.Windows.Abstractions;

namespace SeroGlint.DotNet.Windows.Wrappers
{
    public class WindowsIdentityWrapper : IWindowsIdentityWrapper
    {
        private readonly WindowsIdentity _identity;

        public WindowsIdentityWrapper()
        {
            _identity = WindowsIdentity.GetCurrent();
        }

        public string Name => _identity.Name;

        public object GetIdentity() => _identity;

        public void Dispose() => _identity?.Dispose();

        public WindowsIdentity GetCurrent() => WindowsIdentity.GetCurrent();
    }
}
