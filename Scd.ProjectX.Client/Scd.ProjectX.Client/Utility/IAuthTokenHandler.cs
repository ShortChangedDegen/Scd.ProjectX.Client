namespace Scd.ProjectX.Client.Utility
{
    public interface IAuthTokenHandler
    {
        Task<string?> GetToken();
    }
}