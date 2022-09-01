
using StreamingService.Observable;
using StreamingService.Services;
using StreamingService.Workers;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

builder.Services.AddSingleton<LeaderboardObservable>();
builder.Services.AddHostedService<GeneralLeaderboardWorker>();

builder.Services.AddGrpc();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Configure the HTTP request pipeline.
app.MapGrpcService<LeaderboardService>();
app.MapGet("/", () => "Welcome to the Streaming Service");

logger.LogInformation("Starting application.");
app.Run();
