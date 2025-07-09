using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.ConsoleUtilities;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.Logging;
using SeroGlint.DotNet.NamedPipes;
using SeroGlint.DotNet.NamedPipes.Objects;
using SeroGlint.DotNet.Security;

namespace SeroGlint.DotNet.ElevatedAgent
{
    internal class Program
    {
        private static ILogger? _logger;
        private static NamedPipeClient _namedPipeClient;
        private static string _menu;
        private static string _namedPipeName;
        private static int _retryCount = 3;
        private static TimeSpan _retryDelay = new TimeSpan(0, 0, 0, 5);

        /// <summary>
        /// This program is meant to allow for elevated execution without interrupting the triggering (parent) application
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {
            _logger = new LoggerFactoryBuilder()
                .EnableConsoleOutput()
                .EnableFileOutput(createLogPath: true, logPath: "Logs", logName: "ElevatedAgent")
                .BuildSerilog();

            BuildMenu();

            if (!ValidateArguments(args))
            {
                return;
            }

            if (!ValidateArgumentValues(args))
            {
                return;
            }

            InitializeNamedPipeClient();
            var applicationRunConfiguration = await TryConnectNamedPipe();
            var parsedConfiguration = await ParseRunParameters(applicationRunConfiguration);
            await ExecuteRequestProcess(parsedConfiguration);
        }

        private static async Task<string> TryConnectNamedPipe()
        {
            var configurationResponse = string.Empty;
            var message = new EaMessage()
                .WithOperationType("GetConfig")
                .WithPipeName(_namedPipeName)
                .WithRetryCount(_retryCount)
                .WithRetryDelay(_retryDelay)
                .WithResponseFormatType(typeof(EaConfigurationResponse))
                .WithMessage("Request for external application launch configuration")
                .BuildPipeEnvelope();

            for (var i = 0; i < _retryCount; i++)
            {
                _logger?.LogInformation("Attempting to connect to the named pipe server...");
                _logger?.LogInformation($"Named Pipe Name: {_namedPipeName}");
                _logger?.LogInformation($"Connection Attempt: {i + 1} of {_retryCount}");

                configurationResponse = await _namedPipeClient.SendAsync(message);

                if (!_namedPipeClient.IsConnected)
                {
                    Thread.Sleep(_retryDelay);
                    continue;
                }

                _logger?.LogInformation("Successfully connected to the named pipe server.");
            }

            return configurationResponse;
        }

        private static void BuildMenu()
        {
            var menuBuilder = new MenuBuilder(_logger)
                .WithTitle("Welcome to ElevatedAgent by Iterix!")
                .WithBodyText(
                    "This agent allows you to execute elevated commands without interrupting the parent application. The following arguments are required to leverage this system.")
                .WithLineSeparatorCharacter('=')
                .WithLineLength(50)
                .AsUnorderedList(false)
                .AddMenuOption("[STRING] PipeName (required)")
                .AddMenuOption("[INT] RetryAttemptCount (default 3 tries total)")
                .AddMenuOption("[INT] RetryAttemptDelay (default 5 seconds)");

            _menu = menuBuilder.Build();
        }

        private static bool ValidateArguments(string[] args)
        {
            if (args.Length < 4 && args.Length > 0)
            {
                return true;
            }

            _logger?.LogError("Insufficient arguments provided. Please provide the required arguments to run this agent.");
            _logger?.LogInformation("Arguments required: [RunElevated] [NamedPipeName] [ExecutablePath]");
            _logger?.LogInformation("Example: ElevatedAgent.exe true MyNamedPipe C:\\Path\\To\\Executable.exe");
            _logger?.LogInformation(_menu);
            return false;
        }

        private static bool ValidateArgumentValues(string[] args)
        {
            _namedPipeName = args.Length > 0 ? args[0] : string.Empty;
            if (!ValidatePipeNameArgument())
            {
                return false;
            }

            ValidateRetryParameters(args);

            _logger?.LogInformation($"Named Pipe Name: {_namedPipeName}");
            _logger?.LogInformation($"Retry Count: {_retryCount}");
            _logger?.LogInformation($"Retry Delay: {_retryDelay.TotalSeconds} seconds");
            return true;
        }

