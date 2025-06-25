using Microsoft.AspNetCore.SignalR.Client;
using Scd.ProjectX.Client.Models.Orders;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserOrderEventDispatcher"/> class.
    /// </summary>
    /// <param name="connection">The <see cref="HubConnection"/>.</param>
    public class UserOrderEventDispatcher(HubConnection connection) :
        EventDispatcher<UserOrderEvent>(connection, "GatewayUserOrder", "UnsubscribeOrders"),
        IEventDispatcher<UserOrderEvent>
    {
    }
}