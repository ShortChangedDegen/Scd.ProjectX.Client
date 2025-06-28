using FakeItEasy;
using FluentAssertions;
using Scd.ProjectX.Client.Models.MarketData;
using Scd.ProjectX.Client.Models.Orders;
using Scd.ProjectX.Client.Rest;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Tests.Rest
{
    public class MarketDataFacadeTests
    {
        private IMarketDataApi _marketDataApi;

        public MarketDataFacadeTests()
        {
            _marketDataApi = A.Fake<IMarketDataApi>();
        }

        [Fact]
        public void MarketDataFacade_ShouldThrowArgumentNullException_WhenMarketDataApiIsNull()
        {
            IMarketDataApi? nullMarketDataApi = null;
            Assert.Throws<ArgumentNullException>(() => new MarketDataFacade(nullMarketDataApi!));
        }

        [Fact]
        public async Task GetBars_ShouldReturnBars_WhenValidRequestIsMade()
        {
            // Arrange
            var facade = new MarketDataFacade(_marketDataApi);
            var request = new BarsRequest
            {
                ContractId = "test-contract",
                StartTime = DateTime.UtcNow.AddDays(-1),
                EndTime = DateTime.UtcNow,
                UnitNumber = 5,
                Unit = Unit.Minute,
                Limit = 100,
                IncludePartialBar = false,
                Live = true
            };
            var expectedBars = new List<Candle>
            {
                new Candle { Open = 100, Close = 105, High = 110, Low = 95, Volume = 1000 },
                new Candle { Open = 105, Close = 110, High = 115, Low = 100, Volume = 1200 }
            };
            A.CallTo(() => _marketDataApi.GetBars(A<BarsRequest>._))
                .Returns(new CandleResponse { Bars = expectedBars, Success = true });

            // Act
            var result = await facade.GetBars(request);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBars.Count, result.Count);
            Assert.Equal(expectedBars.First().Open, result.First().Open);
        }

        [Fact]
        public async Task GetBars_ShouldReturnNoBars_WhenInvalidRequestIsMade()
        {
            // Arrange
            var facade = new MarketDataFacade(_marketDataApi);
            var request = new BarsRequest
            {
                ContractId = "test-contract",
                StartTime = DateTime.UtcNow.AddDays(-1),
                EndTime = DateTime.UtcNow,
                UnitNumber = 5,
                Unit = Unit.Minute,
                Limit = 100,
                IncludePartialBar = false,
                Live = true
            };

            A.CallTo(() => _marketDataApi.GetBars(A<BarsRequest>._))
                .Returns(new CandleResponse { Bars = new List<Candle>(), Success = false });

            var result = await facade.GetBars(request);
            result.Count.Should().Be(0, "because the API returned no bars for the given request");
        }

        [Fact]
        public async Task GetBars_ThrowExpectedException_WhenCallToApiThrows()
        {
            var expectedMessage = "Error getting candle data";

            // Arrange
            var facade = new MarketDataFacade(_marketDataApi);
            var request = new BarsRequest
            {
                ContractId = "test-contract",
                StartTime = DateTime.UtcNow.AddDays(-1),
                EndTime = DateTime.UtcNow,
                UnitNumber = 5,
                Unit = Unit.Minute,
                Limit = 100,
                IncludePartialBar = false,
                Live = true
            };

            A.CallTo(() => _marketDataApi.GetBars(A<BarsRequest>._))
                .Throws(new Exception(expectedMessage));

            // Act
            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(async () => await facade.GetBars(request));
            actualException.Message
                .Should()
                .Be(expectedMessage);

            actualException.InnerException
                .Should()
                .NotBeNull()
                .And
                .BeOfType<Exception>();
        }

        [Fact]
        public async Task GetBars_ShouldThrowArgumentNullException_WhenContractIdIsNull()
        {
            var facade = new MarketDataFacade(_marketDataApi);
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await facade.GetBars(null!, DateTime.MinValue, DateTime.MaxValue));
        }

        [Fact]
        public async Task GetBars_ShouldThrowArgumentException_WhenStartDateIsLaterThanEndDate()
        {
            var facade = new MarketDataFacade(_marketDataApi);
            await Assert.ThrowsAsync<ArgumentException>(async () => await facade.GetBars("A contract"!, DateTime.MaxValue, DateTime.MinValue));
        }

        [Fact]
        public async Task GetContracts_ShouldReturnContractsCollection_WhenProvidedValidParameters()
        {
            var contracts = new List<Contract>
            {
                new Contract { Id = "1", Name = "Contract 1" },
                new Contract { Id = "2", Name = "Contract 2" }
            };

            var contractSearchResponse = new ContractSearchResponse
            {
                Contracts = contracts,
                Success = true
            };

            var facade = new MarketDataFacade(_marketDataApi);
            A.CallTo(() => _marketDataApi.GetContracts(A<ContractSearchRequest>._))
                .Returns(Task.FromResult(contractSearchResponse));

            var response = await facade.GetContracts("contracts", "Search Text", true);
            response.Should().NotBeNull();
            response.Count.Should().Be(2, "because the mocked API returns two contracts");
        }

        [Fact]
        public async Task GetContracts_ShouldThrowProjectXClientException_WhenApiErrorOccurs()
        {
            var facade = new MarketDataFacade(_marketDataApi);
            A.CallTo(() => _marketDataApi.GetContracts(A<ContractSearchRequest>._))
                .Throws(new Exception("API error"));

            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(async () => await facade.GetContracts("contracts", "Search Text", true));
            actualException.Message.Should().Be("Error getting contracts");
        }
    }
}