
using System.Reactive;

namespace StreamingService.Observable;

public class LeaderboardObservable: ObservableBase<PersonPoints>
{
    private readonly ILogger _logger;

    public LeaderboardObservable(ILogger logger)
    {
        _logger = logger;
    }

    protected override IDisposable SubscribeCore(IObserver<PersonPoints> observer)
    {
        throw new NotImplementedException();
    }
}


