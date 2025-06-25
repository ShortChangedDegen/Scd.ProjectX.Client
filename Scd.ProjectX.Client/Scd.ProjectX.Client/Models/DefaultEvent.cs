namespace Scd.ProjectX.Client.Models
{
    /// <summary>
    /// A default event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record DefaultEvent<T> : IEvent
        where T : new()
    {
        /// <summary>
        /// The event action.
        /// </summary>
        /// <remarks>Needs to be an enum.</remarks>
        public int Type { get; set; } // 0 = Add, 1 = Update, 2 = Delete ??????

        /// <summary>
        /// The payload.
        /// </summary>
        public required T Arguments { get; set; }

        /// <summary>
        /// Gets or sets the target procedure for the event.
        /// </summary>
        public required string Target { get; set; }
    }
}