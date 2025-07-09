using Microsoft.Extensions.Options;
using Refit;
using Scd.ProjectX.Client.Models.Account;
using Scd.ProjectX.Client.Rest.Apis;
using System.Timers;

namespace Scd.ProjectX.Client.Utility
{
    /// <summary>
    /// This class handles authentication with the TopstepX API
    /// to retrieve an authentication token.
    /// </summary>
    /// <param name="options">ProjectX Client Settings</param>
    public class AuthTokenHandler(IOptions<ProjectXSettings> options) : IAuthTokenHandler
    {
        // Flag to indicate if the token should be refreshed

        private bool _refreshToken = false;

        private string? _token = null;

        private System.Timers.Timer _timer = new()
        {
            Interval = TimeSpan.FromMinutes(options.Value.TokenExpirationMinutes).TotalMilliseconds,
            AutoReset = true
        };

        /// <summary>
        /// Creates a new instance of the AuthTokenHandler class.
        /// </summary>
        /// <param name="username">The API username.</param>
        /// <param name="apiKey">The API SECRET key.</param>
        /// <param name="apiEndpoint">The API Url.</param>"
        public AuthTokenHandler(string username, string apiKey, string apiEndpoint)
            : this(Options.Create(new ProjectXSettings
            {
                Username = username,
                ApiKey = apiKey,
                ApiUrl = apiEndpoint,
                UserHubUrl = string.Empty,
                MarketHubUrl = string.Empty
            }))
        {
        }

        /// <summary>
        /// Retrieves an authentication token from the TopstepX API.
        /// </summary>
        /// <returns>A token when successful; otherwise, null.</returns>
        /// <exception cref="HttpRequestException">Thrown when an API request fails.</exception>
        public async Task<string?> GetToken()
        {
            if (_refreshToken && !string.IsNullOrEmpty(_token))
            {
                // If the token is expired, reset it and stop the timer
                await RefreshToken();
            }
            else if (!string.IsNullOrEmpty(_token))
            {
                return _token;
            }
            else
            {
                try
                {
                    await Authenticate();
                }
                catch (ApiException refitEx)
                {
                    _token = string.Empty;
                    // Hide normal implementation details about our inner dependency.
                    throw new ProjectXClientException($"Authentication failed: {refitEx.Message}", refitEx);
                }
            }

            return _token;
        }

        private async Task Authenticate()
        {
            // Unable to use the common method since all other endpoints will
            // require authentication or a token.
            var accountApi = RestService.For<IAccountApi>(options.Value.ApiUrl);
            var response = await accountApi.Authenticate(new AuthenticationRequest
            {
                UserName = options.Value.Username,
                ApiKey = options.Value.ApiKey
            });

            if (response.Success)
            {
                _token = response.Token;
                _refreshToken = false;
                _timer.Elapsed += OnTimerElapsed;
                _timer.Start();
                return;
            }

            throw new ProjectXClientException($"Authentication failed: {response.ErrorMessage}");
        }

        private async Task RefreshToken()
        {
            var accountApi = RestService.For<IAccountApi>(options.Value.ApiUrl);

            var response = await accountApi.RefreshToken();

            if (response.Success)
            {
                _token = response.NewToken;
                _refreshToken = false;
                _timer.Start();
            }
            else
            {
                _token = string.Empty; // Clear the token if refresh and force auth or fail.
                _timer.Stop(); // Stop the timer if refresh fails
            }
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            _refreshToken = true; // Invalidate the token after expiration
            _timer.Stop(); // Stop the timer until the next request
        }
    }
}