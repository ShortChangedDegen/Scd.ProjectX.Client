using Scd.ProjectX.Client.Models.MarketData;
using Scd.ProjectX.Client.Models.Orders;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// Defines market-related operations in the ProjectX API.
    /// </summary>
    public interface IMarketDataFacade
    {
        /// <summary>
        /// Retrieves historical bars (candles) for a specific contract within a given time range.
        /// </summary>
        /// <param name="request">The request object.</param>
        Task<List<Candle>> GetBars(BarsRequest request);
        
        /// <summary>
        /// Retrieves historical bars (candles) for a specific contract within a given time range.
        /// </summary>
        /// <param name="contractId">The contract ID.</param>
        /// <param name="startTime">The start time of the historical data.</param>
        /// <param name="endTime">The end time of the historical data.</param>
        /// <param name="unitLength">The number of units to aggregate.</param>
        /// <param name="unit">The unit of aggregation for the historical data.</param>
        /// <param name="limit">The maximum number of bars to retrieve.</param>
        /// <param name="live">Whether to retrieve bars using the sim or live data subscription.</param>
        /// <param name="includePartialBar">Whether to include a partial bar representing the current time unit.</param>
        /// <returns>The matching bars.</returns>
        Task<List<Candle>> GetBars(string contractId, DateTime? startTime, DateTime? endTime, int unitLength = 5, Unit unit = Unit.Minute, int limit = 100, bool live = true, bool includePartialBar = false);
        
        /// <summary>
        /// Retrieves a list of contracts based on the provided search request.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <returns>The matching contracts.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<List<Contract>> GetContracts(ContractSearchRequest request);

        /// <summary>
        /// Retrieves a list of contracts based on the provided contract ID and search query.
        /// </summary>
        /// <param name="contractId">A contract ID</param>
        /// <param name="query">Search text</param>
        /// <param name="live">Whether to retrieve bars using the sim or live data subscription.</param>
        /// <returns>The matching contracts.</returns>
        Task<List<Contract>> GetContracts(string contractId, string query, bool live = true);
    }
}