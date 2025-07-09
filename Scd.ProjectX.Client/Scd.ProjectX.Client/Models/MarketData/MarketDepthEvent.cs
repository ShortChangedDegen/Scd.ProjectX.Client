namespace Scd.ProjectX.Client.Models.MarketData
{
    /// <summary>
    /// Represents a market depth event containing information
    /// about price levels and volumes.
    /// </summary>
    public record MarketDepthEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the price level.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the total volume at this price level.
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// Gets or sets the current volume at this price level.
        /// </summary>
        public int CurrentVolume { get; set; }

        /// <summary>
        /// Gets or sets the type of the market depth event.
        /// </summary>
        public int Type { get; set; } // Why is this magical number called Type?

        /// <summary>
        /// Gets or sets the timestamp for the market depth event.
        /// </summary>
        public DateTime Timestamp { get; set; } = default;
    }
}