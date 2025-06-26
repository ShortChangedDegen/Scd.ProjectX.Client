using Scd.ProjectX.Client.Messaging.Dispatchers;
using Scd.ProjectX.Client.Models.Account;
using Scd.ProjectX.Client.Models.Orders;
using Scd.ProjectX.Client.Models.Positions;
using Scd.ProjectX.Client.Models.Trades;

namespace Scd.ProjectX.Client.Messaging
{
    public interface IUserEventHub : IDisposable
    {
        IEventDispatcher<UserAccountEvent> UserAccountHub { get; }
        IEventDispatcher<UserOrderEvent> UserOrderHub { get; }
        IEventDispatcher<UserPositionEvent> UserPositionHub { get; }
        IEventDispatcher<UserTradeEvent> UserTradeHub { get; }

        Task StartAsync();

        Task Subscribe(params IObserver<UserAccountEvent>[] observers);

        Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserOrderEvent>[] observers);

        Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserPositionEvent>[] observers);

        Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserTradeEvent>[] observers);
    }
}