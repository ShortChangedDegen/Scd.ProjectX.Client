using Scd.ProjectX.Client.Messaging;
using Scd.ProjectX.Client.Messaging.Dispatchers;
using Scd.ProjectX.Client.Models.Account;
using Scd.ProjectX.Client.Models.MarketData;
using Scd.ProjectX.Client.Models.Orders;
using Scd.ProjectX.Client.Models.Positions;
using Scd.ProjectX.Client.Models.Trades;

namespace Scd.ProjectX.Client
{
    /// <summary>
    /// Defines the required members of the ProjectX hub.
    /// </summary>
    public interface IProjectXHub
    {
        /// <summary>
        /// Gets the <see cref="MarketHub"/>.
        /// </summary>
        IMarketEventHub MarketHub { get; }

        /// <summary>
        /// Gets an <see cref="EventDispatcher{TEvent}"/> for <see cref="UserAccountEvent"/>s.
        /// </summary>
        IUserEventHub UserHub { get; }

        /// <summary>
        /// Disposes the resources used by the <see cref="UserEventHub"/> class.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Subscribes one or more observers for <see cref="UserOrderEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="accountIds">One or more account IDS.</param>
        Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserOrderEvent>[] observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="UserPositionEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="accountIds">One or more account IDS.</param>
        Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserPositionEvent>[] observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="MarketTradeEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="contractIds">One or more instrument contract IDS.</param>
        Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserTradeEvent>[] observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="MarketDepthEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="contractIds">One or more instrument contract IDS.</param>
        Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketDepthEvent>[] observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="MarketQuoteEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="contractIds">One or more instrument contract IDS.</param>
        Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketQuoteEvent>[] observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="UserTradeEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="accountIds">One or more account IDS.</param>
        Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketTradeEvent>[] observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="UserAccountEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        Task Subscribe(params IObserver<UserAccountEvent>[] observers);

        /// <summary>
        /// Start the event hubs.
        /// </summary>
        /// <returns>A task.</returns>
        Task StartAsync();
    }
}