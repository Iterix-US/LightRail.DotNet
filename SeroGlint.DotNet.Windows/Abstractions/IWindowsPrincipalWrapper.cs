using System.Security.Principal;

namespace SeroGlint.DotNet.Windows.Abstractions
{
    public interface IWindowsPrincipalWrapper
    {
        bool IsInRole(WindowsBuiltInRole role);
    }
}