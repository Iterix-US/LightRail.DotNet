using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Controls;
using RegressionTestHarness.Objects;
using RegressionTestHarness.Utilities;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes;
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
        private static readonly string EncryptionKey = AesEncryptionService.GenerateKey();

        private ILogger _serverLogger;
        private ILogger _clientLogger;
        private NamedPipeServer _namedPipeServer;
        private AesEncryptionService _encryptionService;
        private PipeServerConfiguration _configuration;

        public NamedPipesView()
        {
            InitializeComponent();
            InitializeLogger();
            InitializeServerConfiguration();
            InitializeServer();
        }

        private void InitializeServerConfiguration()
        {
            _encryptionService = new AesEncryptionService(EncryptionKey, _serverLogger);

            _configuration = new PipeServerConfiguration();
            _configuration.Initialize(
                null,
                null,
                _serverLogger,
                _encryptionService,
                new CancellationTokenSource());
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
            _namedPipeServer = new NamedPipeServer(_configuration);
            _namedPipeServer.MessageReceived += NamedPipeServerMessageReceived;
            _namedPipeServer.ResponseRequested += NamedPipeServerResponseRequested;
        }

        private async void btnStartServer_Click(object sender, RoutedEventArgs e)
        {
            _ = Task.Run(_namedPipeServer.StartAsync);
        }

        private void btnStopServer_Click(object sender, RoutedEventArgs e)
        {
            _namedPipeServer.Configuration.CancellationTokenSource.Cancel();
        }

        private async void btnSendTestMessage_Click(object sender, RoutedEventArgs e)
        {
            var envelope = new PipeEnvelope<TestObject>(_clientLogger)
            {
                Payload = new TestObject()
            };

            var client = new NamedPipeClient(_configuration, _clientLogger);
            await client.SendMessage(envelope);
        }

        private void btnClearServerLog_Click(object sender, RoutedEventArgs e)
        {
            LstServerLog.Items.Clear();
        }

        private void btnClearClientLog_Click(object sender, RoutedEventArgs e)
        {
            LstClientLog.Items.Clear();
        }

        private void NamedPipeServerMessageReceived(object sender, PipeMessageReceivedEventArgs args)
        {
            LstServerLog.Items.Add($"FROM PIPE SERVER- Received message: {args.Json}");
        }

        private async Task NamedPipeServerResponseRequested(object sender, PipeResponseRequestedEventArgs args)
        {
            LstServerLog
                .Items
                .Add(
                    $"FROM PIPE SERVER- Response requested for message ID " +
                    $"{args.ResponseObject.MessageId}: " +
                    $"{args.ResponseObject.ToJson()}"
                );
        }
    }
}
