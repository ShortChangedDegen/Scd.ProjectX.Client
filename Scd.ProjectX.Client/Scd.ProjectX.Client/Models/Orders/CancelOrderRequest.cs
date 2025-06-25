namespace Scd.ProjectX.Client.Models.Orders
{
    /// <summary>
    /// Represents a request to cancel an order in the trading system.
    /// </summary>
    public record CancelOrderRequest
    {
        /// <summary>
        /// Gets or sets the account ID associated with the order to be cancelled.
        /// </summary>
        public required int AccountId { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier of the order to be cancelled.
        /// </summary>
        public required int OrderId { get; set; }
    }
}