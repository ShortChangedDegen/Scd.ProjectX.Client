using FakeItEasy;
using FluentAssertions;
using Scd.ProjectX.Client.Models.Trades;
using Scd.ProjectX.Client.Rest;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Tests.Rest
{
    public class TradesFacadeTests
    {
        private ITradesApi _tradesApi;
        private ITradesFacade _tradesFacade;

        public TradesFacadeTests()
        {
            _tradesApi = A.Fake<ITradesApi>();
            _tradesFacade = new TradesFacade(_tradesApi);
        }

        [Fact]
        public async Task GetTrades_ShouldThrowArgumentNullException_WhenSearchRequestIsNull()
        {
            SearchRequest? searchRequest = null;
            var actualException = await Assert.ThrowsAsync<ArgumentNullException>(() => _tradesFacade.GetTrades(searchRequest));
            actualException.Message.Should().Contain("searchRequest");
        }

        [Fact]
        public async Task GetTrades_ShouldThrowArgumentException_WhenSearchRequestAccountIdIsDefault()
        {
            SearchRequest? searchRequest = new SearchRequest
            {
                AccountId = default,
                StartTimestamp = DateTime.Now.AddDays(-1),
            };
            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _tradesFacade.GetTrades(searchRequest));
            actualException.Message.Should().Contain("AccountId");
        }

        [Fact]
        public async Task GetTrades_ShouldThrowArgumentException_WhenSearchRequestEndDateIsEarlierThanStartDate()
        {
            SearchRequest? searchRequest = new SearchRequest
            {
                AccountId = 12345,
                StartTimestamp = DateTime.Now,
                EndTimestamp = DateTime.Now.AddDays(-1)
            };
            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _tradesFacade.GetTrades(searchRequest));
            actualException.Message.Should().Contain("EndTimestamp");
        }

        [Fact]
        public async Task GetTrades_ShouldThrowProjectXClientException_WhenRequestIsNotSucessful()
        {
            SearchRequest searchRequest = new SearchRequest
            {
                AccountId = 12345,
                StartTimestamp = DateTime.Now.AddDays(-1),
                EndTimestamp = DateTime.Now
            };

            A.CallTo(() => _tradesApi.GetTrades(searchRequest)).ThrowsAsync(new Exception("Test Error"));
            await Assert.ThrowsAsync<ProjectXClientException>(() => _tradesFacade.GetTrades(searchRequest));
        }

        [Fact]
        public async Task GetTrades_ShouldThrowProjectXClientException_WhenApiThrowsException()
        {
            SearchRequest searchRequest = new SearchRequest
            {
                AccountId = 12345,
                StartTimestamp = DateTime.Now.AddDays(-1),
                EndTimestamp = DateTime.Now
            };

            A.CallTo(() => _tradesApi.GetTrades(searchRequest))
                .Throws(new Exception("Test Exception"));

            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(() => _tradesFacade.GetTrades(searchRequest));
            actualException.InnerException?.Should().NotBeOfType<HttpRequestException>();
        }
    }
}