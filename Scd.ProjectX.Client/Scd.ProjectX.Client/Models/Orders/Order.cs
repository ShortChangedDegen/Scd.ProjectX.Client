namespace Scd.ProjectX.Client.Models.Orders
{
    /// <summary>
    /// Represents an order in the trading system.
    /// </summary>
    public record Order
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the account ID associated with the order.
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// Gets or sets the contract ID associated with the order.
        /// </summary>
        public string ContractId { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the timestamp when the order was created.
        /// </summary>
        public DateTime CreationTimestamp { get; set; }
        /// <summary>
        /// Gets or sets the timestamp when the order was last updated.
        /// </summary>
        public DateTime UpdateTimestamp { get; set; }
        /// <summary>
        /// Gets or sets the status of the order.
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Gets or sets the type of the order (e.g., market, limit, stop).
        /// </summary>
        public OrderType Type { get; set; }
        /// <summary>
        /// Gets or sets the side of the order (e.g., buy, sell).
        /// </summary>
        public Side Side { get; set; }
        /// <summary>
        /// Gets or sets the size of the order, which indicates how many units of the contract are being ordered.
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// Gets or sets the limit price for the order.
        /// </summary>
        public double? LimitPrice { get; set; }
        /// <summary>
        /// Gets or sets the stop price for the order.
        /// </summary>
        public double? StopPrice { get; set; }
    }
}