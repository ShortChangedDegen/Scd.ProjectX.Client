using Scd.ProjectX.Client.Models.Orders;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// Defines order-related operations in the ProjectX API.
    /// </summary>
    public interface IOrdersFacade
    {
        /// <summary>
        /// Cancels an order by its ID for the specified account.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="InvalidOperationException"></exception>
        Task CancelOrder(CancelOrderRequest request);

        /// <summary>
        /// Cancels an order by its ID for the specified account.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <param name="orderId">The order ID.</param>
        Task CancelOrder(int accountId, int orderId);

        /// <summary>
        /// Creates a new order based on the provided request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The new order ID.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task CreateOrder(CreateOrderRequest request);

        /// <summary>
        /// Retrieves a list of open orders
        /// </summary>
        /// <param name="request">The account ID.</param>
        /// <returns>A collection of orders.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<List<Order>> GetOpenOrders(int accountId);

        /// <summary>
        /// Retrieves a list of orders.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <param name="startTime">The earliest timestamp for the search.</param>
        /// <param name="endTime">the latest timestamp for the search.</param>
        /// <returns>A list of order.</returns>
        Task<List<Order>> GetOrders(int accountId, DateTime startTime, DateTime? endTime);

        /// <summary>
        /// Retrieves a list of orders.
        /// </summary>
        /// <param name="request">A search request.</param>
        /// <returns>A collection of orders.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<List<Order>> GetOrders(SearchRequest request);

        /// <summary>
        /// Updates an existing order by its ID for the specified account.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <param name="orderId">The order ID.</param>
        Task UpdateOrder(int accountId, int orderId);

        /// <summary>
        /// Updates an existing order based on the provided request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="InvalidOperationException"></exception>
        Task UpdateOrder(ModifyRequest request);
    }
}