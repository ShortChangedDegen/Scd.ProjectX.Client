namespace Scd.ProjectX.Client.Models.MarketData
{
    /// <summary>
    /// Represents a contract in the TopstepX API.
    /// </summary>
    public record Contract
    {
        /// <summary>
        /// Gets or sets the unique identifier for the contract.
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the contract.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the contract.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Gets or sets the exchange where the contract is traded.
        /// </summary>
        public decimal TickSize { get; set; }
        /// <summary>
        /// Gets or sets the value of a single tick for the contract.
        /// </summary>
        public decimal TickValue { get; set; }
        /// <summary>
        /// Gets or sets the multiplier for the contract, which indicates
        /// how many units of the underlying asset are represented by one contract.
        /// </summary>
        public bool ActiveContract { get; set; }
    }
}