        private static void ValidateRetryParameters(string[] args)
        {
            if (args.Length > 1 && int.TryParse(args[1], out var retryCount) && retryCount > 0)
            {
                _retryCount = retryCount;
            }
            else
            {
                _logger?.LogInformation("No valid retry count provided. Using default value of 3.");
            }

            if (args.Length > 2 && TimeSpan.TryParse(args[2], out var retryDelay) && retryDelay.TotalSeconds > 0)
            {
                _retryDelay = retryDelay;
            }
            else
            {
                _logger?.LogInformation("No valid retry delay provided. Using default value of 5 seconds.");
            }
        }

        private static bool ValidatePipeNameArgument()
        {
            if (_namedPipeName.IsNullOrWhitespace())
            {
                _logger?.LogError("No named pipe name provided. Please provide a valid named pipe name.");
                _logger?.LogInformation(_menu);
                return false;
            }

            switch (_namedPipeName.Length)
            {
                case < 3:
                    _logger?.LogError("Named pipe name is too short. Please provide a valid named pipe name.");
                    _logger?.LogInformation(_menu);
                    return false;
                case > 256:
                    _logger?.LogError("Named pipe name is too long. Please provide a valid named pipe name.");
                    _logger?.LogInformation(_menu);
                    return false;
            }

            return true;
        }

        private static void InitializeNamedPipeClient()
        {
            var namedPipeConfiguration = new PipeClientConfiguration();
            namedPipeConfiguration.Initialize(
                serverName: ".",
                pipeName: _namedPipeName,
                encryptionService: new AesEncryptionService(AesEncryptionService.GenerateKey(), _logger),
                cancellationTokenSource: new CancellationTokenSource(),
                logger: _logger
            );

            _namedPipeClient = new NamedPipeClient(namedPipeConfiguration, _logger);
        }

        private static async Task<EaConfigurationResponse?> ParseRunParameters(string configurationResponse)
        {
            if (configurationResponse.IsNullOrWhitespace())
            {
                _logger?.LogError("No configuration response received from the named pipe server.");
                await _namedPipeClient.SendAsync(
                    new EaMessage()
                        .WithOperationType("OperationFailed")
                        .WithPipeName(_namedPipeName)
                        .WithMessage("No configuration response received from the named pipe server.")
                        .BuildPipeEnvelope());
                return null;
            }

            EaConfigurationResponse? response;
            try
            {
                response = configurationResponse.FromJsonToType<EaConfigurationResponse>();
                
                if (response == null)
                {
                    _logger?.LogError("Failed to parse the configuration response from the named pipe server.");
                    return null;
                }

                _logger?.LogInformation("Configuration response received from the named pipe server");
            }
            catch (Exception ex)
            {
                await _namedPipeClient.SendAsync(new EaMessage()
                    .WithOperationType("OperationFailed")
                    .WithPipeName(_namedPipeName)
                    .WithMessage($"Failed to parse the configuration response: {ex.Message}")
                    .BuildPipeEnvelope());
                _logger?.LogError(ex, "Failed to parse the configuration response from the named pipe server.");

                return null;
            }

            return response;
        }

        private static async Task ExecuteRequestProcess(EaConfigurationResponse parsedConfiguration)
        {
            await _namedPipeClient.SendAsync(new EaMessage()
                .WithOperationType("Event")
                .WithPipeName(_namedPipeName)
                .WithMessage("Attempting to execute application with given configuration.")
                .BuildPipeEnvelope());

            _logger?.LogInformation("Executing application with the provided configuration...");

            ExecuteApplicationLaunch(parsedConfiguration);
        }

        private static void ExecuteApplicationLaunch(EaConfigurationResponse parsedConfiguration)
        {
            var processInfo = new ProcessStartInfo()
            {
                // TODO launch as admin based on config arguments
            };
        }
    }
}
