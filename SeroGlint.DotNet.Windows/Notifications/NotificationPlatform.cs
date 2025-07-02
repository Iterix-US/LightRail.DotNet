using System.Xml;
using Windows.UI.Notifications;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Notifications.Interfaces;
using Microsoft.Toolkit.Uwp.Notifications;
using SeroGlint.DotNet.Windows.Extensions;

namespace SeroGlint.DotNet.Windows.Notifications
{
    public class NotificationPlatform : INotificationPlatform
    {
        private readonly ToastContentBuilder _contentBuilder = new ToastContentBuilder();
        private ToastNotifier _notifier;
        public ILogger Logger { get; }
        public string OperatingSystem => "Windows";
        public string SoftwareName { get; }

        public NotificationPlatform(ILogger logger, string softwareName)
        {
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            SoftwareName = softwareName;
        }

        public void PopToast(IToastPayload payload)
        {
            CreateNotifier();

            Logger.LogInformation("Popping toast notification on Windows.");
            _notifier.Show(payload.ToToast());
        }

        private void CreateNotifier()
        {
            _notifier = ToastNotificationManager.CreateToastNotifier("AppName");
            var toastXml = new XmlDocument();
            toastXml.LoadXml(_contentBuilder!.GetToastContent().GetContent());
        }
    }
}
