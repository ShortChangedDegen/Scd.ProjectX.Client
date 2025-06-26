using Microsoft.AspNetCore.SignalR.Client;

using Microsoft.Extensions.Options;
using Scd.ProjectX.Client.Messaging.Dispatchers;
using Scd.ProjectX.Client.Models;
using Scd.ProjectX.Client.Models.Account;
using Scd.ProjectX.Client.Models.Orders;
using Scd.ProjectX.Client.Models.Positions;
using Scd.ProjectX.Client.Models.Trades;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Messaging
{
    /// <summary>
    /// Represents a hub for user-related events such as account, order, position, and trade updates.
    /// </summary>
    public class UserEventHub : IDisposable, IUserEventHub
    {
        protected List<IDisposable> subscribers = new();

        protected IEventDispatcher<UserAccountEvent>? userAccountHub;
        protected IEventDispatcher<UserOrderEvent>? userOrderHub;
        protected IEventDispatcher<UserPositionEvent>? userPositionHub;
        protected IEventDispatcher<UserTradeEvent>? userTradeHub;

        protected IHubConnectionBuilder? hubBuilder;
        protected HubConnection hubConnection;

        protected readonly ProjectXSettings? settings;
        protected bool isDisposed;
        private bool disposeHubConnection = false;
        private AuthTokenHandler _authTokenHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEventHub"/> class.
        /// </summary>
        /// <param name="api">The ProjectX API.</param>
        /// <param name="settings">The ProjectX settings.</param>
        public UserEventHub(AuthTokenHandler authTokenHandler, IOptions<ProjectXSettings> projectXSettings)
        {
            settings = Guard.NotNull(projectXSettings.Value, nameof(projectXSettings));
            _authTokenHandler = Guard.NotNull(authTokenHandler, nameof(authTokenHandler));

            hubBuilder = new HubConnectionBuilder()
                .WithAutomaticReconnect();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEventHub"/> class.
        /// </summary>
        /// <param name="connection">The shared <see cref="HubConnection">.</param>
        public UserEventHub(AuthTokenHandler authTokenHandler, HubConnection connection)
        {
            _authTokenHandler = Guard.NotNull(authTokenHandler, nameof(authTokenHandler));
            hubConnection = Guard.NotNull(connection, nameof(connection));
        }

        /// <summary>
        /// Starts the <see cref="UserEventHub"/>.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task StartAsync()
        {
            if (hubConnection == null && hubBuilder != null)
            {
                hubConnection = hubBuilder
                    .WithUrl($"{settings!.UserHubUrl}?access_token={await _authTokenHandler.GetToken()}").Build();
                disposeHubConnection = true;
            }

            while (hubConnection?.State == HubConnectionState.Disconnected)
            {
                await hubConnection.StartAsync();
            }
        }

        /// <summary>
        /// Gets an <see cref="EventDispatcher{TEvent}"/> for <see cref="UserAccountEvent"/>s.
        /// </summary>
        public virtual IEventDispatcher<UserAccountEvent> UserAccountHub =>
            userAccountHub ??= new UserAccountDispatcher(hubConnection);

        /// <summary>
        /// Gets an <see cref="EventDispatcher{TEvent}"/> for <see cref="UserOrderEvent"/>s.
        /// </summary>
        public virtual IEventDispatcher<UserOrderEvent> UserOrderHub =>
            userOrderHub ??= new UserOrderEventDispatcher(hubConnection);

        /// <summary>
        /// Gets an <see cref="EventDispatcher{TEvent}"/> for <see cref="UserPositionEvent"/>s.
        /// </summary>
        public virtual IEventDispatcher<UserPositionEvent> UserPositionHub =>
            userPositionHub ??= new UserPositionEventDispatcher(hubConnection);

        /// <summary>
        /// Gets an <see cref="EventDispatcher{TEvent}"/> for <see cref="UserTradeEvent"/>s.
        /// </summary>
        public virtual IEventDispatcher<UserTradeEvent> UserTradeHub =>
            userTradeHub ??= new UserTradeEventDispatcher(hubConnection);

        /// <summary>
        /// Subscribes one or more observers to <see cref="UserAccountEvent">s.
        /// </summary>
        /// <param name="accountIds">One or more account IDs to listen for.</param>
        /// <param name="observers">One or more observers.</param>
        public virtual async Task Subscribe(params IObserver<UserAccountEvent>[] observers)
        {
            subscribers.AddRange(observers.Select(UserAccountHub.Subscribe));
            await hubConnection.InvokeAsync("SubscribeAccounts");
        }

        /// <summary>
        /// Subscribes one or more observers to <see cref="UserOrderEvent">.
        /// </summary>
        /// <param name="accountIds">One or more account IDs to listen for.</param>
        /// <param name="observers">One or more observers.</param>
        public virtual async Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserOrderEvent>[] observers) =>
            await Subscribe("SubscribeOrders", UserOrderHub, accountIds, observers);

        /// <summary>
        /// Subscribes one or more observers to <see cref="UserPositionEvent">.
        /// </summary>
        /// <param name="accountIds">One or more account IDs to listen for.</param>
        /// <param name="observers">One or more observers.</param>
        public virtual async Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserPositionEvent>[] observers) =>
            await Subscribe("SubscribePositions", UserPositionHub, accountIds, observers);

        /// <summary>
        /// Subscribes one or more observers to <see cref="UserTradeEvent">.
        /// </summary>
        /// <param name="accountIds">One or more account IDs to listen for.</param>
        /// <param name="observers">One or more observers.</param>
        public virtual async Task Subscribe(IEnumerable<int> accountIds, params IObserver<UserTradeEvent>[] observers) =>
            await Subscribe("SubscribeTrades", UserTradeHub, accountIds, observers);

        private Task Subscribe<T>(string remoteProcedure, IEventDispatcher<T> dispatcher, IEnumerable<int> accountIds, params IObserver<T>[] observers)
            where T : IEvent
        {
            var subscriberTasks = new List<Task>();
            subscribers.AddRange(observers.Select(dispatcher.Subscribe));

            foreach (var id in accountIds)
            {
                subscriberTasks.Add(hubConnection.InvokeAsync(remoteProcedure, id));
            }
            return Task.WhenAll(subscriberTasks);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    if (disposeHubConnection)
                    {
                        hubConnection?.DisposeAsync();
                    }

                    userAccountHub?.Dispose();
                    userOrderHub?.Dispose();
                    userPositionHub?.Dispose();
                    userTradeHub?.Dispose();
                }
                isDisposed = true;
            }
        }

        /// <summary>
        /// Disposes the resources used by the <see cref="UserEventHub"/> class.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}