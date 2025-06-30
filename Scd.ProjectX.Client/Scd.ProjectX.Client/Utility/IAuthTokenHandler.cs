namespace Scd.ProjectX.Client.Utility
{
    /// <summary>
    /// The Refit AuthTokenHandler interface is used to
    /// retrieve an authentication token.
    /// </summary>
    public interface IAuthTokenHandler
    {
        /// <summary>
        /// Retrieves an authentication token from the TopstepX API.
        /// </summary>
        /// <returns>A token when successful; otherwise, null.</returns>
        /// <exception cref="ProjectXClientException">Thrown when an API request fails.</exception>
        Task<string?> GetToken();
    }
}