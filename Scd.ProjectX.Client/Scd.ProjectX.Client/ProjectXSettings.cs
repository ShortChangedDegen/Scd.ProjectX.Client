namespace Scd.ProjectX.Client
{
    /// <summary>
    /// Represents the configuration options for the TopstepX API client.
    /// </summary>
    public record ProjectXSettings
    {
        /// <summary>
        /// Gets the section name.
        /// </summary>
        public const string SectionName = "ProjectX";

        /// <summary>
        /// Gets or sets the username for the API authentication.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the API key for the API authentication.
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// Gets or sets the base URL for the TopstepX API.
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL for receiving user events.
        /// </summary>
        public string UserHubUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL for receiving market events.
        /// </summary>
        public string MarketHubUrl { get; set; }

        /// <summary>
        /// Gets or sets the instrument symbols to pull market events for.
        /// </summary>
        public string[] Symbols { get; set; }

        /// <summary>
        /// Gets or sets the token expiration time in minutes.
        /// </summary>
        public int TokenExpirationMinutes { get; set; } = 1440;
    }
}