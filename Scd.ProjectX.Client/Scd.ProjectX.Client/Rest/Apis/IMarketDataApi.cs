using Refit;
using Scd.ProjectX.Client.Models.MarketData;

namespace Scd.ProjectX.Client.Rest.Apis
{
    [Headers("Authorization: Bearer")]
    public interface IMarketDataApi
    {
        [Post("/api/History/retrieveBars")] // Get
        Task<CandleResponse> GetBars(BarsRequest request);

        [Post("/api/Contract/search")] // Get/Post
        Task<ContractSearchResponse> GetContracts(ContractSearchRequest request);

        [Post("/api/Contract/searchById")] // Get
        Task<ContractSearchResponse> GetContractsById(string contractId);
    }
}