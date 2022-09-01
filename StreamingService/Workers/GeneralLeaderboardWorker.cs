using StreamingService.Observable;

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
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Running {name}", nameof(GeneralLeaderboardWorker));

            //}

            //_leaderboardObservable.sub


            await Task.Delay(5000);
            _logger.LogWarning("Background job is shutting down...");
        }
    }
}
