using Scd.ProjectX.Client.Models.Account;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// Defines a facade for account-related operations in the ProjectX API.
    /// </summary>
    /// <remarks>This is intended to provide easier access, logging, etc.</remarks>
    public interface IAccountFacade
    {
        /// <summary>
        /// Authenticates the user account with the provided username and API key.
        /// </summary>
        /// <param name="request">The authentication request.</param>
        /// <returns>A JWT token.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<string> Authenticate(AuthenticationRequest request);

        /// <summary>
        /// Authenticates the user account with the provided username and API key.
        /// </summary>
        /// <param name="username">The username for the API authentication.</param>
        /// <param name="apiKey">The API key for the API authentication.</param>
        /// <returns>A JWT token.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<string> Authenticate(string username, string apiKey);

        /// <summary>
        /// Retrieves the user account information based on the provided filter.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <returns>A <see cref="List{Account}"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<List<Account>> GetUserAccount(AccountSearchRequest request);

        /// <summary>
        /// Retrieves the user account information based on the provided filter.
        /// </summary>
        /// <param name="onlyActive">Indicates whether to include only active accounts.</param>
        /// <returns>A <see cref="List{Account}"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<List<Account>> GetUserAccount(bool onlyActive = true);

        /// <summary>
        /// Refreshes the authentication token for the user account.
        /// </summary>
        /// <returns>The new JWT token.</returns>
        /// <exception cref="InvalidOperationException">Failed to refresh token.</exception>
        Task<string?> RefreshToken();
    }
}