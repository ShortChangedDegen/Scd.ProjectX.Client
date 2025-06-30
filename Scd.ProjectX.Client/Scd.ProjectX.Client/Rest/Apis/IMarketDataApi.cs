using Refit;
using Scd.ProjectX.Client.Models.MarketData;

namespace Scd.ProjectX.Client.Rest.Apis
{
    [Headers("Authorization: Bearer")]
    public interface IMarketDataApi
    {
        [Post("/api/History/retrieveBars")]
        Task<CandleResponse> GetBars(BarsRequest request);

        [Post("/api/Contract/search")]
        Task<ContractSearchResponse> GetContracts(ContractSearchRequest request);

        [Post("/api/Contract/searchById")]
        Task<ContractSearchResponse> GetContractsById(string contractId);
    }
}