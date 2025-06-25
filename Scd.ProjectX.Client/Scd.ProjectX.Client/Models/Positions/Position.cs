using Scd.ProjectX.Client.Models.Orders;

namespace Scd.ProjectX.Client.Models.Positions
{
    /// <summary>
    /// Represents a position in the trading system.
    /// </summary>
    public record Position
    {
        /// <summary>
        /// Gets or sets the unique identifier for the position.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the account ID associated with the position.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the contract ID associated with the position.
        /// </summary>
        public string ContractId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the position was created.
        /// </summary>
        public DateTime CreationTimestamp { get; set; } = default;

        /// <summary>
        /// Gets or sets the timestamp when the position was last updated.
        /// </summary>
        public OrderType Type { get; set; }

        /// <summary>
        /// Gets or sets the price at which the position was opened.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the profit and loss associated with the position.
        /// </summary>
        public decimal AveragePrice { get; set; }
    }
}