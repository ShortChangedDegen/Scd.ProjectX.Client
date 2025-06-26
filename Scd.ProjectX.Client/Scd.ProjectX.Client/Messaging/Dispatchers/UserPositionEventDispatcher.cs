using Microsoft.AspNetCore.SignalR.Client;
using Scd.ProjectX.Client.Models.Positions;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MarketTradeDispatcher"/> class.
    /// </summary>
    /// <param name="connection">The <see cref="HubConnection"/>.</param>
    public class UserPositionEventDispatcher(HubConnection connection) :
        EventDispatcher<UserPositionEvent>(connection, "GatewayUserPosition"),
        IEventDispatcher<UserPositionEvent>
    {
    }
}