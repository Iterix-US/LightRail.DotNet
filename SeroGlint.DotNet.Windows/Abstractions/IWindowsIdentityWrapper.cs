using System.Security.Principal;

namespace SeroGlint.DotNet.Windows.Abstractions
{
    public interface IWindowsIdentityWrapper
    {
        string Name { get; }
        object GetIdentity();
        void Dispose();
        WindowsIdentity GetCurrent();
    }
}