namespace Scd.ProjectX.Client.Models.Positions
{
    /// <summary>
    /// A close order request.
    /// </summary>
    /// <param name="accountId">The account ID associated with the close request</param>
    /// <param name="contractId">The contract ID</param>
    public record CloseRequest(int accountId, string contractId = "")
    {
        /// <summary>
        /// Default Constructor for CloseRequest.
        /// </summary>
        public CloseRequest() : this(0, string.Empty)
        {
        }

        /// <summary>
        /// Gets or sets the account ID associated with the close request.
        /// </summary>
        public int AccountId { get; set; } = accountId;

        /// <summary>
        /// Gets or sets the contract ID.
        /// </summary>
        public string ContractId { get; set; } = contractId;
    }
}