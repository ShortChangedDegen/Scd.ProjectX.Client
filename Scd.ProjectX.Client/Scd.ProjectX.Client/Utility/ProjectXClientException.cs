namespace Scd.ProjectX.Client.Utility
{
    public class ProjectXClientException : Exception
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
    }
}