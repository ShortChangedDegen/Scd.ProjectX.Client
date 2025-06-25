namespace Scd.ProjectX.Client.Models.Orders
{
    public record CreateOrderRequest
    {
        /// <summary>
        /// The account ID.
        /// </summary>
        public required int AccountId { get; set; }
        /// <summary>
        /// 	Gets or sets the contract ID.
        /// </summary>
        public required string ContractId { get; set; }
        /// <summary>
        /// Gets or sets the order type.
        /// </summary>
        public required OrderType Type { get; set; }
        /// <summary>
        /// Gets or sets the side of the order.
        /// </summary>
        public required Side Side { get; set; }
        /// <summary>
        /// Gets or sets the size of the order.
        /// </summary>
        public required int Size { get; set; }
        /// <summary>
        /// Gets or sets the limit price for the order, if applicable.
        /// </summary>
        public decimal? LimitPrice { get; set; }
        /// <summary>
        /// Gets or sets the stop price for the order, if applicable.
        /// </summary>
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Gets or sets the trail price for the order, if applicable.
        /// </summary>
        public decimal? TrailPrice { get; set; }
        /// <summary>
        /// Gets or sets an optional custom tag for the order.
        /// </summary>
        public string? CustomTag { get; set; }
        /// <summary>
        /// Gets or sets the linked order id.
        /// </summary>
        public int? LinkedOrderId { get; set; }
    }
}