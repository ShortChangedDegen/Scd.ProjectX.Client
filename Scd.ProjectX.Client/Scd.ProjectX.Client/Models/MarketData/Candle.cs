using Newtonsoft.Json;

namespace Scd.ProjectX.Client.Models.MarketData
{
    /// <summary>
    /// Represents a single market data candle.
    /// </summary>
    public record Candle
    {
        /// <summary>
        /// Gets or sets the timestamp of the candle.
        /// </summary>
        [JsonProperty("t")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Gets or sets the open of the candle.
        /// </summary>
        [JsonProperty("o")]
        public decimal Open { get; set; }
        /// <summary>
        /// Gets or sets the open of the candle.
        /// </summary>
        [JsonProperty("h")]
        public decimal High { get; set; }
        /// <summary>
        /// Gets or sets the low of the candle.
        /// </summary>
        [JsonProperty("l")]
        public decimal Low { get; set; }
        /// <summary>
        /// Gets or sets the close of the candle.
        /// </summary>
        [JsonProperty("c")]
        public decimal Close { get; set; }
        /// <summary>
        /// Gets or sets the volume of the candle.
        /// </summary>
        [JsonProperty("v")]
        public int Volume { get; set; }
    }
}