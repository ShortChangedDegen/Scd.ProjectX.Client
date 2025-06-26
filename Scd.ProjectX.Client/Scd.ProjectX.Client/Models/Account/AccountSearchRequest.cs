namespace Scd.ProjectX.Client.Models.Account
{
    /// <summary>
    /// An account search request.
    /// </summary>
    public record AccountSearchRequest
    {
        /// <summary>
        /// Gets or sets whether to include only active accounts.
        /// </summary>
        public bool OnlyActiveAccounts { get; set; }
    }
}