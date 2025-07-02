namespace Scd.ProjectX.Client.Utility
{
    /// <summary>
    /// ProjectX Client Exception
    /// </summary>
    public class ProjectXClientException : Exception
    {
        private int _statusCode;

        /// <summary>
        /// Initializes a new instance of the ProjectXClientException class.
        /// </summary>
        public ProjectXClientException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProjectXClientException class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public ProjectXClientException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProjectXClientException class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ProjectXClientException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProjectXClientException class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="inner">The inner exception</param>
        public ProjectXClientException(string message, int statusCode)
            : this(message)
        {
            _statusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the ProjectXClientException class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="inner">The inner exception</param>
        /// <param name="statusCode">A ProjectX ErrorCode.</param>
        public ProjectXClientException(string message, Exception inner, int statusCode)
            : this(message, inner)
        {
            _statusCode = statusCode;
        }
    }
}