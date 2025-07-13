using Microsoft.AspNetCore.SignalR.Client;
using Scd.ProjectX.Client.Models.MarketData;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MarketDepthDispatcher"/> class.
    /// </summary>
    public class MarketDepthDispatcher(HubConnection connection) :
        EventDispatcher<MarketDepthEvent>(connection, "GatewayDepth", "UnsubscribeContractMarketDepth"),
        IEventDispatcher<MarketDepthEvent>
    {
        public override void Init() =>
            hubConnection.On<string, List<MarketDepthEvent>>(PublishMethodName, Publish);


        /// <summary>
        /// Publishes an event to all observers and stores it in the event list.
        /// </summary>
        /// <param name="event">The <see cref="MarketDepthEvent"/>.</param>
        public void Publish(string id, List<MarketDepthEvent> @event)
        {
            Guard.NotNull(@event, nameof(@event));
            @event.ForEach(e => e.SymbolId = id);
            events.AddRange(@event);
            foreach (var observer in observers)
            {
                try
                {
                    @event.ForEach(e => observer.OnNext(e));
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            }
        }
    }
}