using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Logging;
using SeroGlint.DotNet.Notifications.Interfaces;
using SeroGlint.DotNet.Notifications.Toasts;
using SeroGlint.DotNet.Windows.Notifications;

namespace RegressionTestHarness.Pages
{
    /// <summary>
    /// Interaction logic for ToastNotificationsView.xaml
    /// </summary>
    public partial class ToastNotificationsView : UserControl
    {
        private readonly ILogger _logger;
        private IBasicToast? _baseToast;
        private INotificationPlatform _notificationPlatform;

        public ToastNotificationsView()
        {
            InitializeComponent();
            _logger = new LoggerFactoryBuilder()
                .EnableConsoleOutput()
                .EnableFileOutput(createLogPath: true, logPath: "Logs", logName: "ToastNotificationsView")
                .BuildSerilog();

            _notificationPlatform = new NotificationPlatform(_logger, AppDomain.CurrentDomain.FriendlyName);
        }

        private void btnPopBasicToast_Click(object sender, RoutedEventArgs e)
        {
            _baseToast = new BasicToast()
                .WithId(Guid.NewGuid().ToString())
                .WithGroupName("TestGroup")
                .WithLogger(_logger)
                .WithTimeout(TimeSpan.FromSeconds(10))
                .WithTitle("Test Title")
                .WithBody("Test body text");

            _notificationPlatform.PopToast(_baseToast);
        }
    }
}
