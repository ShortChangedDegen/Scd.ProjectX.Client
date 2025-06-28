using Scd.ProjectX.Client.Models.Orders;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// A facade to simplify access to order-related operations in the ProjectX API.
    /// </summary>
    /// <remarks>This is intended to provide easier access, logging, etc.</remarks>
    public class OrderFacade : IOrderFacade
    {
        private readonly IOrdersApi _ordersApi;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersFacade"/> class.
        /// </summary>
        /// <param name="ordersApi">An <see cref="IOrdersApi"/> implementation.</param>
        public OrderFacade(IOrdersApi ordersApi)
        {
            _ordersApi = Guard.NotNull(ordersApi, nameof(ordersApi));
        }

        /// <summary>
        /// Retrieves a list of orders.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <param name="startTime">The earliest timestamp for the search.</param>
        /// <param name="endTime">the latest timestamp for the search.</param>
        /// <returns>A list of order.</returns>
        public async Task<List<Order>> GetOrders(int accountId, DateTime startTime, DateTime? endTime) =>
            await GetOrders(new SearchRequest
            {
                AccountId = accountId,
                StartTimestamp = startTime,
                EndTimestamp = endTime
            });

        /// <summary>
        /// Retrieves a list of orders.
        /// </summary>
        /// <param name="request">A search request.</param>
        /// <returns>A collection of orders.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<Order>> GetOrders(SearchRequest request)
        {
            var response = await _ordersApi.GetOrders(request);
            if (response.Success)
            {
                return response.Orders ?? [];
            }
            else
            {
                throw new InvalidOperationException($"Failed to retrieve orders: {response.ErrorMessage}");
            }
        }

        /// <summary>
        /// Retrieves a list of open orders
        /// </summary>
        /// <param name="request">The account ID.</param>
        /// <returns>A collection of orders.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<Order>> GetOpenOrders(int accountId)
        {
            var response = await _ordersApi.GetOpenOrders(accountId);

            if (response.Success)
            {
                return response.Orders ?? [];
            }
            else
            {
                throw new InvalidOperationException($"Failed to retrieve orders: {response.ErrorMessage}");
            }
        }

        /// <summary>
        /// Creates a new order based on the provided request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The new order ID.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<int?> CreateOrder(CreateOrderRequest request)
        {
            var response = await _ordersApi.CreateOrder(request);
            if (response.Success)
            {
                return response.OrderId;
            }
            else
            {
                throw new InvalidOperationException($"Failed to create order: {response.ErrorMessage}");
            }
        }

        /// <summary>
        /// Cancels an order by its ID for the specified account.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <param name="orderId">The order ID.</param>

        public async Task CancelOrder(int accountId, int orderId) =>
            await CancelOrder(new CancelOrderRequest
            {
                AccountId = accountId,
                OrderId = orderId
            });

        /// <summary>
        /// Cancels an order by its ID for the specified account.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task CancelOrder(CancelOrderRequest request)
        {
            var response = await _ordersApi.CancelOrder(request);
            if (!response.Success)
            {
                throw new InvalidOperationException($"Failed to cancel order: {response.ErrorMessage}");
            }
        }

        /// <summary>
        /// Updates an existing order by its ID for the specified account.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <param name="orderId">The order ID.</param>
        public async Task UpdateOrder(int accountId, int orderId) =>
            await UpdateOrder(new ModifyRequest
            {
                AccountId = accountId,
                OrderId = orderId
            });

        /// <summary>
        /// Updates an existing order based on the provided request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task UpdateOrder(ModifyRequest request)
        {
            var response = await _ordersApi.UpdateOrder(request);
            if (!response.Success)
            {
                throw new InvalidOperationException($"Failed to update order: {response.ErrorMessage}");
            }
        }
    }
}