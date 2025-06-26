using Microsoft.AspNetCore.SignalR.Client;
using Scd.ProjectX.Client.Models.MarketData;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MarketTradeDispatcher"/> class.
    /// </summary>
    /// <param name="connection">The <see cref="HubConnection"/>.</param>
    public class MarketTradeDispatcher(HubConnection connection) :

        MultiEventDispatcher<List<MarketTradeEvent>, MarketTradeEvent, string>(connection, "GatewayTrade"),
        IEventDispatcher<MarketTradeEvent>
    {
    }
}