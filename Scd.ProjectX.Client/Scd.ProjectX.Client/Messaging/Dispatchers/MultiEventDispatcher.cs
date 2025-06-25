using Microsoft.AspNetCore.SignalR.Client;
using Scd.ProjectX.Client.Models;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    public abstract class MultiEventDispatcher<T, TEvent, TId> :
        EventDispatcher<TEvent>,
        IEventDispatcher<TEvent>
        where T : ICollection<TEvent>, new()
        where TEvent : IEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventDispatcher{TEvent}"/>
        /// class with the specified <paramref name="connection"/>.
        /// </summary>
        /// <param name="connection">The <see cref="HubConnection"/>.</param>
        /// /// <param name="publishMethodName">The method name used to subscribe to published events.</param>
        protected MultiEventDispatcher(HubConnection connection, string publishMethodName)
            : base(connection, publishMethodName)
        {
            hubConnection = Guard.NotNull(connection, nameof(connection));
            publishMethod = Guard.NotNullOrEmpty(publishMethodName, nameof(publishMethodName));
        }

        /// <summary>
        /// Initializes the dispatcher's subscription.
        /// </summary>
        public override void Init() =>
            hubConnection.On<TId, T>(PublishMethodName, this.Publish);

        /// <summary>
        /// Publishes an event to all observers and stores it in the event list.
        /// </summary>
        /// <param name="event">The <see cref="IEvent"/>.</param>
        public void Publish(TId id, T events)
        {
            Guard.NotNull(events, nameof(events));
            foreach (var @event in events)
            {
                Guard.NotNull(@event, nameof(@event));
                base.events.Add(@event);
                foreach (var observer in observers)
                {
                    observer.OnNext(@event);
                }
            }
        }
    }
}