namespace Scd.ProjectX.Client.Models.Orders
{
    /// <summary>
    /// Represents the unit of time for aggregating market data or orders.
    /// </summary>
    public enum Unit
    {
        None = 0,
        Second = 1,
        Minute = 2,
        Hour = 31,
        Day = 4,
        Week = 5,
        Month = 6
    }
}