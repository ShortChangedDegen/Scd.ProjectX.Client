using Microsoft.Extensions.Options;
using Scd.ProjectX.Client.Messaging;
using Scd.ProjectX.Client.Models.Account;
using Scd.ProjectX.Client.Models.MarketData;
using Scd.ProjectX.Client.Models.Orders;
using Scd.ProjectX.Client.Models.Positions;
using Scd.ProjectX.Client.Models.Trades;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client
{
    /// <summary>
    /// ProjectXHub is a central hub for managing subscriptions to various market and user events.
    /// </summary>
    public class ProjectXHub : IDisposable, IProjectXHub
    {
        private bool _isDisposed;
        private List<IDisposable> _subscriptions = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectXHub"/> class with the specified market and user hubs.
        /// </summary>
        /// <param name="marketHub">A market hub.</param>
        /// <param name="userHub">A user hub.</param>
        internal ProjectXHub(IMarketEventHub marketHub, IUserEventHub userHub)
        {
            UserHub = Guard.NotNull(userHub, nameof(userHub));
            MarketHub = Guard.NotNull(marketHub, nameof(marketHub));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectXHub"/> class with the specified settings.
        /// </summary>
        /// <param name="api">The ProjectX API.</param>
        /// <param name="settings">The ProjectX settings.</param>
        public ProjectXHub(IAuthTokenHandler authTokenHandler, IOptions<ProjectXSettings> settings)
            : this(new MarketEventHub(authTokenHandler, settings), new UserEventHub(authTokenHandler, settings))
        {
        }

        /// <summary>
        /// Gets the <see cref="UserHub"/>.
        /// </summary>
        public IUserEventHub UserHub { get; }

        /// <summary>
        /// Gets the <see cref="MarketHub"/>.
        /// </summary>
        public IMarketEventHub MarketHub { get; }

        /// <summary>
        /// Subscribes one or more observers for <see cref="MarketQuoteEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="contractIds">One or more instrument contract IDS.</param>
        public async Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketQuoteEvent>[] observers) =>
            await MarketHub.Subscribe(contractIds, observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="MarketTradeEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="contractIds">One or more instrument contract IDS.</param>
        public async Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketTradeEvent>[] observers) =>
            await MarketHub.Subscribe(contractIds, observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="MarketDepthEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="contractIds">One or more instrument contract IDS.</param>
        public async Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketDepthEvent>[] observers) =>
            await MarketHub.Subscribe(contractIds, observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="UserAccountEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        public async Task Subscribe(params IObserver<UserAccountEvent>[] observers) =>
            await UserHub.Subscribe(observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="UserOrderEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="accountIds">One or more account IDS.</param>
        public async Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserOrderEvent>[] observers) =>
            await UserHub.Subscribe(accountIds, observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="UserPositionEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="accountIds">One or more account IDS.</param>
        public async Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserPositionEvent>[] observers) =>
            await UserHub.Subscribe(accountIds, observers);

        /// <summary>
        /// Subscribes one or more observers for <see cref="UserTradeEvent"/>s.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        /// <param name="accountIds">One or more account IDS.</param>
        public async Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserTradeEvent>[] observers) =>
            await UserHub.Subscribe(accountIds, observers);

        /// <summary>
        /// Start the event hubs.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task StartAsync() =>
            await Task.WhenAll(MarketHub.StartAsync(), UserHub.StartAsync());

        /// <summary>
        /// Disposes the resources used by the <see cref="ProjectXHub"/> instance.
        /// </summary>
        /// <param name="disposing">Whether to clean up managed resource.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _subscriptions.ForEach(s => s.Dispose());
                }

                _isDisposed = true;
            }
        }

        /// <summary>
        /// Disposes the resources used by the <see cref="ProjectXHub"/> instance.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}