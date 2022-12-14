using StreamingService.Dto;
using StreamingService.Services;
using StreamingService.Workers;
using System.Reactive.Subjects;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

builder.Services.AddSingleton<ISubject<LeaderboardDto>, Subject<LeaderboardDto>>();

builder.Services.AddHostedService<GeneralLeaderboardWorker>();

builder.Services.AddGrpc();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Configure the HTTP request pipeline.
app.MapGrpcService<LeaderboardGrpcService>();
app.MapGet("/", () => "Welcome to the Streaming Service");

logger.LogInformation("Starting application.");
app.Run();
