using Microsoft.AspNetCore.SignalR.Client;
using Scd.ProjectX.Client.Models.Trades;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserTradeEventDispatcher"/> class.
    /// </summary>
    /// <param name="connection">The <see cref="HubConnection"/>.</param>
    public class UserTradeEventDispatcher(HubConnection connection) :
        EventDispatcher<UserTradeEvent>(connection, "GatewayUserTrade"),
        IEventDispatcher<UserTradeEvent>
    {
    }
}