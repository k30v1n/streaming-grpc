using Grpc.Core;
using StreamingService.Observable;
using static StreamingService.Points;

namespace StreamingService.Services;

public class LeaderboardService : PointsBase
{
    private readonly ILogger<LeaderboardService> _logger;
    private readonly LeaderboardObservable _leaderboardObservable;

    public LeaderboardService(ILogger<LeaderboardService> logger, LeaderboardObservable leaderboardObservable)
    {
        _logger = logger;
        _leaderboardObservable = leaderboardObservable;
    }

    public override async Task Stream(PointsRequest request, IServerStreamWriter<LeaderboardReply> responseStream, ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;

        using var observerSubscription = _leaderboardObservable.Subscribe(leaderboards =>
        {
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
