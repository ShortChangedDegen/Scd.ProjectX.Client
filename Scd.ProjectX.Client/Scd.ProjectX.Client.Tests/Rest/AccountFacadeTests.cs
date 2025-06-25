using FakeItEasy;
using FluentAssertions;
using Scd.ProjectX.Client.Models.Account;
using Scd.ProjectX.Client.Rest;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Tests.Rest
{
    public class AccountFacadeTests
    {
        private IAccountApi _accountApi;

        public AccountFacadeTests()
        {
            _accountApi = A.Fake<IAccountApi>();
        }

        [Fact]
        public void AccountFacade_ShouldThrowArgumentNullException_WhenAccountApiIsNull()
        {
            var thrownException = Assert.Throws<ArgumentNullException>(() => new AccountFacade(null));
            thrownException.Message.Should().Contain("accountApi");

        }

        [Fact]
        public void GetUserAccount_ShouldThrowArgumentNullException_WhenProvidedRequestIsNull()
        {
            var accountFacade = new AccountFacade(_accountApi);
            var actualException = Assert.ThrowsAsync<ArgumentNullException>(() => accountFacade.GetUserAccount(null));            
        }

        [Fact]
        public void GetUserAccount_ShouldThrowProjectXException_WhenAnErrorOccursCallingSearchAccounts()
        {
            var accountFacade = new AccountFacade(_accountApi);
            A.CallTo(() => _accountApi.SearchAccounts(A<AccountSearchRequest>._)).Throws(new Exception("Test Error"));

            var actualException = Assert.ThrowsAsync<ProjectXClientException>(async () => await accountFacade.GetUserAccount(true));

            
        }

        [Fact]
        public void GetUserAccount_ShouldCallAccountApi_WhenProvidedValidRequest()
        {
            var accountFacade = new AccountFacade(_accountApi);
            var response = accountFacade.GetUserAccount(true);

            A.CallTo(() => _accountApi.SearchAccounts(A<AccountSearchRequest>._)).MustHaveHappened();
        }

        [Fact]
        public async Task Authenticate_ShouldThrowArgumentNullException_WhenRequestOrItsUsernameOrApiKeyIsNull()
        {
            var accountFacade = new AccountFacade(_accountApi);
            var actualException = await Assert.ThrowsAsync<ArgumentNullException>(() => accountFacade.Authenticate(null, "apiKey"));
            actualException.Message.Should().Contain("username");
            actualException = await Assert.ThrowsAsync<ArgumentNullException>(() => accountFacade.Authenticate("username", null));
            actualException.Message.Should().Contain("apiKey");
        }

        [Fact]
        public void Authenticate_ShouldThrowProjectXClientException_WhenCallToApiFails()
        {
            A.CallTo(() => _accountApi.Authenticate(A<AuthenticationRequest>._))
                .Throws(new Exception("Test Error"));
            var accountFacade = new AccountFacade(_accountApi);

            var exception = Assert.ThrowsAsync<ProjectXClientException>(() => accountFacade.Authenticate(new AuthenticationRequest { ApiKey = "test", UserName = "test"}));
            exception.Result.Message.Should().Contain("Error authenticating user");
        }

        [Fact]
        public async Task Authenticate_ShouldReturnExpectedToken_WhenCallToApiSuccessful()
        {
            var expectedToken = "TestToken";
            A.CallTo(() => _accountApi.Authenticate(A<AuthenticationRequest>._))
                .Returns(new AuthenticationResponse { Success = true, Token = expectedToken });

            var accountFacade = new AccountFacade(_accountApi);

            var response = await accountFacade.Authenticate(
                new AuthenticationRequest
                {
                    UserName = "username",
                    ApiKey = "password"
                });
            
            response.Should().Be(expectedToken);
        }

        [Fact]
        public async Task Authenticate_ShouldEmptyString_WhenCallToApiNotSuccessful()
        {
            var expectedToken = string.Empty;
            A.CallTo(() => _accountApi.Authenticate(A<AuthenticationRequest>._))
                .Returns(new AuthenticationResponse { Success = false, Token = "NotAnEmptyToken"});

            var accountFacade = new AccountFacade(_accountApi);

            var response = await accountFacade.Authenticate(new AuthenticationRequest{ UserName = "username", ApiKey = "password"});

            response.Should().Be(expectedToken);
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnExpectedToken_WhenSuccessful()
        {
            var expectedToken = "A test token";
            A.CallTo(() => _accountApi.RefreshToken())
                .Returns(new RefreshTokenResponse { Success = true, NewToken = expectedToken });

            var accountFacade = new AccountFacade(_accountApi);

            var response = await accountFacade.RefreshToken();

            response.Should().Be(expectedToken);
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnExpectedToken_WhenNotSuccessful()
        {
            var expectedToken = "";
            A.CallTo(() => _accountApi.RefreshToken())
                .Returns(new RefreshTokenResponse { Success = false, NewToken = "NotAnEmptyValue" });

            var accountFacade = new AccountFacade(_accountApi);

            var response = await accountFacade.RefreshToken();

            response.Should().Be(expectedToken);
        }

        [Fact]
        public async Task RefreshToken_ShouldThrowExpectedException_WhenCallToApiThrows()
        {
            var expectedMessage = "Error refreshing token";
            A.CallTo(() => _accountApi.RefreshToken())
                .Throws(new Exception(expectedMessage));

            var accountFacade = new AccountFacade(_accountApi);

            var actualException = await Assert.ThrowsAsync<ProjectXClientException>(async () => await accountFacade.RefreshToken());

            actualException.Message.Should().Be(expectedMessage);
        }
    }
}
