namespace Scd.ProjectX.Client.Utility
{
    public class ProjectXClientException : HttpRequestException
    {
        private int _statusCode;

        public ProjectXClientException()
        {
        }

        public ProjectXClientException(string message)
            : base(message)
        {
        }

        public ProjectXClientException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ProjectXClientException(string message, Exception inner, int statusCode)
            : this(message, inner)
        {
            _statusCode = statusCode;
        }
    }
}