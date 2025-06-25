using Refit;
using Scd.ProjectX.Client.Models;
using Scd.ProjectX.Client.Models.Orders;

namespace Scd.ProjectX.Client.Rest.Apis
{
    [Headers("Authorization: Bearer")]
    public interface IOrdersApi
    {
        [Post("/api/Order/search")] // Get/Post depending on query complexity
        Task<SearchResponse> GetOrders(SearchRequest request);

        [Post("/api/Order/searchOpen")] // Get
        Task<SearchResponse> GetOpenOrders(int accountId);

        [Post("/api/Order/place")] // Post
        Task<CreateResponse> CreateOrder(CreateOrderRequest request);

        [Post("/api/Order/cancel")] //Delete
        Task<DefaultResponse> CancelOrder(CancelOrderRequest request);

        [Post("/api/Order/modify")] // Put
        Task<DefaultResponse> UpdateOrder(ModifyRequest request);
    }
}