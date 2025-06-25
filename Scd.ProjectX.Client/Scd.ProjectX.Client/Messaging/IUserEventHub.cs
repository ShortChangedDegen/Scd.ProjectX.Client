using Scd.ProjectX.Client.Messaging.Dispatchers;
using Scd.ProjectX.Client.Models.Account;
using Scd.ProjectX.Client.Models.Orders;
using Scd.ProjectX.Client.Models.Positions;
using Scd.ProjectX.Client.Models.Trades;

namespace Scd.ProjectX.Client.Messaging
{
    /// <summary>
    /// Defines the required members of the user event hub for ProjectX.
    /// </summary>
    public interface IUserEventHub : IDisposable
    {
        /// <summary>
        /// Gets an <see cref="EventDispatcher{TEvent}"/> for <see cref="UserAccountEvent"/>s.
        /// </summary>
        IEventDispatcher<UserAccountEvent> UserAccountHub { get; }

        /// <summary>
        /// Gets an <see cref="EventDispatcher{TEvent}"/> for <see cref="UserOrderEvent"/>s.
        /// </summary>
        IEventDispatcher<UserOrderEvent> UserOrderHub { get; }

        /// <summary>
        /// Gets an <see cref="EventDispatcher{TEvent}"/> for <see cref="UserPositionEvent"/>s.
        /// </summary>
        IEventDispatcher<UserPositionEvent> UserPositionHub { get; }

        /// <summary>
        /// Gets an <see cref="EventDispatcher{TEvent}"/> for <see cref="UserTradeEvent"/>s.
        /// </summary>
        IEventDispatcher<UserTradeEvent> UserTradeHub { get; }

        /// <summary>
        /// Starts the <see cref="MarketEventHub"/>.
        /// </summary>
        /// <returns>A task.</returns>
        Task StartAsync();

        /// <summary>
        /// Subscribes one or more observers to <see cref="UserAccountEvent">s.
        /// </summary>
        /// <param name="accountIds">One or more account IDs to listen for.</param>
        /// <param name="observers">One or more observers.</param>
        Task Subscribe(params IObserver<UserAccountEvent>[] observers);

        /// <summary>
        /// Subscribes one or more observers to <see cref="UserOrderEvent">.
        /// </summary>
        /// <param name="accountIds">One or more account IDs to listen for.</param>
        /// <param name="observers">One or more observers.</param>
        Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserOrderEvent>[] observers);

        /// <summary>
        /// Subscribes one or more observers to <see cref="UserPositionEvent">.
        /// </summary>
        /// <param name="accountIds">One or more account IDs to listen for.</param>
        /// <param name="observers">One or more observers.</param>
        Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserPositionEvent>[] observers);

        /// <summary>
        /// Subscribes one or more observers to <see cref="UserTradeEvent">.
        /// </summary>
        /// <param name="accountIds">One or more account IDs to listen for.</param>
        /// <param name="observers">One or more observers.</param>
        Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserTradeEvent>[] observers);
    }
}