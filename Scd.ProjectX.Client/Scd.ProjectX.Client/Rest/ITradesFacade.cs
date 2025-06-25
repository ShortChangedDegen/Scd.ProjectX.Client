using Scd.ProjectX.Client.Models.Trades;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// Defines trade-related operations in the ProjectX API.
    /// </summary>
    public interface ITradesFacade
    {
        /// <summary>
        /// Retrieves a list of trades for a specific account within a given time range.
        /// </summary>
        /// <param name="accountId">An account ID.</param>
        /// <param name="startTime">The earliest timestamp.</param>
        /// <param name="endTime">The latest timestamp.</param>
        /// <returns>The matching trades.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<List<Trade>> GetTrades(int accountId, DateTime startTime, DateTime? endTime = null);

        /// <summary>
        /// Retrieves a list of trades based on the provided search request.
        /// </summary>
        /// <param name="searchRequest">The request.</param>
        /// <returns>A list of matching trades.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<List<Trade>> GetTrades(SearchRequest searchRequest);
    }
}