using Scd.ProjectX.Client.Models.Orders;

namespace Scd.ProjectX.Client.Models.Trades
{
    public record Trade
    {
        /// <summary>
        /// Gets or set the unique identifier for the trade.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the account ID associated with the trade.
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// Gets or sets the contract ID associated with the trade.
        /// </summary>
        public string ContractId { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the timestamp when the trade was created.
        /// </summary>
        public DateTime CreationTimestamp { get; set; }
        /// <summary>
        /// Gets or sets the timestamp when the trade was last updated.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Gets or sets the profit and loss associated with the trade.
        /// </summary>
        public decimal ProfitAndLoss { get; set; }
        /// <summary>
        /// Gets or sets the fees associated with the trade.
        /// </summary>
        public decimal Fees { get; set; }
        /// <summary>
        /// Gets or sets the side of the trade (buy/sell).
        /// </summary>
        public Side Side { get; set; }
        /// <summary>
        /// Gets or sets the size of the trade, representing the number
        /// of contracts involved.
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the trade has been
        /// voided.
        /// </summary>
        public bool Voided { get; set; }
        /// <summary>
        /// Gets or sets the order ID associated with the trade.
        /// </summary>
        public int OrderId { get; set; }
    }
}