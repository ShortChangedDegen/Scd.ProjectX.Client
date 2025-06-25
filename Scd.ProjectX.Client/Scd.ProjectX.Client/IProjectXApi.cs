using Scd.ProjectX.Client.Rest;

namespace Scd.ProjectX.Client
{
    /// <summary>
    /// Defines the interface for the TopstepX API, providing access to
    /// various services.
    /// <remarks>
    /// It is suggested to use a dependency injection container to locate API members
    /// for more complicated applications. (I probably will)
    /// </remarks>
    public interface IProjectXApi : IDisposable
    {
        /// <summary>
        /// Gets the account management API for accessing account-related operations.
        /// </summary>
        IAccountFacade Accounts { get; }

        /// <summary>
        /// Gets the market data API for accessing market data operations.
        /// </summary>
        IMarketDataFacade MarketData { get; }

        /// <summary>
        /// Gets the orders API for accessing order-related operations.
        /// </summary>
        IOrderFacade Orders { get; }

        /// <summary>
        /// Gets the positions API for accessing position-related operations.
        /// </summary>
        IPositionsFacade Positions { get; }

        /// <summary>
        /// Gets the trades API for accessing trade-related operations.
        /// </summary>
        ITradesFacade Trades { get; }

        /// <summary>
        /// Authenticates with the API.
        /// </summary>
        /// <returns>The authentication JWT.</returns>
        Task<string> Authenticate();
    }
}