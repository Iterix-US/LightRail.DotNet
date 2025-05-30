using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Controls;
using RegressionTestHarness.Utilities;
using SeroGlint.DotNet.NamedPipes.Packaging;
using SeroGlint.DotNet.NamedPipes.Servers;
using SeroGlint.DotNet.SecurityUtilities;

namespace RegressionTestHarness.Pages
{
    /// <summary>
    /// Interaction logic for NamedPipesView.xaml
    /// </summary>
    public partial class NamedPipesView : UserControl
    {
        private ILogger _serverLogger;
        private ILogger _clientLogger;
        private NamedPipeServer _namedPipeServer;

        public NamedPipesView()
        {
            InitializeComponent();
            InitializeLogger();
            InitializeServer();
        }

        private void InitializeLogger()
        {
            _serverLogger = ListBoxLogger.RouteLoggerToListBox(Dispatcher, LstServerLog, "NamedPipeServerTestHarness");
            _serverLogger.LogInformation("Server logger initialized in test harness.");

            _clientLogger = ListBoxLogger.RouteLoggerToListBox(Dispatcher, LstClientLog, "NamedPipeClientTestHarness");
            _clientLogger.LogInformation("Client logger initialized in test harness.");
        }

        private void InitializeServer()
        {
            var serverConfiguration = new PipeServerConfiguration
            {
                Logger = _serverLogger,
                EncryptionKey = AesEncryptionService.GenerateKey(),
                UseEncryption = true
            };

            _namedPipeServer = new NamedPipeServer(serverConfiguration);
        }

        private void btnStartServer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnStopServer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSendTestMessage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClearServerLog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClearClientLog_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
