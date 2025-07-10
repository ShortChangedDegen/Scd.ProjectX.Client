namespace Scd.ProjectX.Client.Models.MarketData
{
    /// <summary>
    /// Represents a market trade event containing details about a
    /// trade execution.
    /// </summary>
    public record MarketTradeEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the full symbol identifier for the instrument associated with the trade.
        /// </summary>
        public string? SymbolId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the instrument associated with the trade.
        /// </summary>
        public required string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the price at which the trade was executed.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the trade occurred.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the type of trade event.
        /// </summary>
        public int Type { get; set; } //?? 0 = Trade, 1 = Depth ?.. 0 = Buy, 1 = Sell

        /// <summary>
        /// Gets or sets the volume of the trade.
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// Gets or sets the current volume of trades at the time of this event.
        /// </summary>
        public int CurrentVolume { get; set; }
    }
}