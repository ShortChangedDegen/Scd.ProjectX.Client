using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Refit;
using Scd.ProjectX.Client.Rest;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;
using System.Text.Json;

namespace Scd.ProjectX.Client
{
    /// <summary>
    /// This service locator or factory will be removed when DI is implemented.
    /// </summary>
    public class ProjectXApi(IOptions<ProjectXSettings> options, IAuthTokenHandler authTokenHandler) : IProjectXApi
    {
        private IAccountFacade? _accountFacade;
        private IMarketDataFacade? _marketDataFacade;
        private IOrdersFacade? _ordersFacade;
        private IPositionsFacade? _positionsFacade;
        private ITradesFacade? _tradesFacade;

        private readonly IAuthTokenHandler _authTokenHandler = Guard.NotNull(authTokenHandler, nameof(authTokenHandler));

        /// <summary>
        /// Gets the account management API for accessing account-related operations.
        /// </summary>
        public virtual IAccountFacade Accounts =>
            _accountFacade ??= new AccountFacade(CreateService<IAccountApi>());

        /// <summary>
        /// Gets the market data API for accessing market data operations.
        /// </summary>
        public virtual IMarketDataFacade MarketData =>
            _marketDataFacade ??= new MarketDataFacade(CreateService<IMarketDataApi>());

        /// <summary>
        /// Gets the orders API for accessing order-related operations.
        /// </summary>
        public virtual IOrdersFacade Orders =>
            _ordersFacade ??= new OrdersFacade(CreateService<IOrdersApi>());

        /// <summary>
        /// Gets the positions API for accessing position-related operations.
        /// </summary>
        public virtual IPositionsFacade Positions =>
            _positionsFacade ??= new PositionsFacade(CreateService<IPositionsApi>());

        /// <summary>
        /// Gets the trades API for accessing trade-related operations.
        /// </summary>
        public virtual ITradesFacade Trades =>
            _tradesFacade ??= new TradesFacade(CreateService<ITradesApi>());

        /// <summary>
        /// Authenticates with the API and cache the token.
        /// </summary>
        /// <returns>A JWT.</returns>
        public virtual async Task<string> Authenticate() => await _authTokenHandler.GetToken();

        /// <summary>
        /// Disposes of the API resources.
        /// </summary>
        public virtual void Dispose()
        {
            _accountFacade = null;
            _marketDataFacade = null;
            _ordersFacade = null;
            _positionsFacade = null;
            _tradesFacade = null;
        }

        /// <summary>
        /// Creates a service instance for the specified type using Refit.
        /// </summary>
        /// <typeparam name="T">The type defining the remote REST API.</typeparam>
        /// <returns>The new API service.</returns>
        private T CreateService<T>() => RestService.For<T>(
            options.Value.ApiUrl,
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                }),
                AuthorizationHeaderValueGetter = async (request, cancellationToken) => await Authenticate()
            });
    }
}