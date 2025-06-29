using Scd.ProjectX.Client.Models;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    /// <summary>
    /// Defines a contract for an event hub that can publish and
    /// subscribe to events of type <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventDispatcher<TEvent> : IObservable<TEvent>, IDisposable
        where TEvent : IEvent
    {
        /// <summary>
        /// Gets the name of the method used to subscribe to events
        /// from the hub.
        /// </summary>
        string PublishMethodName { get; }

        /// <summary>
        /// Publishes an event to all observers and stores it in the event list.
        /// </summary>
        /// <param name="event">The <see cref="IEvent"/>.</param>
        void Publish(TEvent @event);

        /// <summary>
        /// Initializes the dispatcher's subscription.
        /// </summary>
        void Init();

        /// <summary>
        /// Subscribes an observer to the event hub.
        /// </summary>
        /// <param name="observer">The <see cref="IObserver{TEvent}"/>.</param>
        /// <returns>A disposable instance of the <see cref="IObserver{TEvent}"/>.</returns>
        IDisposable Subscribe(IObserver<TEvent> observer);

        /// <summary>
        /// Unsubscribes from the event hub using the specified method name.
        /// </summary>
        /// <param name="unsubscribeMethod">The hub method to unsubscribe from events during cleanup.</param>
        /// <exception cref="ProjectXClientException"></exception>
        Task Unsubscribe();
    }
}