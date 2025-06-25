using Refit;
using Scd.ProjectX.Client.Models;
using Scd.ProjectX.Client.Models.Positions;

namespace Scd.ProjectX.Client.Rest.Apis
{
    [Headers("Authorization: Bearer")]
    public interface IPositionsApi
    {
        [Post("/api/Position/closeContract")] //Put
        Task<DefaultResponse> CloseContract(CloseRequest request);

        [Post("/api/Position/partialCloseContract")] // Put
        Task<DefaultResponse> PartiallyCloseContract(PartialCloseRequest request);

        [Post("/api/Position/searchOpen")] // Get
        Task<SearchResponse> SearchOpenPositions(int accountId);
    }
}