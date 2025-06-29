using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Scd.ProjectX.Client.Messaging;
using Scd.ProjectX.Client.Rest;

namespace Scd.ProjectX.Client.Utility
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddProjectXClientServices(this IHostBuilder hostBuilder) =>
            hostBuilder.ConfigureServices((context, services) =>
            {
                // Register the ProjectX API settings
                services.Configure<ProjectXSettings>(opts => context.Configuration.GetSection(ProjectXSettings.SectionName).Bind(opts));

                // Register the ProjectX API and its facades
                services.AddSingleton<IAuthTokenHandler, AuthTokenHandler>();
                services.AddSingleton<IAccountFacade, AccountFacade>();
                services.AddSingleton<IMarketDataFacade, MarketDataFacade>();
                services.AddSingleton<IOrdersFacade, OrdersFacade>();
                services.AddSingleton<IPositionsFacade, PositionsFacade>();
                services.AddSingleton<ITradesFacade, TradesFacade>();
                services.AddSingleton<IProjectXApi, ProjectXApi>();

                // Register the hubs
                services.AddSingleton<IProjectXHub, ProjectXHub>();
                services.AddSingleton<IUserEventHub, UserEventHub>();
                services.AddSingleton<IMarketEventHub, MarketEventHub>();

                services.AddSingleton<JsonSerializerSettings>(sp =>
                {
                    var settings = new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        Error = (sender, args) =>
                        {
                            // Handle JSON deserialization errors
                            var errorContext = args.ErrorContext;
                            if (errorContext != null)
                            {
                                // Log the error or handle it as needed
                                Console.WriteLine($"JSON Deserialization Error: {errorContext.Error.Message}");
                                errorContext.Handled = true; // Mark the error as handled
                            }
                        }
                    };
                    return settings;
                });
            });
    }
}
