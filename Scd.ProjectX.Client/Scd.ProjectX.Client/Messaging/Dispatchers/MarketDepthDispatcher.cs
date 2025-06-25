using Microsoft.AspNetCore.SignalR.Client;
using Scd.ProjectX.Client.Models.MarketData;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MarketDepthDispatcher"/> class.
    /// </summary>
    public class MarketDepthDispatcher(HubConnection connection) :
        MultiEventDispatcher<List<MarketDepthEvent>, MarketDepthEvent, string>(connection, "GatewayDepth", "UnsubscribeContractMarketDepth"),
        IEventDispatcher<MarketDepthEvent>
    {
    }
}