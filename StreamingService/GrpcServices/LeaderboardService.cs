using Grpc.Core;
using StreamingService.Observables;

namespace StreamingService.Services;

public class LeaderboardGrpcService : LeaderboardService.LeaderboardServiceBase
{
    private readonly ILogger<LeaderboardGrpcService> _logger;
    private readonly LeaderboardObservable _leaderboardObservable;

    public LeaderboardGrpcService(ILogger<LeaderboardGrpcService> logger, LeaderboardObservable leaderboardObservable)
    {
        _logger = logger;
        _leaderboardObservable = leaderboardObservable;
    }

    public override async Task Stream(LeaderboardRequest request, IServerStreamWriter<LeaderboardReply> responseStream, ServerCallContext context)
    {
        // Eu sou o observer
        // Eu quero observar o Leaderboard de ID 1

        var cancellationToken = context.CancellationToken;
        

        using var observerSubscription = _leaderboardObservable
            .Subscribe(leaderboards =>
        {
            // verificar se esse eh o leaderboard do meu interesse?

            _logger.LogDebug("Executing on next...");

            var result = new LeaderboardReply();

            result.Data.AddRange(leaderboards);

            responseStream.WriteAsync(result);

        });

        
        var waitUserCancellation = await Task.Factory.StartNew(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(100);
            }
            _logger.LogDebug("Cancellation requested from the client...");
        });

        await waitUserCancellation;
        _logger.LogDebug("Stream is ending.");
    }
}
