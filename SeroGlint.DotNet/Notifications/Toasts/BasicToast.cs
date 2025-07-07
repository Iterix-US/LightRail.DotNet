using SeroGlint.DotNet.Notifications.Interfaces;

namespace SeroGlint.DotNet.Notifications.Toasts
{
    public class BasicToast : ToastBase<BasicToast>, IBasicToast
    {
        public string Body { get; } = "";
        public string IconPath { get; } = "";
    }
}
