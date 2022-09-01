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
        try
        {
            var result = new LeaderboardReply();

            for (int i = 0; i < 10; i++)
            {
                result.Data.Add(new PersonPoints
                {
                    Position = 1,
                    Person = $"XYZ_{i}",
                    Points = 100000
                });

                await responseStream.WriteAsync(result);

                await Task.Delay(1000);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
