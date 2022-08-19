namespace StreamingService.Workers
{
    public class GeneralLeaderboardWorker : BackgroundService
    {
        private readonly ILogger<GeneralLeaderboardWorker> _logger;

        public GeneralLeaderboardWorker(ILogger<GeneralLeaderboardWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Running {name}", nameof(GeneralLeaderboardWorker));

                await Task.Delay(5000);
            }

            _logger.LogWarning("Background job is shutting down...");
        }
    }
}
