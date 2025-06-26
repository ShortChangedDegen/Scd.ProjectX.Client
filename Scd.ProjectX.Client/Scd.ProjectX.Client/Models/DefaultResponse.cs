namespace Scd.ProjectX.Client.Models
{
    /// <summary>
    /// Represents a default response structure for API calls.
    /// </summary>
    public record DefaultResponse
    {
        /// <summary>
        /// Gets a value indicating whether the response was successful.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Gets a rudimentary error code. 0 indicates no error.
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Gets a more specific error message. An empty string indicates
        /// no error.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}