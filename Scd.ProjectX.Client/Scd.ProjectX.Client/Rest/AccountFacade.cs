using Microsoft.Extensions.Logging;
using Scd.ProjectX.Client.Models.Account;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// A facade to simplify access to account-related operations in the ProjectX API.
    /// </summary>
    /// <remarks>This is intended to provide easier access, logging, etc.</remarks>
    public class AccountFacade : IAccountFacade
    {
        private readonly IAccountApi _accountApi;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountFacade"/> class.
        /// </summary>
        /// <param name="accountApi">An <see cref="IAccountApi"/> implementation.</param>

        public AccountFacade(IAccountApi accountApi)
        {
            _accountApi = Guard.NotNull(accountApi, nameof(accountApi));
        }

        /// <summary>
        /// Retrieves the user account information based on the provided filter.
        /// </summary>
        /// <param name="onlyActive">Indicates whether to include only active accounts.</param>
        /// <returns>A <see cref="List{Account}"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<Account>> GetUserAccount(bool onlyActive) =>
            await GetUserAccount(new AccountSearchRequest
            {
                OnlyActiveAccounts = onlyActive
            });

        /// <summary>
        /// Retrieves the user account information based on the provided filter.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <returns>A <see cref="List{Account}"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<Account>> GetUserAccount(AccountSearchRequest request)
        {
            var response = await _accountApi.SearchAccounts(request);

            if (response.Success)
            {
                if (response.Accounts is null || response.Accounts.Count == 0)
                {
                    return [];
                }

                return response.Accounts;
            }
            else
            {
                throw new InvalidOperationException(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Authenticates the user account with the provided username and API key.
        /// </summary>
        /// <param name="username">The username for the API authentication.</param>
        /// <param name="apiKey">The API key for the API authentication.</param>
        /// <returns>A JWT token.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<string> Authenticate(string username, string apiKey) =>
            await Authenticate(new AuthenticationRequest
            {
                UserName = username,
                ApiKey = apiKey
            });

        /// <summary>
        /// Authenticates the user account with the provided username and API key.
        /// </summary>
        /// <param name="request">The authentication request.</param>
        /// <returns>A JWT token.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<string> Authenticate(AuthenticationRequest request)
        {
            var response = await _accountApi.Authenticate(request);

            if (response.Success)
            {
                if (string.IsNullOrEmpty(response.Token))
                {
                    return string.Empty;
                }

                return response.Token;
            }
            else
            {
                throw new InvalidOperationException(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Refreshes the authentication token for the user account.
        /// </summary>
        /// <returns>The new JWT token.</returns>
        /// <exception cref="InvalidOperationException">Failed to refresh token.</exception>
        public async Task<string> RefreshToken()
        {
            var response = await _accountApi.RefreshToken();
            if (response.Success)
            {
                if (string.IsNullOrEmpty(response.NewToken))
                {
                    return string.Empty;
                }
                return response.NewToken;
            }
            else
            {
                throw new InvalidOperationException(response.ErrorMessage);
            }
        }
    }
}