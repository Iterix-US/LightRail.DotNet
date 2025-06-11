using Microsoft.Extensions.Logging;
using System.Windows;
using RegressionTestHarness.Objects;
using RegressionTestHarness.Utilities;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes;
using SeroGlint.DotNet.NamedPipes.Packaging;
using SeroGlint.DotNet.SecurityUtilities;
using SeroGlint.DotNet.NamedPipes.EventArguments;

namespace RegressionTestHarness.Pages
{
    /// <summary>
    /// Interaction logic for NamedPipesView.xaml
    /// </summary>
    public partial class NamedPipesView
    {
        private static readonly string EncryptionKey = AesEncryptionService.GenerateKey();

        private ILogger? _serverLogger;
        private ILogger? _clientLogger;
        private NamedPipeServer? _namedPipeServer;
        private AesEncryptionService? _encryptionService;
        private PipeServerConfiguration? _serverConfiguration;
        private PipeClientConfiguration? _clientConfiguration;

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

            _serverConfiguration = new PipeServerConfiguration();
            _serverConfiguration.Initialize(
                null,
                "TestPipe",
                _serverLogger,
                _encryptionService,
                new CancellationTokenSource());
        }

        private void InitializeLogger()
        {
            _serverLogger =
                ListBoxLogger.RouteLoggerToListBox(Dispatcher, LstServerLog, "NamedPipeServerTestHarness", "Server");
            _serverLogger?.LogInformation("Server logger initialized in test harness.");

            _clientLogger =
                ListBoxLogger.RouteLoggerToListBox(Dispatcher, LstClientLog, "NamedPipeClientTestHarness", "Client");
            _clientLogger?.LogInformation("Client logger initialized in test harness.");
        }

        private void InitializeServer()
        {
            _namedPipeServer = new NamedPipeServer(_serverConfiguration);
            _namedPipeServer.MessageReceived += NamedPipeServerMessageReceived;
            _namedPipeServer.ResponseRequested += NamedPipeServerResponseRequested;
            _namedPipeServer.ServerStateChanged += NamedPipeServerStateChanged;
        }

        private async void btnStartServer_Click(object sender, RoutedEventArgs e)
        {
            await _namedPipeServer?.StartAsync()!;
            txtServerStatus.Text = "Running";
        }

        private void btnStopServer_Click(object sender, RoutedEventArgs e)
        {
            _namedPipeServer?.Configuration.CancellationTokenSource.Cancel();
            txtServerStatus.Text = "Not running";
        }

        private async void btnSendTestMessage_Click(object sender, RoutedEventArgs e)
        {
            var envelope = new PipeEnvelope<TestObject>(_clientLogger)
            {
                Payload = new TestObject()
            };

            if (_clientConfiguration == null)
            {
                _clientConfiguration = new PipeClientConfiguration();
                _clientConfiguration.Initialize(
                    ".",
                    "TestPipe",
                    _clientLogger,
                    _encryptionService,
                    new CancellationTokenSource());
            }

            var client = new NamedPipeClient(_clientConfiguration, _clientLogger);
            var response = await client.SendAsync(envelope);

            _clientConfiguration.Logger.LogInformation($"FROM PIPE CLIENT- Message received: {response}");
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
            Dispatcher.Invoke(() =>
            {
                LstServerLog.Items.Add($"UI Server Log Capture- Received message: {args.Json}");
            });

            var parsedObject = args.Json.FromJsonToType<PipeEnvelope<TestObject>>();
            var logMessage = $"UI Server Log Capture- Parsed object:\n" +
                             $"Type- {parsedObject?.TypeName ?? "null"}\n" +
                             $"Name- {parsedObject?.Payload?.Name}\n" +
                             $"Source- {parsedObject?.Payload?.Source}\n" +
                             $"Payload Id- {parsedObject?.Payload?.Id}\n";

            _serverLogger?.LogInformation(logMessage);
        }

        private Task NamedPipeServerResponseRequested(object sender, PipeResponseRequestedEventArgs args)
        {
            var logMessage = $"UI Server Log Capture- Response requested for message ID " +
                             $"{args.ResponseObject.MessageId}: " +
                             $"{args.ResponseObject.ToJson()}";
            
            _serverLogger?.LogInformation(logMessage);

            return Task.CompletedTask;
        }

        private Task NamedPipeServerStateChanged(object sender, PipeServerStateChangedEventArgs args)
        {
            var logMessage = args.GetLogMessage();
            Dispatcher.Invoke(() =>
            {
                LstServerLog.Items.Add(
                    logMessage
                );
            });

            return Task.CompletedTask;
        }
    }
}
