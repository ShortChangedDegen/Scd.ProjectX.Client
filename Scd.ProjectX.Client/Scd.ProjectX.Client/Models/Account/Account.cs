namespace Scd.ProjectX.Client.Models.Account
{
    /// <summary>
    /// An account search result.
    /// </summary>
    public record Account
    {
        /// <summary>
        /// Gets or sets the unique identifier for the account.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the account balance.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets whether the account can trade.
        /// </summary>
        public bool CanTrade { get; set; }

        /// <summary>
        /// Gets or sets whether the account is visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets whether the record is from a simulated environment.
        /// </summary>
        public bool Simulated { get; set; }
    }
}