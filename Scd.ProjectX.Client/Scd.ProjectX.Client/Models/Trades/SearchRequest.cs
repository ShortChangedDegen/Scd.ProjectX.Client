namespace Scd.ProjectX.Client.Models.Trades
{
    /// <summary>
    /// Represents a request to search for trades within a specific account
    /// and time range.
    /// </summary>
    public record SearchRequest
    {
        /// <summary>
        /// Default Constructor for SearchRequest.
        /// </summary>
        public SearchRequest()
        {
        }

        /// <summary>
        /// Gets the account ID.
        /// </summary>
        public required int AccountId { get; set; }

        /// <summary>
        /// Getsthe start timestamp for the search.
        /// </summary>
        public required DateTime StartTimestamp { get; set; }

        /// <summary>
        /// Gets the end timestamp for the search.
        /// </summary>
        public DateTime EndTimestamp { get; set; }
    }
}