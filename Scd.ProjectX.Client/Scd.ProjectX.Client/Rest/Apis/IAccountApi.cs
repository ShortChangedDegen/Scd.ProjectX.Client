using Refit;
using Scd.ProjectX.Client.Models.Account;

namespace Scd.ProjectX.Client.Rest.Apis
{
    public interface IAccountApi
    {
        [Headers("Accept: text/plain", "Content-Type: application/json",
            "Authorization: Bearer")]
        [Post("/api/Account/search")]
        Task<AccountSearchResponse> SearchAccounts(AccountSearchRequest request);

        [Post("/api/Auth/loginKey")]
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);

        [Post("/api/Auth/validate")]
        Task<RefreshTokenResponse> RefreshToken();
    }
}