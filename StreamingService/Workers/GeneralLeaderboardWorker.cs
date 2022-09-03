using StreamingService.Observables;

namespace StreamingService.Workers
{
    public class GeneralLeaderboardWorker : BackgroundService
    {
        private readonly ILogger<GeneralLeaderboardWorker> _logger;
        private readonly LeaderboardObservable _leaderboardObservable;

        public GeneralLeaderboardWorker(
            ILogger<GeneralLeaderboardWorker> logger,
            LeaderboardObservable leaderboardObservable)
        {
            _logger = logger;
            _leaderboardObservable = leaderboardObservable;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000);
            }

            _logger.LogWarning("Background job is shutting down...");
        }
    }
}
