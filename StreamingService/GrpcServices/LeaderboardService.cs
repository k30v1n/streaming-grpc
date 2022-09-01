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
        IDisposable? observerSubscription = null;
        try
        {
            var cancellationToken = context.CancellationToken;

            observerSubscription = _leaderboardObservable.Subscribe(leaderboards =>
            {
                _logger.LogDebug("Executing on next...");
                
                var result = new LeaderboardReply();
                result.Data.AddRange(leaderboards);

                responseStream.WriteAsync(result);

            });

            await Task.Factory.StartNew(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(100);
                }
                _logger.LogDebug("Cancellation requested from the client...");
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
        finally
        {
            observerSubscription?.Dispose();
        }
    }
}
