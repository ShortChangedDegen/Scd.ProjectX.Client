using Microsoft.Extensions.Logging;
using Scd.ProjectX.Client.Models.MarketData;
using Scd.ProjectX.Client.Models.Orders;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// A facade to simplify access to market-related operations in the ProjectX API.
    /// </summary>
    /// <remarks>This is intended to provide easier access, logging, etc.</remarks>
    public class MarketDataFacade : IMarketDataFacade
    {
        private readonly IMarketDataApi _marketDataApi;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketDataFacade"/> class.
        /// </summary>
        /// <param name="marketDataApi">An implementation of <see cref="IMarketDataApi"/></param>
        public MarketDataFacade(IMarketDataApi marketDataApi)
        {
            _marketDataApi = Guard.NotNull(marketDataApi, nameof(marketDataApi));
        }

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
        public async Task<List<Candle>> GetBars(
            string contractId,
            DateTime? startTime,
            DateTime? endTime,
            int unitLength = 5,
            Unit unit = Unit.Minute,
            int limit = 100,
            bool live = true,
            bool includePartialBar = false) =>
            await GetBars(new BarsRequest
            {
                ContractId = contractId,
                StartTime = startTime,
                EndTime = endTime,
                UnitNumber = unitLength,
                Unit = unit,
                Limit = limit,
                IncludePartialBar = includePartialBar,
                Live = live // Assuming live data is the default
            });

        /// <summary>
        /// Retrieves historical bars (candles) for a specific contract within a given time range.
        /// </summary>
        /// <param name="request">The request object.</param>
        public async Task<List<Candle>> GetBars(BarsRequest request)
        {
            var response = await _marketDataApi.GetBars(request);
            if (response.Success)
            {
                return response.Bars;
            }
            else
            {
                throw new InvalidOperationException(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Retrieves a list of contracts based on the provided contract ID and search query.
        /// </summary>
        /// <param name="contractId">A contract ID</param>
        /// <param name="query">Search text</param>
        /// <param name="live">Whether to retrieve bars using the sim or live data subscription.</param>
        /// <returns>The matching contracts.</returns>
        public async Task<List<Contract>> GetContracts(string contractId, string query, bool live = true) =>
            await GetContracts(new ContractSearchRequest
            {
                ContractId = contractId,
                SearchText = query,
                Live = live
            });


        /// <summary>
        /// Retrieves a list of contracts based on the provided search request.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <returns>The matching contracts.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<Contract>> GetContracts(ContractSearchRequest request)
        {
            var response = await _marketDataApi.GetContracts(request);
            if (response.Success)
            {
                return response.Contracts;
            }
            else
            {
                throw new InvalidOperationException(response.ErrorMessage);
            }
        }
    }
}
