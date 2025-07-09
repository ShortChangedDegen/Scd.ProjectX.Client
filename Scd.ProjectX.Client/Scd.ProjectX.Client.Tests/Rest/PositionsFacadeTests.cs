using FakeItEasy;
using FluentAssertions;
using Polly;
using Scd.ProjectX.Client.Models;
using Scd.ProjectX.Client.Models.Positions;
using Scd.ProjectX.Client.Rest;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Tests.Rest
{
    public class PositionsFacadeTests
    {
        private readonly IPositionsApi _positionsApi;
        private readonly IPositionsFacade _positionsFacade;
        private readonly ResiliencePipeline _pipeline = ResiliencePipeline.Empty;

        public PositionsFacadeTests()
        {
            _positionsApi = A.Fake<IPositionsApi>();
            _positionsFacade = new PositionsFacade(_positionsApi, _pipeline);
        }

        [Fact]
        public void PositionsFacade_ShouldThrowArgumentNullException_WhenPositionsApiIsNull()
        {
            IPositionsApi? nullPositionsApi = null;
            Assert.Throws<ArgumentNullException>(() => new PositionsFacade(nullPositionsApi, _pipeline));
        }

        [Fact]
        public async Task CloseContract_ShouldThrowArgumentNullException_WhenCloseRequestIsNull()
        {
            CloseRequest request = null;

            var actualException = await Assert.ThrowsAsync<ArgumentNullException>(() => _positionsFacade.CloseContract(request));
            actualException.Message.Should().Contain("request");
        }

        [Fact]
        public async Task CloseContract_ShouldThrowArgumentException_WhenCloseRequestAccountIdIsDefault()
        {
            CloseRequest request = new CloseRequest
            {
                AccountId = default,
                ContractId = "contract123",
            };

            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _positionsFacade.CloseContract(request));
            actualException.Message.Should().Contain("AccountId");
        }

        [Fact]
        public async Task CloseContract_ShouldThrowArgumentException_WhenCloseRequestContractIdIsNull()
        {
            CloseRequest request = new CloseRequest
            {
                AccountId = default,
                ContractId = null,
            };

            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _positionsFacade.CloseContract(request));
            actualException.Message.Should().Contain("AccountId");
        }

        [Fact]
        public async Task CloseContract_ShouldThrowProjectXClientException_WhenRequestNotSuccessful()
        {
            var request = new CloseRequest
            {
                AccountId = 12345,
                ContractId = "ContractId",
            };

            A.CallTo(() => _positionsApi.CloseContract(request))
                .Returns(new DefaultResponse { Success = false, ErrorMessage = "Test Error" });

            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(() => _positionsFacade.CloseContract(request));
            actualException.Message.Should().Contain("Test Error");
        }

        [Fact]
        public async Task CloseContract_ShouldThrowProjectXClientException_WhenPositionsApiThrowsException()
        {
            var request = new CloseRequest
            {
                AccountId = 12345,
                ContractId = "ContractId",
            };

            A.CallTo(() => _positionsApi.CloseContract(request))
                .Throws(new Exception("Failed to partially close contract"));

            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(() => _positionsFacade.CloseContract(request));
            actualException.Message.Should().Contain("Failed to partially close contract");
        }

        [Fact]
        public async Task PartiallyCloseContract_ShouldThrowArgumentNullException_WhenProvidedRequestIsNull()
        {
            PartialCloseRequest request = null;

            var actualException = await Assert.ThrowsAsync<ArgumentNullException>(() => _positionsFacade.PartiallyCloseContract(request));
            actualException.Message.Should().Contain("request");
        }

        [Fact]
        public async Task PartiallyCloseContract_ShouldThrowArgumentException_WhenProvidedRequestAccountIdIsDefault()
        {
            PartialCloseRequest request = new PartialCloseRequest
            {
                AccountId = default,
                ContractId = "contract123",
                Size = 10
            };

            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _positionsFacade.PartiallyCloseContract(request));
            actualException.Message.Should().Contain("AccountId");
        }

        [Fact]
        public async Task PartiallyCloseContract_ShouldThrowArgumentException_WhenProvidedRequestContractIdIsNullOrEmpty()
        {
            PartialCloseRequest request = new PartialCloseRequest
            {
                AccountId = 1234,
                ContractId = null,
                Size = 10
            };

            var actualException = await Assert.ThrowsAsync<ArgumentNullException>(() => _positionsFacade.PartiallyCloseContract(request));
            actualException.Message.Should().Contain("ContractId");
        }

        [Fact]
        public async Task PartiallyCloseContract_ShouldThrowArgumentException_WhenProvidedRequestSizeIsNotGreaterThanZero()
        {
            PartialCloseRequest request = new PartialCloseRequest
            {
                AccountId = 1234,
                ContractId = "test id",
                Size = 0
            };

            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _positionsFacade.PartiallyCloseContract(request));
            actualException.Message.Should().Contain("Size");
        }

        [Fact]
        public async Task SearchOpenPositions_ShouldThrowArgumentException_WhenAccountIdIsDefault()
        {
            int accountId = default;

            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => _positionsFacade.SearchOpenPositions(accountId));
            actualException.Message.Should().Contain("accountId");
        }

        [Fact]
        public async Task SearchOpenPositions_ShouldThrowProjectXClientException_WhenResponseNotSuccessful()
        {
            int accountId = 123456;
            A.CallTo(() => _positionsApi.SearchOpenPositions(accountId)).Throws(new Exception("Test Error"));
            await Assert.ThrowsAsync<ProjectXClientException>(() => _positionsFacade.SearchOpenPositions(accountId));
        }

        [Fact]
        public async Task SearchOpenPositions_ShouldThrowProjectXClientException_WhenPositionsApiThrowsException()
        {
            int accountId = 12345;

            A.CallTo(() => _positionsApi.SearchOpenPositions(accountId))
                .Throws(new Exception("Failed to retrieve open positions"));

            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(() => _positionsFacade.SearchOpenPositions(accountId));
            actualException.Message.Should().Contain("Error searching open positions");
        }
    }
}