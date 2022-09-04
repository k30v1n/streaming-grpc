using StreamingService.Dto;
using System.Reactive.Subjects;

namespace StreamingService.Workers
{
    public class GeneralLeaderboardWorker : BackgroundService
    {
        private readonly ILogger<GeneralLeaderboardWorker> _logger;
        private readonly ISubject<LeaderboardDto> _leaderboardSubject;

        public GeneralLeaderboardWorker(
            ILogger<GeneralLeaderboardWorker> logger,
            ISubject<LeaderboardDto> leaderboardSubject)
        {
            _logger = logger;
            _leaderboardSubject = leaderboardSubject;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var next = new LeaderboardDto
                {
                    LeaderBoardId = 1,
                    Data = new List<PersonPointsDto>().AsReadOnly()
                };

                _logger.LogDebug($"Publishing for leaderboard `{next.LeaderBoardId}`");

                _leaderboardSubject.OnNext(next);

                await Task.Delay(5000);
            }

            _logger.LogWarning("Background job is shutting down...");
            _leaderboardSubject.OnCompleted();
        }
    }
}
