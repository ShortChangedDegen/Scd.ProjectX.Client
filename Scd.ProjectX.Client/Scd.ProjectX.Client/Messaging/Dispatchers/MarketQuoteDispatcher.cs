using Microsoft.AspNetCore.SignalR.Client;
using Scd.ProjectX.Client.Models.MarketData;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MarketQuoteDispatcher"/> class.
    /// </summary>
    public class MarketQuoteDispatcher
        : EventDispatcher<MarketQuoteEvent>,
            IEventDispatcher<MarketQuoteEvent>
    {
        public MarketQuoteDispatcher(HubConnection connection)
            : base(connection, "GatewayQuote", "UnsubscribeContractQuotes")
        {            
        }

        public override void Init() =>
            hubConnection.On<string, MarketQuoteEvent>(PublishMethodName, Publish);

        /// <summary>
        /// Publishes an event to all observers and stores it in the event list.
        /// </summary>
        /// <param name="event">The <see cref="IEvent"/>.</param>
        public void Publish(string id, MarketQuoteEvent @event)
        {
            Guard.NotNull(@event, nameof(@event));
            @event.SymbolId = id;
            events.Add(@event);
            foreach (var observer in observers)
            {
                try
                {
                    observer.OnNext(@event);
                }
                catch (Exception ex) 
                { 
                    observer.OnError(ex);
                }
            }
        }
    }
}