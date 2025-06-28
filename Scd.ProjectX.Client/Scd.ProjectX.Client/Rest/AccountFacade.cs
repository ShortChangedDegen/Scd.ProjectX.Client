using Scd.ProjectX.Client.Models.Account;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// A facade to simplify access to account-related operations in the ProjectX API.
    /// </summary>
    /// <remarks>This is intended to provide easier access, validation, etc.</remarks>
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
        public async Task<List<Account>> GetUserAccount(bool onlyActive = true) =>
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
            try
            {
                var response = await _accountApi.SearchAccounts(Guard.NotNull(request, nameof(request)));
                return response.Success ? response.Accounts ?? [] : [];
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException("Error getting user accounts", ex);
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
                UserName = Guard.NotNullOrEmpty(username, nameof(username)),
                ApiKey = Guard.NotNullOrEmpty(apiKey, nameof(apiKey)),
            });

        /// <summary>
        /// Authenticates the user account with the provided username and API key.
        /// </summary>
        /// <param name="request">The authentication request.</param>
        /// <returns>A JWT token.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<string?> Authenticate(AuthenticationRequest request)
        {
            Guard.NotNull(request, nameof(request));
            Guard.NotNull(request.UserName, nameof(request.UserName));
            Guard.NotNullOrEmpty(request.ApiKey, nameof(request.ApiKey));

            try
            {
                var response = await _accountApi.Authenticate(request);
                return response.Success ? response?.Token : string.Empty;
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException($"Error authenticating user", ex);
            }
        }

        /// <summary>
        /// Refreshes the authentication token for the user account.
        /// </summary>
        /// <returns>The new JWT token.</returns>
        /// <exception cref="InvalidOperationException">Failed to refresh token.</exception>
        public async Task<string?> RefreshToken()
        {
            try
            {
                var response = await _accountApi.RefreshToken();
                return response.Success ? response?.NewToken : string.Empty;
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException("Error refreshing token", ex);
            }
        }
    }
}