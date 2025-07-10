using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Scd.ProjectX.Client.Messaging.Dispatchers;
using Scd.ProjectX.Client.Models;
using Scd.ProjectX.Client.Models.MarketData;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Messaging
{
    /// <summary>
    /// Represents a hub for market events such as quotes, trades, and depth updates.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MarketEventHub"/> class.
    /// </remarks>
    /// <param name="projectXSettings">The ProjectX settings.</param>
    public class MarketEventHub : IDisposable, IMarketEventHub
    {
        protected List<IDisposable> subscribers = new();

        protected IEventDispatcher<MarketQuoteEvent>? marketQuoteHub;
        protected IEventDispatcher<MarketTradeEvent>? marketTradeHub;
        protected IEventDispatcher<MarketDepthEvent>? marketDepthHub;

        protected IHubConnectionBuilder hubBuilder;

        protected HubConnection? hubConnection;

        protected readonly ProjectXSettings settings;
        protected bool isDisposed;
        private bool _disposeHubConnection = true;
        private IAuthTokenHandler _authTokenHandler;

        public MarketEventHub(IAuthTokenHandler handler, IOptions<ProjectXSettings> projectXSettings)
        {
            settings = Guard.NotNull(projectXSettings?.Value, nameof(projectXSettings));
            _authTokenHandler = Guard.NotNull(handler, nameof(handler));
            
        }

        /// <summary>
        /// Starts the <see cref="MarketEventHub"/>.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task StartAsync()
        {
            var getTokenTask = _authTokenHandler.GetToken();
            try
            {
                if (getTokenTask.Status != TaskStatus.Running
                    && getTokenTask.Status != TaskStatus.WaitingForActivation
                    && getTokenTask.Status != TaskStatus.RanToCompletion)
                {
                    getTokenTask.RunSynchronously();
                }
            }
            catch (InvalidOperationException)
            {
            }

            hubConnection = new HubConnectionBuilder()
                .WithAutomaticReconnect()
                .WithUrl($"{settings.MarketHubUrl}?access_token={getTokenTask.Result}")
                .Build();

            while (hubConnection!.State == HubConnectionState.Disconnected)
            {
                await hubConnection.StartAsync();
            }
        }

        /// <summary>
        /// Gets the market quote event hub.
        /// </summary>
        public IEventDispatcher<MarketQuoteEvent>? MarketQuoteHub =>
            marketQuoteHub ??= new MarketQuoteDispatcher(hubConnection); //TODO: THis breaks with StartAsync()

        /// <summary>
        /// Gets the market trade event hub.
        /// </summary>
        public IEventDispatcher<MarketTradeEvent>? MarketTradeHub =>
            marketTradeHub ??= new MarketTradeDispatcher(hubConnection);

        /// <summary>
        /// Gets the market depth event hub.
        /// </summary>
        public IEventDispatcher<MarketDepthEvent> MarketDepthHub =>
            marketDepthHub ??= new MarketDepthDispatcher(hubConnection);

        /// <summary>
        /// Subscribes one or more observers to <see cref="MarketDepthHub">.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        public virtual async Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketDepthEvent>[] observers) =>
            await Subscribe("SubscribeContractMarketDepth", "UnsubscribeContractMarketDepth", MarketDepthHub, contractIds, observers);

        /// <summary>
        /// Subscribes one or more observers to <see cref="MarketQuoteEvent">.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        public virtual async Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketQuoteEvent>[] observers) =>
            await Subscribe("SubscribeContractQuotes", "UnsubscribeContractQuotes", MarketQuoteHub!, contractIds, observers);

        /// <summary>
        /// Subscribes one or more observers to <see cref="MarketTradeEvent">.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        public virtual async Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketTradeEvent>[] observers) =>
            await Subscribe("SubscribeContractTrades", "UnsubscribeContractTrades", MarketTradeHub!, contractIds, observers);

        private async Task Subscribe<T>(string subscribeProcedure, string unsubscribeProcedure, IEventDispatcher<T> dispatcher, IEnumerable<string> contractIds, params IObserver<T>[] observers)
            where T : IEvent
        {
            var subscriberTasks = new List<Task>();

            dispatcher.Init();
            subscribers.AddRange(observers.Select(dispatcher.Subscribe));

            foreach (var id in contractIds)
            {
                subscriberTasks.Add(hubConnection.InvokeAsync(subscribeProcedure, id));
            }
            await Task.WhenAll(subscriberTasks);
        }

        /// <summary>
        /// Disposes the resources used by the <see cref="MarketEventHub"/> class.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual async Task Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    marketDepthHub?.Unsubscribe();
                    marketDepthHub?.Dispose();

                    marketTradeHub?.Unsubscribe();
                    marketTradeHub?.Dispose();

                    marketQuoteHub?.Unsubscribe();
                    marketQuoteHub?.Dispose();

                    if (_disposeHubConnection)
                    {
                        await hubConnection.DisposeAsync();
                    }
                }
                isDisposed = true;
            }
        }

        /// <summary>
        /// Disposes the resources used by the <see cref="UserEventHub"/> class.
        /// </summary>
        public async void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            await Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}