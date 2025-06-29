namespace Scd.ProjectX.Client.Utility
{
    public class ProjectXClientException : HttpRequestException
    {
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

        public ProjectXClientException(string message, Exception inner, System.Net.HttpStatusCode httpStatusCode)
            : base(message, inner, httpStatusCode)
        {
        }

        public ProjectXClientException(HttpRequestError httpRequestError, string message, Exception inner, System.Net.HttpStatusCode httpStatusCode)
            : base(httpRequestError, message, inner, httpStatusCode)
        {
        }
    }
}