using Microsoft.Extensions.Logging;
using Scd.ProjectX.Client.Models.Positions;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// A facade to simplify access to position-related operations in the ProjectX API.
    /// </summary>
    public class PositionsFacade : IPositionsFacade
    {
        private readonly IPositionsApi _positionsApi;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionsFacade"/> class.
        /// </summary>
        /// <param name="positionApi">An implementation of <see cref="IPositionsApi"/>.</param>
        public PositionsFacade(IPositionsApi positionApi)
        {
            _positionsApi = Guard.NotNull(positionApi, nameof(positionApi));
        }

        /// <summary>
        /// Closes a contract for a specific account using the contract ID.
        /// </summary>
        /// <param name="accountId">An account ID.</param>
        /// <param name="contractId">A contract ID.</param>
        public async Task CloseContract(int accountId, string contractId) =>
            await CloseContract(new CloseRequest
            {
                AccountId = accountId,
                ContractId = Guard.NotNullOrEmpty(contractId, nameof(contractId))
            });

        /// <summary>
        /// Closes a contract for a specific account using the provided request object.
        /// </summary>
        /// <param name="request">The request object</param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task CloseContract(CloseRequest request)
        {
            Guard.NotNull(request, nameof(request));
            var response = await _positionsApi.CloseContract(request);
            if (!response.Success)
            {
                throw new InvalidOperationException($"Failed to close contract: {response.ErrorMessage}");
            }
        }

        /// <summary>
        /// Partially closes a contract for a specific account using the specified size.
        /// </summary>
        /// <param name="size">The number of positions to close.</param>
        public async Task PartiallyCloseContract(int size) =>
            await PartiallyCloseContract(new PartialCloseRequest
            {
                Size = size
            });

        /// <summary>
        /// Partially closes a contract for a specific account using the specified size.
        /// </summary>
        /// <param name="request"> The request object containing the size to close.</param>
        public async Task PartiallyCloseContract(PartialCloseRequest request)
        {
            Guard.NotNull(request, nameof(request));
            var response = await _positionsApi.PartiallyCloseContract(request);
            if (!response.Success)
            {
                throw new InvalidOperationException($"Failed to partially close contract: {response.ErrorMessage}");
            }
        }

        /// <summary>
        /// Searches for open positions for a specific account ID.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <returns>The open positions for the specifiec account.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<Position>> SearchOpenPositions(int accountId)
        {
            var response = await _positionsApi.SearchOpenPositions(accountId);
            if (response.Success)
            {
                return response.Positions ?? new List<Position>();
            }
            else
            {
                throw new InvalidOperationException(response.ErrorMessage);
            }
        }
    }
}
