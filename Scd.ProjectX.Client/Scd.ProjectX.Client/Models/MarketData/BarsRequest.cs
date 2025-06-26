using Scd.ProjectX.Client.Models.Orders;

namespace Scd.ProjectX.Client.Models.MarketData
{
    /// <summary>
    /// A market data candle request.
    /// </summary>
    public record BarsRequest
    {
        /// <summary>
        ///     The contract ID.
        /// </summary>
        public string? ContractId { get; set; }

        /// <summary>
        ///     Whether to retrieve bars using the sim or live data subscription.
        /// </summary>
        public required bool Live { get; set; } = true;

        /// <summary>
        ///     The start time of the historical data.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 	The end time of the historical data.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        ///     The unit of aggregation for the historical data.
        /// </summary>
        public required Unit? Unit { get; set; }

        /// <summary>
        /// 	The number of units to aggregate.
        /// </summary>
        public required int? UnitNumber { get; set; }

        /// <summary>
        ///     The maximum number of bars to retrieve.
        /// </summary>
        public required int? Limit { get; set; }

        /// <summary>
        /// 	Whether to include a partial bar representing the current time unit.
        /// </summary>
        public required bool IncludePartialBar { get; set; } = true;
    }
}