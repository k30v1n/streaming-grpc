﻿
using System.Collections.Concurrent;
using System.Reactive;
using System.Reactive.Disposables;

namespace StreamingService.Observables;

public class LeaderboardObservable: ObservableBase<IReadOnlyCollection<PersonPoints>>
{
    private readonly ILogger<LeaderboardObservable> _logger;

    private ConcurrentDictionary<Guid, IObserver<IReadOnlyCollection<PersonPoints>>> _observers;

    public LeaderboardObservable(ILogger<LeaderboardObservable> logger)
    {
        _logger = logger;
        _observers = new ConcurrentDictionary<Guid, IObserver<IReadOnlyCollection<PersonPoints>>>();
    }

    protected override IDisposable SubscribeCore(IObserver<IReadOnlyCollection<PersonPoints>> observer)
    {
        var guid = Guid.NewGuid();

        _observers.TryAdd(guid, observer);

        return Disposable.Create(() =>
        {
            _logger.LogDebug($"Observer {guid} is unsubscribing");
            
            bool removed = _observers.TryRemove(guid, out _); // exponential retry possibility
            
            _logger.LogDebug("Observer {guid} removed from observable", guid);
        });
    }

    //public void PublishNew(IReadOnlyCollection<PersonPoints> personPoints)
    //{

    //}
}

