using System.Reactive;
using System.Reactive.Disposables;
using StreamingService.Dto;

namespace StreamingService.Observables;

public class LeaderboardObservable : ObservableBase<LeaderboardDto>
{
    private readonly ILogger<LeaderboardObservable> _logger;

    private event Action<LeaderboardDto>? ObserversOnNextEvents;

    public LeaderboardObservable(ILogger<LeaderboardObservable> logger)
    {
        _logger = logger;
    }

    public void PublishNext(LeaderboardDto leaderboardDto) => ObserversOnNextEvents?.Invoke(leaderboardDto);

    protected override IDisposable SubscribeCore(IObserver<LeaderboardDto> observer)
    {
        _logger.LogDebug("New observer being subscribed");
        ObserversOnNextEvents += observer.OnNext;

        return Disposable.Create(() =>
        {
            _logger.LogDebug("Observer is unsubscribing");
            ObserversOnNextEvents -= observer.OnNext;
        });
    }
}


