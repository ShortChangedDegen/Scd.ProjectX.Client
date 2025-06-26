using Scd.ProjectX.Client.Models.Positions;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// Defines a facade for account-related operations in the ProjectX API.
    /// </summary>
    public interface IPositionsFacade
    {
        /// <summary>
        /// Closes a contract for a specific account using the provided request object.
        /// </summary>
        /// <param name="request">The request object</param>
        /// <exception cref="InvalidOperationException"></exception>
        Task CloseContract(CloseRequest request);

        /// <summary>
        /// Closes a contract for a specific account using the contract ID.
        /// </summary>
        /// <param name="accountId">An account ID.</param>
        /// <param name="contractId">A contract ID.</param>
        Task CloseContract(int accountId, string contractId);

        /// <summary>
        /// Partially closes a contract for a specific account using the specified size.
        /// </summary>
        /// <param name="size">The number of positions to close.</param>
        Task PartiallyCloseContract(int size);

        /// <summary>
        /// Partially closes a contract for a specific account using the specified size.
        /// </summary>
        /// <param name="request"> The request object containing the size to close.</param>

        Task PartiallyCloseContract(PartialCloseRequest request);

        /// <summary>
        /// Searches for open positions for a specific account ID.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <returns>The open positions for the specifiec account.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<List<Position>> SearchOpenPositions(int accountId);
    }
}