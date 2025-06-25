using Microsoft.Extensions.Logging;
using Scd.ProjectX.Client.Models.Account;
using Scd.ProjectX.Client.Models.Trades;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// A facade to simplify access to trade-related operations in the ProjectX API.
    /// </summary>
    public class TradesFacade : ITradesFacade
    {
        private readonly ITradesApi _tradesApi;

        /// <summary>
        /// Initializes a new instance of the <see cref="TradesFacade"/> class.
        /// </summary>
        /// <param name="tradesApi">An <see cref="ITradesApi"/> implementation.</param>
        public TradesFacade(ITradesApi tradesApi)
        {
            _tradesApi = Guard.NotNull(tradesApi, nameof(tradesApi));
        }

        /// <summary>
        /// Retrieves a list of trades for a specific account within a given time range.
        /// </summary>
        /// <param name="accountId">An account ID.</param>
        /// <param name="startTime">The earliest timestamp.</param>
        /// <param name="endTime">The latest timestamp.</param>
        /// <returns>The matching trades.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<Trade>> GetTrades(int accountId, DateTime? startTime = null, DateTime? endTime = null)
        {
            var response = await _tradesApi.GetTrades(new SearchRequest
            {
                AccountId = accountId,
                StartTimestamp = startTime ?? DateTime.MinValue,
                EndTimestamp = endTime ?? DateTime.Now
            });

            if (response.Success)
            {
                if (response.Trades is null || response.Trades.Count == 0)
                {
                    return [];
                }
                return response.Trades;
            }
            else
            {
                throw new InvalidOperationException($"Failed to retrieve trades: {response.ErrorMessage}");
            }
        }

        /// <summary>
        /// Retrieves a list of trades based on the provided search request.
        /// </summary>
        /// <param name="searchRequest">The request.</param>
        /// <returns>A list of matching trades.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<Trade>> GetTrades(SearchRequest searchRequest)
        {
            var response = await _tradesApi.GetTrades(searchRequest);

            if (response.Success)
            {
                if (response.Trades is null || response.Trades.Count == 0)
                {
                    return [];
                }
                return response.Trades;
            }
            else
            {
                throw new InvalidOperationException($"Failed to retrieve trades: {response.ErrorMessage}");
            }
        }

    }
}
