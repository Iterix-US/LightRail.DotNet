using Microsoft.Extensions.Logging;

namespace SeroGlint.DotNet.Notifications.Interfaces
{
    public interface INotificationPlatform
    {
        ILogger Logger { get; }
        string OperatingSystem { get; }
        string SoftwareName { get; }
        void PopToast(IToastPayload payload);
    }
}
