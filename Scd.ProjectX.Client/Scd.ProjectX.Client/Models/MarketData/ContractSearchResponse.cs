namespace Scd.ProjectX.Client.Models.MarketData
{
    public record ContractSearchResponse : DefaultResponse
    {
        public List<Contract> Contracts { get; set; } = [];
    }
}