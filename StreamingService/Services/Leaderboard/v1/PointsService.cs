using Grpc.Core;
using static StreamingService.Points;

namespace StreamingService.Services.Leaderboard.v1;

public class PointsService : PointsBase
{
    private readonly ILogger<PointsService> _logger;
    public PointsService(ILogger<PointsService> logger)
    {
        _logger = logger;
    }

    public override async Task Stream(PointsRequest request, IServerStreamWriter<LeaderboardReply> responseStream, ServerCallContext context)
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
}
