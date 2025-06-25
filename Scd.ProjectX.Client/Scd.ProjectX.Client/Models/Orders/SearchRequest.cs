namespace Scd.ProjectX.Client.Models.Orders
{
    /// <summary>
    /// Represents a request to search for orders.
    /// </summary>
    public class SearchRequest
    {
        /// <summary>
        /// Gets or sets the account ID.
        /// </summary>
        public required int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the start timestamp for the search.
        /// </summary>
        /// <remarks>
        /// Consider breaking into two separate models.
        /// </remarks>
        public required DateTime StartTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the end timestamp for the search.
        /// </summary>
        public DateTime? EndTimestamp { get; set; }
    }
}