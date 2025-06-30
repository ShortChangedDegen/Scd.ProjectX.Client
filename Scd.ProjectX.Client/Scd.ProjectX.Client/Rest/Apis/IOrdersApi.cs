using Refit;
using Scd.ProjectX.Client.Models;
using Scd.ProjectX.Client.Models.Orders;

namespace Scd.ProjectX.Client.Rest.Apis
{
    [Headers("Authorization: Bearer")]
    public interface IOrdersApi
    {
        [Post("/api/Order/search")]
        Task<SearchResponse> GetOrders(SearchRequest request);

        [Post("/api/Order/searchOpen")]
        Task<SearchResponse> GetOpenOrders(int accountId);

        [Post("/api/Order/place")]
        Task<CreateResponse> CreateOrder(CreateOrderRequest request);

        [Post("/api/Order/cancel")]
        Task<DefaultResponse> CancelOrder(CancelOrderRequest request);

        [Post("/api/Order/modify")]
        Task<DefaultResponse> UpdateOrder(ModifyRequest request);
    }
}