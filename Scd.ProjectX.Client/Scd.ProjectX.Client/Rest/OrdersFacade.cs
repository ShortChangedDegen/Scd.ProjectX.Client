using Polly;
using Scd.ProjectX.Client.Models;
using Scd.ProjectX.Client.Models.Orders;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// A facade to simplify access to order-related operations in the ProjectX API.
    /// </summary>
    /// <remarks>This is intended to provide easier access, logging, poly, etc.</remarks>
    public class OrdersFacade : IOrdersFacade
    {
        private readonly IOrdersApi _ordersApi;
        private readonly ResiliencePipeline _pipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersFacade"/> class.
        /// </summary>
        /// <param name="ordersApi">An <see cref="IOrdersApi"/> implementation.</param>
        public OrdersFacade(IOrdersApi ordersApi, ResiliencePipeline pipeline)
        {
            _ordersApi = Guard.NotNull(ordersApi, nameof(ordersApi));
            _pipeline = Guard.NotNull(pipeline, nameof(pipeline));

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
        /// <exception cref="ProjectXClientException"></exception>
        public async Task<List<Order>> GetOrders(SearchRequest request)
        {
            Guard.NotNull(request, nameof(request));
            SearchResponse response;

            if (request.EndTimestamp != null)
            {
                Guard.IsEarlierDate(request.StartTimestamp, request.EndTimestamp.Value, nameof(request.EndTimestamp));
            }

            try
            {
                response = await _pipeline.ExecuteAsync(async context =>
                    await _ordersApi.GetOrders(request)
                );
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException("Error getting orders", ex);
            }

            return response.Success
                    ? response.Orders ?? []
                    : throw new ProjectXClientException($"Error getting orders: {response.ErrorMessage}", response.ErrorCode);
        }

        /// <summary>
        /// Retrieves a list of open orders
        /// </summary>
        /// <param name="request">The account ID.</param>
        /// <returns>A collection of orders.</returns>
        /// <exception cref="ProjectXClientException"></exception>
        public async Task<List<Order>> GetOpenOrders(int accountId)
        {
            SearchResponse response;
            try
            {
                response = await _pipeline.ExecuteAsync(async context =>
                    await _ordersApi.GetOpenOrders(accountId)
                );
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException("Error getting open orders", ex);
            }

            return response.Success
                    ? response.Orders ?? []
                    : throw new ProjectXClientException($"Error getting open orders: {response.ErrorMessage}", response.ErrorCode);
        }

        /// <summary>
        /// Creates a new order based on the provided request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The new order ID.</returns>
        /// <exception cref="ProjectXClientException"></exception>
        public async Task CreateOrder(CreateOrderRequest request)
        {
            Guard.NotNull(request, nameof(request));
            Guard.IsGreaterThan(0, request.Size, nameof(request.Size));
            Guard.NotNullOrEmpty(request.ContractId, nameof(request.ContractId));
            Guard.NotDefault(request.Type, nameof(request.Type));

            CreateResponse response;

            try
            {
                response = await _pipeline.ExecuteAsync(async context =>
                    await _ordersApi.CreateOrder(request)
                );                               
                
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException($"Error creating orders.", ex);
            }

            if (!response.Success)
            {
                throw new ProjectXClientException($"Failed to create order: {response.ErrorMessage}", response.ErrorCode);
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
        /// <exception cref="ProjectXClientException"></exception>
        public async Task CancelOrder(CancelOrderRequest request)
        {
            Guard.NotNull(request, nameof(request));
            Guard.NotDefault(request.AccountId, nameof(request.AccountId));
            Guard.NotDefault(request.OrderId, nameof(request.OrderId));

            DefaultResponse response;

            try
            {
                response = await _pipeline.ExecuteAsync(async context =>
                    await _ordersApi.CancelOrder(request)
                );
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException($"Error cancelling order.", ex);
            }

            if (!response.Success)
            {
                throw new ProjectXClientException($"Failed to cancel order: {response.ErrorMessage}", response.ErrorCode);
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
        /// <exception cref="ProjectXClientException"></exception>
        public async Task UpdateOrder(ModifyRequest request)
        {
            Guard.NotNull(request, nameof(request));
            Guard.NotDefault(request.AccountId, nameof(request.AccountId));
            Guard.NotDefault(request.OrderId, nameof(request.OrderId));

            DefaultResponse response;

            try
            {
                response = await _pipeline.ExecuteAsync(async context =>
                    await _ordersApi.UpdateOrder(request)
                );
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException($"Failed to update order.", ex);
            }

            if (!response.Success)
            {
                throw new ProjectXClientException($"Failed to update order: {response.ErrorMessage}", response.ErrorCode);
            }
        }
    }
}