namespace Scd.ProjectX.Client.Models.Account
{
    public record RefreshTokenResponse : DefaultResponse
    {
        public string? NewToken { get; set; }
    }
}