using FakeItEasy;
using FluentAssertions;
using Scd.ProjectX.Client.Models;
using Scd.ProjectX.Client.Models.Orders;
using Scd.ProjectX.Client.Rest;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Tests.Rest
{
    public class OrdersFacadeTests
    {
        private IOrdersApi _ordersApi;
        private OrdersFacade _ordersFacade;

        public OrdersFacadeTests()
        {
            _ordersApi = A.Fake<IOrdersApi>();
            _ordersFacade = new OrdersFacade(_ordersApi);
        }

        [Fact]
        public void OrdersFacade_ShouldThrowArgumentNullException_WhenOrdersApiIsNull()
        {
            var thrownException = Assert.Throws<ArgumentNullException>(() => new OrdersFacade(null));
            thrownException.Message.Should().Contain("ordersApi");
        }

        [Fact]
        public async Task GetOrders_ShouldThrowArgumentNullException_WhenProvidedRequestIsNull()
        {
            var ordersFacade = new OrdersFacade(_ordersApi);
            var actualException = await Assert.ThrowsAsync<ArgumentNullException>(() => _ordersFacade.GetOrders(null));
            actualException.Message.Should().Contain("request");
        }

        [Fact]
        public async Task GetOrders_ShouldThrowArgumentException_WhenProvidedEndDateIsEarlierThanStartDate()
        {
            var accountId = 12345;
            var startTime = DateTime.UtcNow.AddDays(-1);
            var endTime = DateTime.UtcNow.AddDays(-2);

            var ordersFacade = new OrdersFacade(_ordersApi);
            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _ordersFacade.GetOrders(accountId, startTime, endTime));
            actualException.Message.Should().Contain("EndTimestamp");
        }

        [Fact]
        public async Task GetOrders_ShouldThrowProjectXClientException_WhenApiThrowsException()
        {
            var accountId = 12345;
            var startTime = DateTime.UtcNow.AddDays(-1);
            var endTime = DateTime.UtcNow;

            A.CallTo(() => _ordersApi.GetOrders(A<SearchRequest>._))
                .Throws(new Exception("TestException"));

            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(() => _ordersFacade.GetOrders(accountId, startTime, endTime));
            actualException.Message.Should().Contain("orders");
        }

        [Fact]
        public async Task GetOrders_ShouldReturnOrdersCollection_WhenRequestIsSuccessful()
        {
            var accountId = 12345;
            var startTime = DateTime.UtcNow.AddDays(-1);
            var endTime = DateTime.UtcNow;

            var providedOrders = new List<Order>
            {
                new Order
                {
                    Id = 1234,
                    AccountId = accountId,
                    CreationTimestamp = DateTime.UtcNow.AddHours(-12),
                    ContractId = "TestContract1",
                    Type = OrderType.Market,
                    Side = Side.Bid,
                    Status = 1,
                    LimitPrice = 100.50,
                    Size = 10,
                    StopPrice = 99.00,
                    UpdateTimestamp = DateTime.UtcNow
                },
                new Order
                {
                    Id = 1244,
                    AccountId = accountId,
                    CreationTimestamp = DateTime.UtcNow.AddHours(-10),
                    ContractId = "TestContract2",
                    Type = OrderType.Market,
                    Side = Side.Bid,
                    Status = 1,
                    LimitPrice = 100.50,
                    Size = 10,
                    StopPrice = 99.00,
                    UpdateTimestamp = DateTime.UtcNow
                }
            };

            A.CallTo(() => _ordersApi.GetOrders(A<SearchRequest>._))
                .Returns(new SearchResponse { Orders = providedOrders, Success = true });

            var results = await _ordersFacade.GetOrders(accountId, startTime, endTime);
            results.Should().BeEquivalentTo(providedOrders);
        }

        [Fact]
        public async Task GetOrders_ShouldReturnEmptyOrdersCollection_WhenRequestIsNotSuccessful()
        {
            var accountId = 12345;
            var startTime = DateTime.UtcNow.AddDays(-1);
            var endTime = DateTime.UtcNow;

            var providedOrders = new List<Order>
            {
                new Order
                {
                    Id = 1234,
                    AccountId = accountId,
                    CreationTimestamp = DateTime.UtcNow.AddHours(-12),
                    ContractId = "TestContract1",
                    Type = OrderType.Market,
                    Side = Side.Bid,
                    Status = 1,
                    LimitPrice = 100.50,
                    Size = 10,
                    StopPrice = 99.00,
                    UpdateTimestamp = DateTime.UtcNow
                },
                new Order
                {
                    Id = 1244,
                    AccountId = accountId,
                    CreationTimestamp = DateTime.UtcNow.AddHours(-10),
                    ContractId = "TestContract2",
                    Type = OrderType.Market,
                    Side = Side.Bid,
                    Status = 1,
                    LimitPrice = 100.50,
                    Size = 10,
                    StopPrice = 99.00,
                    UpdateTimestamp = DateTime.UtcNow
                }
            };

            A.CallTo(() => _ordersApi.GetOrders(A<SearchRequest>._))
                .Returns(new SearchResponse { Orders = providedOrders, Success = false });

            var results = await _ordersFacade.GetOrders(accountId, startTime, endTime);
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetOpenOrders_ShouldThrowProjectXClientException_WhenApiThrowsException()
        {
            var accountId = 12345;
            A.CallTo(() => _ordersApi.GetOpenOrders(accountId))
                .Throws(new Exception("TestException"));

            var ordersFacade = new OrdersFacade(_ordersApi);

            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(() => _ordersFacade.GetOpenOrders(accountId));
            actualException.Message.Should().Contain("open orders");
        }

        [Fact]
        public async Task GetOpenOrders_ShouldReturnOrdersCollection_WhenRequestIsSuccessful()
        {
            var accountId = 12345;

            var providedOrders = new List<Order>
            {
                new Order
                {
                    Id = 1234,
                    AccountId = accountId,
                    CreationTimestamp = DateTime.UtcNow.AddHours(-12),
                    ContractId = "TestContract1",
                    Type = OrderType.Market,
                    Side = Side.Bid,
                    Status = 1,
                    LimitPrice = 100.50,
                    Size = 10,
                    StopPrice = 99.00,
                    UpdateTimestamp = DateTime.UtcNow
                },
                new Order
                {
                    Id = 1244,
                    AccountId = accountId,
                    CreationTimestamp = DateTime.UtcNow.AddHours(-10),
                    ContractId = "TestContract2",
                    Type = OrderType.Market,
                    Side = Side.Bid,
                    Status = 1,
                    LimitPrice = 100.50,
                    Size = 10,
                    StopPrice = 99.00,
                    UpdateTimestamp = DateTime.UtcNow
                }
            };

            A.CallTo(() => _ordersApi.GetOpenOrders(accountId))
                .Returns(new SearchResponse { Orders = providedOrders, Success = true });

            var results = await _ordersFacade.GetOpenOrders(accountId);
            results.Should().BeEquivalentTo(providedOrders);
        }

        [Fact]
        public async Task GetOpenOrders_ShouldReturnEmptyOrdersCollection_WhenRequestIsNotSuccessful()
        {
            var accountId = 12345;
            var startTime = DateTime.UtcNow.AddDays(-1);
            var endTime = DateTime.UtcNow;

            var providedOrders = new List<Order>
            {
                new Order
                {
                    Id = 1234,
                    AccountId = accountId,
                    CreationTimestamp = DateTime.UtcNow.AddHours(-12),
                    ContractId = "TestContract1",
                    Type = OrderType.Market,
                    Side = Side.Bid,
                    Status = 1,
                    LimitPrice = 100.50,
                    Size = 10,
                    StopPrice = 99.00,
                    UpdateTimestamp = DateTime.UtcNow
                },
                new Order
                {
                    Id = 1244,
                    AccountId = accountId,
                    CreationTimestamp = DateTime.UtcNow.AddHours(-10),
                    ContractId = "TestContract2",
                    Type = OrderType.Market,
                    Side = Side.Bid,
                    Status = 1,
                    LimitPrice = 100.50,
                    Size = 10,
                    StopPrice = 99.00,
                    UpdateTimestamp = DateTime.UtcNow
                }
            };

            A.CallTo(() => _ordersApi.GetOpenOrders(accountId))
                .Returns(new SearchResponse { Orders = providedOrders, Success = false });

            var ordersFacade = new OrdersFacade(_ordersApi);
            var results = await _ordersFacade.GetOpenOrders(accountId);
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateOrder_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            CreateOrderRequest providedRequest = null;

            var ordersFacade = new OrdersFacade(_ordersApi);
            var actualException = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _ordersFacade.CreateOrder(providedRequest));
            actualException.Message.Should().Contain("request");
        }

        [Fact]
        public async Task CreateOrder_ShouldThrowArgumentException_WhenSizeIsLessThanOne()
        {
            var providedRequest = new CreateOrderRequest
            {
                AccountId = 12345,
                ContractId = "TestContract",
                Type = OrderType.Market,
                Side = Side.Bid,
                Size = 0,
                LimitPrice = 100.50m,
                StopPrice = 99.00m
            };

            var ordersFacade = new OrdersFacade(_ordersApi);
            var actualException = await Assert.ThrowsAsync<ArgumentException>(() =>
            ordersFacade.CreateOrder(providedRequest));
            actualException.Message.Should().Contain("Size");
        }

        [Fact]
        public async Task CreateOrder_ShouldThrowArgumentException_WhenContractIdIsEmptyOrNull()
        {
            var providedRequest = new CreateOrderRequest
            {
                AccountId = 12345,
                ContractId = "",
                Type = OrderType.Market,
                Side = Side.Bid,
                Size = 1,
                LimitPrice = 100.50m,
                StopPrice = 99.00m
            };

            var ordersFacade = new OrdersFacade(_ordersApi);
            var actualException = await Assert.ThrowsAsync<ArgumentException>(() =>
            _ordersFacade.CreateOrder(providedRequest));
            actualException.Message.Should().Contain("ContractId");
        }

        [Fact]
        public async Task CreateOrder_ShouldThrowArgumentException_WhenTypeIsUnknown()
        {
            var providedRequest = new CreateOrderRequest
            {
                AccountId = 12345,
                ContractId = "test123",
                Type = default,
                Side = Side.Bid,
                Size = 1,
                LimitPrice = 100.50m,
                StopPrice = 99.00m
            };

            var ordersFacade = new OrdersFacade(_ordersApi);
            var actualException = await Assert.ThrowsAsync<ArgumentException>(() =>
            _ordersFacade.CreateOrder(providedRequest));
            actualException.Message.Should().Contain("Type");
        }

        [Fact]
        public async Task CreateOrder_ShouldThrowProjectXClientException_WhenOrdersApiThrowsException()
        {
            var providedRequest = new CreateOrderRequest
            {
                AccountId = 12345,
                ContractId = "test123",
                Type = OrderType.Market,
                Side = Side.Bid,
                Size = 1,
                LimitPrice = 100.50m,
                StopPrice = 99.00m
            };

            A.CallTo(() => _ordersApi.CreateOrder(A<CreateOrderRequest>._))
                .Throws(new Exception("TestException"));

            var ordersFacade = new OrdersFacade(_ordersApi);
            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(
                () => _ordersFacade.CreateOrder(providedRequest));
            actualException.Message.Should().Contain("Error creating orders");
        }

        [Fact]
        public async Task CancelOrdeShouldr_ShouldThrowArgumentNullException_WhenProvidedRequestIsNull()
        {
            CancelOrderRequest providedRequest = null;

            var actualException = await Assert.ThrowsAsync<ArgumentNullException>(() => _ordersFacade.CancelOrder(providedRequest));

            actualException.Message.Should().Contain("request");
        }

        [Fact]
        public async Task CancelOrder_ShouldThrowArgumentException_WhenProvidedRequestAccountIdIsDefault()
        {
            var providedRequest = new CancelOrderRequest
            {
                AccountId = 0,
                OrderId = 12345
            };

            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _ordersFacade.CancelOrder(providedRequest));

            actualException.Message.Should().Contain("AccountId");
        }

        [Fact]
        public async Task CancelOrder_ShouldThrowArgumentException_WhenProvidedRequestOrderIdIsDefault()
        {
            var providedRequest = new CancelOrderRequest
            {
                AccountId = 12345,
                OrderId = 0
            };

            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _ordersFacade.CancelOrder(providedRequest));
            actualException.Message.Should().Contain("OrderId");
        }

        [Fact]
        public async Task UpdateOrder_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            ModifyRequest providedRequest = null;

            var actualException = await Assert.ThrowsAsync<ArgumentNullException>(() => _ordersFacade.UpdateOrder(providedRequest));
            actualException.Message.Should().Contain("request");
        }

        [Fact]
        public async Task UpdateOrder_ShouldArgumentException_WhenAccountIdIsDefault()
        {
            var providedRequest = new ModifyRequest
            {
                AccountId = 0,
                OrderId = 12345,
                Size = 10,
                LimitPrice = 100.50m,
                StopPrice = 99.00m
            };

            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _ordersFacade.UpdateOrder(providedRequest));
            actualException.Message.Should().Contain("AccountId");
        }

        [Fact]
        public async Task UpdateOrder_ShouldThrowArgumentException_WhenOrderIdIsDefault()
        {
            var providedRequest = new ModifyRequest
            {
                AccountId = 12345,
                OrderId = 0,
                Size = 10,
                LimitPrice = 100.50m,
                StopPrice = 99.00m
            };

            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _ordersFacade.UpdateOrder(providedRequest));
            actualException.Message.Should().Contain("OrderId");
        }

        [Fact]
        public async Task UpdateOrder_ShouldThrowProjectXClientException_WhenOrderApiThrowsException()
        {
            var providedRequest = new ModifyRequest
            {
                AccountId = 12345,
                OrderId = 54321,
                Size = 10,
                LimitPrice = 100.50m,
                StopPrice = 99.00m
            };

            A.CallTo(() => _ordersApi.UpdateOrder(A<ModifyRequest>._))
                .Throws(new Exception("TestException"));

            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(() => _ordersFacade.UpdateOrder(providedRequest));
            actualException.Message.Should().Contain("update order");
        }

        [Fact]
        public async Task UpdateOrder_ShouldThrowExpectedProjectXClientException_WhenApiRequestNotSuccessful()
        {
            var providedRequest = new ModifyRequest
            {
                AccountId = 12345,
                OrderId = 54321,
                Size = 10,
                LimitPrice = 100.50m,
                StopPrice = 99.00m
            };

            var response = new DefaultResponse
            {
                Success = false,
                ErrorMessage = "Failed to update order"
            };

            A.CallTo(() => _ordersApi.UpdateOrder(A<ModifyRequest>._))
                .Returns(response);

            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(() => _ordersFacade.UpdateOrder(providedRequest));
            actualException.Message.Should().Contain("update order");
        }
    }
}