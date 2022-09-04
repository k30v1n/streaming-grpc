using Grpc.Core;
using StreamingService.Dto;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace StreamingService.Services;

public class LeaderboardGrpcService : LeaderboardService.LeaderboardServiceBase
{
    private readonly ILogger<LeaderboardGrpcService> _logger;
    private readonly ISubject<LeaderboardDto> _leaderboardSubject;

    public LeaderboardGrpcService(
        ILogger<LeaderboardGrpcService> logger,
        ISubject<LeaderboardDto> leaderboardSubject)
    {
        _logger = logger;
        _leaderboardSubject = leaderboardSubject;
    }

    public override async Task Stream(LeaderboardRequest request, IServerStreamWriter<LeaderboardReply> responseStream, ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;

        using var observerSubscription = _leaderboardSubject
            .Where(leaderboard => leaderboard.LeaderBoardId == request.Leaderboard)
            .Subscribe(leaderboards =>
            {
                _logger.LogDebug("Executing on next...");

                LeaderboardReply result = new ();
                result.Data.AddRange(leaderboards.Data.Select(x => new PersonPoints {
                    Person = x.Person,
                    Points = x.Points,
                    Position = x.Position 
                }));

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
