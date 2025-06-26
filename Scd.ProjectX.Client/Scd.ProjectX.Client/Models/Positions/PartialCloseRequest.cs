namespace Scd.ProjectX.Client.Models.Positions
{
    /// <summary>
    /// A request to partially close a position.
    /// </summary>
    /// <param name="size">The size of the order.</param>
    public record PartialCloseRequest(int size) : CloseRequest
    {
        /// <summary>
        /// Default Constructor for PartialCloseRequest.
        /// </summary>
        public PartialCloseRequest() : this(0)
        {
        }

        /// <summary>
        /// Gets or sets the size of the order.
        /// </summary>
        public int Size { get; set; } = size;
    }
}