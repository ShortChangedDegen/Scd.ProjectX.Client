using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scd.ProjectX.Client.Example.Observers;
using Scd.ProjectX.Client.Messaging;
using Scd.ProjectX.Client.Models.MarketData;
using Scd.ProjectX.Client.Rest;
using Scd.ProjectX.Client.Rest.Apis;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Example
{
    internal class Program
    {
        private static IProjectXApi? _projectXApi;

        private static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<AuthTokenHandler>();
                    services.Configure<ProjectXSettings>(opts => context.Configuration.GetSection("ProjectX").Bind(opts));
                    
                    services.AddSingleton<IProjectXApi, ProjectXApi>();
                    services.AddSingleton<IAccountFacade, AccountFacade>();
                    services.AddSingleton<IMarketDataFacade, MarketDataFacade>();
                    services.AddSingleton<IOrderFacade, OrderFacade>();
                    services.AddSingleton<IPositionsFacade, PositionsFacade>();
                    services.AddSingleton<ITradesFacade, TradesFacade>();

                    services.AddSingleton<IProjectXHub, ProjectXHub>();
                    services.AddSingleton<IUserEventHub, UserEventHub>();
                    services.AddSingleton<IMarketEventHub, MarketEventHub>();
                });               

            var app = builder.Build();

            try
            {
                _projectXApi = app.Services.GetService<IProjectXApi>();

                Console.WriteLine("Accounts:");
                var accounts = await _projectXApi.Accounts.GetUserAccount(true);
                accounts = accounts.Where(x => x.Name.StartsWith("PRACTICE", StringComparison.InvariantCultureIgnoreCase)).ToList();
                accounts.ForEach(x => Console.WriteLine($"{x.Id} - {x.Name}"));
                if (!accounts?.Any() ?? false)
                {
                    Console.WriteLine("No accounts found");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Contracts:");
                var contracts = await _projectXApi.MarketData.GetContracts(new ContractSearchRequest
                {
                    SearchText = "",
                    Live = false
                });
                contracts.ForEach(x => Console.WriteLine($"{x.Id} - {x.Name}, {x.Description}"));

                var accountIds = accounts?.Select(a => a.Id!);
                var contractIds = contracts.Select(x => x.Id!);

                //await GetHistory(contractIds.First());

                var projectXHub = app.Services.GetService<IProjectXHub>();
                await projectXHub!.StartAsync();

                if (contractIds.Any())
                {
                    await projectXHub.Subscribe(contractIds, new MarketQuoteObserver());
                    await projectXHub.Subscribe(contractIds, new MarketTradeObserver());
                    await projectXHub.Subscribe(contractIds, new MarketDepthObserver());
                }

                await projectXHub.Subscribe(new UserAccountObserver());
                if (accountIds?.Any() ?? false)
                {
                    await projectXHub.Subscribe(accountIds, new UserOrderObserver());
                    await projectXHub.Subscribe(accountIds, new UserTradeObserver());
                    await projectXHub.Subscribe(accountIds, new UserPositionObserver());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }

            Console.ReadLine();
        }        
    }
}