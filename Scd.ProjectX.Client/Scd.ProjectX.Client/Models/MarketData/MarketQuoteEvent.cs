namespace Scd.ProjectX.Client.Models.MarketData
{
    /// <summary>
    /// Represents a market quote event containing
    /// the best bid and ask prices for a specific instrument.
    /// </summary>
    public class MarketQuoteEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the full symbol identifier for the instrument associated with the trade.
        /// </summary>
        public string? SymbolId { get; set; }

        /// <summary>
        /// Gets or sets the instrument identifier for the quote.
        /// </summary>
        public string? Symbol { get; set; }

        /// <summary>
        /// Gets or sets the best bid price for the quote.
        /// </summary>
        public decimal BestBid { get; set; }

        /// <summary>
        /// Gets or sets the best ask price for the quote.
        /// </summary>
        public decimal BestAsk { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the quote.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the amount the price has changed.
        /// </summary>
        public decimal Change { get; set; }

        /// <summary>
        /// Gets or sets the percentage the price has changed.
        /// </summary>
        public decimal ChangePercent { get; set; }

        /// <summary>
        /// Get or set the open price.
        /// </summary>
        public decimal Open { get; set; }

        /// <summary>
        /// Get or set the high price.
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// Get or set the close price.
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// Get or set the low price.
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// Get or set the volume.
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Gets or sets the last updated timestamp of the quote.
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}