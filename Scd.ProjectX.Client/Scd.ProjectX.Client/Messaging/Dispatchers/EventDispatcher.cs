using Microsoft.AspNetCore.SignalR.Client;
using Scd.ProjectX.Client.Models;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    /// <summary>
    /// Base class for event hubs that handle events of type <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The type of <see cref="IEvent"/>.</typeparam>
    public abstract class EventDispatcher<TEvent> : IEventDispatcher<TEvent>
        where TEvent : IEvent
    {
        protected HubConnection hubConnection;
        protected bool isDisposed;
        protected List<IObserver<TEvent>> observers = new();

        // This needs to be moved to a cache or a persisted store.
        protected List<TEvent> events = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventDispatcher{TEvent}"/>
        /// class with the specified <paramref name="connection"/>.
        /// </summary>
        /// <param name="connection">The <see cref="HubConnection"/>.</param>
        /// <param name="publishMethodName">The method name used to subscribe to published events.</param>
        /// <param name="unsubscribeMethodName"> The method name used to unsubscribe from events.</param>
        protected EventDispatcher(HubConnection connection, string publishMethodName, string unsubscribeMethodName)
        {
            hubConnection = Guard.NotNull(connection, nameof(connection));
            PublishMethodName = Guard.NotNullOrEmpty(publishMethodName, nameof(publishMethodName));
            UnsubscribeMethodName = Guard.NotNullOrEmpty(unsubscribeMethodName, nameof(unsubscribeMethodName));
            hubConnection.On<TEvent>(PublishMethodName, Publish);
        }

        /// <summary>
        /// Gets the name of the method used to subscribe to events
        /// from the hub.
        /// </summary>
        public virtual string PublishMethodName
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the name of the method used to unsubscribe to events
        /// from the hub.
        /// </summary>
        public virtual string UnsubscribeMethodName
        {
            get;
            protected set;
        }

        /// <summary>
        /// Publishes an event to all observers and stores it in the event list.
        /// </summary>
        /// <param name="event">The <see cref="IEvent"/>.</param>
        public virtual void Publish(TEvent @event)
        {
            Guard.NotNull(@event, nameof(@event));
            events.Add(@event);
            foreach (var observer in observers)
            {
                observer.OnNext(@event);
            }
        }

        /// <summary>
        /// Subscribes an observer to the event hub.
        /// </summary>
        /// <param name="observer">The <see cref="IObserver{TEvent}"/>.</param>
        /// <returns>A disposable instance of the <see cref="IObserver{TEvent}"/>.</returns>
        public virtual IDisposable Subscribe(IObserver<TEvent> observer)
        {
            Guard.NotNull(observer, nameof(observer));
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                foreach (var @event in events)
                {
                    observer.OnNext(@event);
                }
            }
            return new Unsubscriber<TEvent>(observers, observer);
        }

        /// <summary>
        /// Unsubscribes from the event hub using the specified method name.
        /// </summary>
        /// <param name="unsubscribeMethod">The hub method to unsubscribe from events during cleanup.</param>
        /// <exception cref="ProjectXClientException"></exception>
        public virtual async Task Unsubscribe()
        {
            try
            {
                await hubConnection.InvokeAsync<TEvent>(UnsubscribeMethodName);
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException($"Failed to unsubscribe from {UnsubscribeMethodName}.", ex);
            }
        }

        /// <summary>
        /// Disposes the resources used by the <see cref="EventDispatcher{TEvent}"/> class.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources used by the <see cref="EventDispatcher{TEvent}"/> class.
        /// </summary>
        /// <param name="disposing">true if disposing; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    events.Clear();
                    observers.Clear();
                }
                // This is shared. Just set locaL reference to null.
                isDisposed = true;
            }
        }
    }
}