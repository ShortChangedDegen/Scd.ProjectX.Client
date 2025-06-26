namespace Scd.ProjectX.Client.Models.Orders
{
    /// <summary>
    /// Represents a request to update an existing order in the TopstepX API.
    /// </summary>
    public record ModifyRequest
    {
        /// <summary>
        /// Gets or sets the account ID associated with the order.
        /// </summary>
        public required int AccountId { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the order to be updated.
        /// </summary>
        public required int OrderId { get; set; }
        /// <summary>
        /// Gets or sets the contract ID associated with the order.
        /// </summary>
        public int? Size { get; set; }
        /// <summary>
        /// Gets or sets the limit price for the order.
        /// </summary>
        public decimal? LimitPrice { get; set; }
        /// <summary>
        /// Gets or sets the stop price for the order.
        /// </summary>
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Gets or sets the trail price for the order.
        /// </summary>
        public decimal? TrailPrice { get; set; }
    }
}