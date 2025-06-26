namespace Scd.ProjectX.Client.Models.Account
{
    /// <summary>
    /// Represents a response containing a list of accounts.
    /// </summary>
    /// <param name="accounts">The matching accounts.</param>
    public record AccountSearchResponse : DefaultResponse
    {
        public List<Account> Accounts { get; set; } = [];
    }
}