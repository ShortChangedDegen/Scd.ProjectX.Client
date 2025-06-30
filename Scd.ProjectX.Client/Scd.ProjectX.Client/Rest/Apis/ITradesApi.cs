using Refit;
using Scd.ProjectX.Client.Models.Trades;

namespace Scd.ProjectX.Client.Rest.Apis
{
    [Headers(
        "Accept: application/json",
        "Content-Type: application/json",
        "Authorization: Bearer")]
    public interface ITradesApi
    {
        [Post("api/Trade/search")]
        Task<SearchResponse> GetTrades(SearchRequest request);
    }
}