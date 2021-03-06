using System;
using MyNatsClient.Internals;

namespace MyNatsClient.Extensions
{
    public static class NatsObservableExtensions
    {
        public static INatsObservable<TResult> OfType<TResult>(this INatsObservable<object> ob) where TResult : class
            => new OfTypeObservable<TResult>(ob);

        public static INatsObservable<TResult> Cast<TSource, TResult>(this INatsObservable<TSource> ob) where TSource : class where TResult : class
            => new CastObservable<TSource, TResult>(ob);

        public static INatsObservable<T> Where<T>(this INatsObservable<T> ob, Func<T, bool> predicate) where T : class
            => new WhereObservable<T>(ob, predicate);

        public static IDisposable Subscribe<T>(this INatsObservable<T> ob, Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
            => ob.Subscribe(NatsObserver.Delegating(onNext, onError, onCompleted));

        public static IDisposable SubscribeSafe<T>(this INatsObservable<T> ob, Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
            => ob.Subscribe(NatsObserver.Safe(onNext, onError, onCompleted));

        public static IDisposable SubscribeSafe<T>(this INatsObservable<T> ob, IObserver<T> observer) where T : class
            => ob.Subscribe(NatsObserver.Safe<T>(observer.OnNext, observer.OnError, observer.OnCompleted));
    }
}