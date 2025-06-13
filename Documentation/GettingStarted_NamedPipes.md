# How to Use: Named Pipes

## 🛠 Creating a Named Pipe Server

To set up a pipe server that receives encrypted messages:

```csharp
// Setup the encryption service
var encryptionService = new AesEncryptionService(
    base64Key: "[your-base64-key]",
    logger: logger);

// Configure the pipe server
var config = new PipeServerConfiguration
{
    ServerName = ".",
    PipeName = "MyPipe",
    UseEncryption = true,
    EncryptionService = encryptionService,
    Logger = logger
};

// Optionally, use a cancellation token for a momentary server
config.CancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)); // momentary
// Or leave it running indefinitely for a perpetual server

// Create the server
var server = new NamedPipeServerCore(config);

// Hook into events
server.MessageReceived += (sender, args) =>
{
    var message = args.Deserialized.Payload;
    logger.LogInformation("Message received: {Payload}", message);
};

server.ResponseRequested += (sender, args) =>
{
    var responseBytes = Encoding.UTF8.GetBytes("Acknowledged");
    args.Server.WriteAsync(responseBytes, 0, responseBytes.Length);
};

// Start the server
await server.StartAsync();

```

## Creating a Named Pipe Client

To send a message to a pipe server:

```csharp
// Setup the encryption service
var encryptionService = new AesEncryptionService(
    base64Key: "[your-base64-key]",
    logger: logger);

// Configure the pipe client
var config = new PipeClientConfiguration
{
    ServerName = ".",
    PipeName = "MyPipe",
    UseEncryption = true,
    EncryptionService = encryptionService,
    CancellationTokenSource = new CancellationTokenSource(3000), // optional timeout
};

// Create the client
var client = new NamedPipeClient(config, logger);

// Prepare the message
var envelope = new PipeEnvelope<MyMessageType>(logger)
{
    Payload = new MyMessageType { Id = 1, Name = "Test" }
};

// Send the message
await client.SendMessage(envelope);
```
