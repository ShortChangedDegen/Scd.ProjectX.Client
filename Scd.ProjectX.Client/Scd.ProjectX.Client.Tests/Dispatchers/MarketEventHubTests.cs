using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Scd.ProjectX.Client.Messaging;
using Scd.ProjectX.Client.Models.MarketData;
using Scd.ProjectX.Client.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scd.ProjectX.Client.Tests.Dispatchers
{
    public class MarketEventHubTests
    {
        private IAuthTokenHandler _providedAuthTokenHandler;
        private IOptions<ProjectXSettings> _providedSettings;

        public MarketEventHubTests() 
        {
            _providedAuthTokenHandler = A.Fake<IAuthTokenHandler>();
            _providedSettings = Options.Create(new ProjectXSettings
            {
                ApiKey = "ApiKey",
                ApiUrl = "https://api.topstepx.com",
                MarketHubUrl = "https://market.topstepx.com",
                Symbols = [],
                TokenExpirationMinutes = 60,
                UserHubUrl = "https://user.topstepx.com",
                Username = "testuser",
            });
        }        

        [Fact]
        public void MarketEventHub_ShouldThrowArgumentNullException_WhenAuthTokenHandlerIsNull()
        {
            AuthTokenHandler? authTokenHandler = null;
            Assert.Throws<ArgumentNullException>(() => new UserEventHub(authTokenHandler, _providedSettings));
        }

        [Fact]
        public void MarketEventHub_ShouldThrowArgumentNullException_WhenProjectXSettingsIsNull()
        {
            IOptions<ProjectXSettings> providedOptions = null;
            var actualException = Assert.Throws<ArgumentNullException>(() => new UserEventHub(_providedAuthTokenHandler, providedOptions));
            actualException.Message.Should().Contain("projectXSettings");
        }

        [Fact(Skip = "External Calls from HubConnection")]
        public async Task StartAsync_ShouldGetAuthToken()
        {
            const string providedUrl = "https://api.topstepx.com";

            A.CallTo(() => _providedAuthTokenHandler.GetToken())
                .Returns(Task.FromResult("fake-auth-token"));

            var userEventHub = new UserEventHub(_providedAuthTokenHandler, _providedSettings);
            await userEventHub.StartAsync();
        }
    }
}
