namespace Scd.ProjectX.Client.Models.Orders
{
    /// <summary>
    /// Represents the type of an order in the TopstepX API.
    /// </summary>
    public enum OrderType
    {
        Unknown = 0,
        Limit = 1,
        Market = 2,
        Stop = 4,
        TrailingStop = 5,
        JoinBid = 6,
        JoinAsk = 7,
    }
}