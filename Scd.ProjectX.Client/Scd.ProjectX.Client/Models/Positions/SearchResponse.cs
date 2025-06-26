namespace Scd.ProjectX.Client.Models.Positions
{
    /// <summary>
    /// Represents a response containing a list of positions.
    /// </summary>
    public record SearchResponse : DefaultResponse
    {
        public List<Position> Positions { get; set; } = [];
    }
}