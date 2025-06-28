using Microsoft.AspNetCore.SignalR.Client;
using Scd.ProjectX.Client.Models.Account;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserAccountDispatcher"/> class.
    /// </summary>
    /// <param name="connection">The <see cref="HubConnection"/>.</param>
    public class UserAccountDispatcher :
        EventDispatcher<UserAccountEvent>,
        IEventDispatcher<UserAccountEvent>
    {
        public UserAccountDispatcher(HubConnection connection)
            : base(connection, "GatewayUserAccount", "UnsubscribeAccounts")
        {
        }
    }
}