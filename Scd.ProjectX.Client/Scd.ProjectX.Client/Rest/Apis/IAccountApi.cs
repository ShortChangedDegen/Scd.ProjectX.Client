using Refit;
using Scd.ProjectX.Client.Models.Account;

namespace Scd.ProjectX.Client.Rest.Apis
{
    public interface IAccountApi
    {
        [Headers("Accept: text/plain", "Content-Type: application/json",
            "Authorization: Bearer")]
        [Post("/api/Account/search")] // Get or post
        Task<AccountSearchResponse> SearchAccounts(AccountSearchRequest request);

        [Post("/api/Auth/loginKey")] // Post
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);

        [Post("/api/Auth/validate")] // Post
        Task<RefreshTokenResponse> RefreshToken();
    }
}