using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using SolisCloudApi.Client;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
        .AddConsole();
});

ILogger logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("Program started");
var cts = new CancellationTokenSource();
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
var apiClient = new ApiClient(client, logger);
await apiClient.GetStationList(cts.Token);