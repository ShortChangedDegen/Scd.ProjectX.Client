﻿using Polly;
using Scd.ProjectX.Client.Models;
using Scd.ProjectX.Client.Models.Positions;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;
using System.Collections.Generic;

namespace Scd.ProjectX.Client.Rest
{
    /// <summary>
    /// A facade to simplify access to position-related operations in the ProjectX API.
    /// </summary>
    /// <remarks>This is intended to provide easier access, logging, poly, etc.</remarks>
    public class PositionsFacade : IPositionsFacade
    {
        private readonly IPositionsApi _positionsApi;
        private readonly ResiliencePipeline _pipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionsFacade"/> class.
        /// </summary>
        /// <param name="positionApi">An implementation of <see cref="IPositionsApi"/>.</param>
        public PositionsFacade(IPositionsApi positionApi, ResiliencePipeline pipeline)
        {
            _positionsApi = Guard.NotNull(positionApi, nameof(positionApi));
            _pipeline = Guard.NotNull(pipeline, nameof(pipeline));
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
                ContractId = contractId
            });

        /// <summary>
        /// Closes a contract for a specific account using the provided request object.
        /// </summary>
        /// <param name="request">The request object</param>
        /// <exception cref="ProjectXClientException"></exception>
        public async Task CloseContract(CloseRequest request)
        {
            Guard.NotNull(request, nameof(request));
            Guard.NotDefault(request.AccountId, nameof(request.AccountId));
            Guard.NotNullOrEmpty(request.ContractId, nameof(request.ContractId));

            DefaultResponse response;

            try
            {
                response = await _pipeline.ExecuteAsync(async context =>
                    await _positionsApi.CloseContract(request)
                );                
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException($"Error closing contract: {ex.Message}", ex);
            }

            if (!response.Success)
            {
                throw new ProjectXClientException($"Failed to close contract: {response.ErrorMessage}", response.ErrorCode);
            }
        }

        /// <summary>
        /// Partially closes a contract for a specific account using the specified size.
        /// </summary>
        /// <param name="accountId">An account ID.</param>
        /// <param name="contractId">A contract ID.</param>
        /// <param name="size">The number of positions to close.</param>
        public async Task PartiallyCloseContract(int accountId, string contractId, int size) =>
            await PartiallyCloseContract(new PartialCloseRequest
            {
                AccountId = accountId,
                ContractId = contractId,
                Size = size
            });

        /// <summary>
        /// Partially closes a contract for a specific account using the specified size.
        /// </summary>
        /// <param name="request"> The request object containing the size to close.</param>
        public async Task PartiallyCloseContract(PartialCloseRequest request)
        {
            Guard.NotNull(request, nameof(request));
            Guard.NotDefault(request.AccountId, nameof(request.AccountId));
            Guard.NotNullOrEmpty(request.ContractId, nameof(request.ContractId));
            Guard.IsGreaterThan(0, request.Size, nameof(request.Size));

            DefaultResponse response;

            try
            {
                response = await _pipeline.ExecuteAsync(async context =>
                    await _positionsApi.PartiallyCloseContract(request)
                );
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException($"Error partially closing contract: {ex.Message}", ex);
            }

            if (!response.Success)
            {
                throw new ProjectXClientException($"Failed to partially close contract: {response.ErrorMessage}", response.ErrorCode);
            }
        }

        /// <summary>
        /// Searches for open positions for a specific account ID.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <returns>The open positions for the specifiec account.</returns>
        /// <exception cref="ProjectXClientException"></exception>
        public async Task<List<Position>> SearchOpenPositions(int accountId)
        {
            Guard.NotDefault(accountId, nameof(accountId));
            SearchResponse response;
            try
            {
                response = await _pipeline.ExecuteAsync(async context =>
                    await _positionsApi.SearchOpenPositions(accountId)
                );                
            }
            catch (Exception ex)
            {
                throw new ProjectXClientException($"Error searching open positions: {ex.Message}", ex);
            }

            return response.Success
                    ? response.Positions ?? []
                    : throw new ProjectXClientException($"Error searching open positions: {response.ErrorMessage}", response.ErrorCode);
        }
    }
}