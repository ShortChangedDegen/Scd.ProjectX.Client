namespace Scd.ProjectX.Client.Models.Orders
{
    /// <summary>
    /// Represents the side of an order in a trading context, indicating
    /// whether it is a buy or sell order.
    /// </summary>
    public enum Side
    {
        /// <summary>
        /// Default or unknown; used when the side is not specified
        /// or recognized.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Represents a buy order, indicating the intention to purchase
        /// an asset.
        /// </summary>
        Bid = 0,

        /// <summary>
        /// Represents a sell order, indicating the intention to sell
        /// an asset.
        /// </summary>
        Ask = 1,
    }
}