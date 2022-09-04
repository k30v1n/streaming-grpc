using StreamingService.Dto;
using System.Reactive.Subjects;

namespace StreamingService.Workers
{
    public class GeneralLeaderboardWorker : BackgroundService
    {
        private readonly ILogger<GeneralLeaderboardWorker> _logger;
        private readonly ISubject<LeaderboardDto> _leaderboardSubject;

        #region mocking data
        private readonly Random _random;
        private readonly Dictionary<int, List<PersonPointsDto>> _leaderboadPointsStorage;
        #endregion mocking data

        public GeneralLeaderboardWorker(
            ILogger<GeneralLeaderboardWorker> logger,
            ISubject<LeaderboardDto> leaderboardSubject)
        {
            _logger = logger;
            _leaderboardSubject = leaderboardSubject;
            
            _random = new ();
            _leaderboadPointsStorage = new ();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                var randomLeaderboardIdToUpdate = _random.Next(1, 3);

                var data = GetLeaderboard(randomLeaderboardIdToUpdate);

                var next = new LeaderboardDto
                {
                    LeaderBoardId = randomLeaderboardIdToUpdate,
                    Data = data.AsReadOnly()
                };

                _logger.LogDebug($"Publishing for leaderboard `{next.LeaderBoardId}`");

                _leaderboardSubject.OnNext(next);

                await Task.Delay(1000);
            }

            _logger.LogWarning("Background job is shutting down...");
            _leaderboardSubject.OnCompleted();
        }

        private List<PersonPointsDto> GetLeaderboard(int leadearboardId)
        {
            if (_leaderboadPointsStorage.TryGetValue(leadearboardId, out var result))
            {
                // Generate new value
                return result;
            }

            result = new List<PersonPointsDto>();

            var personNames = new string[5];
            for (int i = 0; i < personNames.Length; i++)
            {
                personNames[i] = Faker.Name.FullName();
                result.Add(new ()
                {
                    Person = personNames[i],
                    Position = i+1,
                    Points = 0
                });
            }

            _leaderboadPointsStorage.Add(leadearboardId, result);

            return result;
        }
    }
}
