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

                services.AddSingleton(sp =>
                {
                    var settings = new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        Error = (sender, args) =>
                        {                            
                            var errorContext = args.ErrorContext;
                            if (errorContext != null)
                            {                                                             
                                errorContext.Handled = true; // Mark the error as handled
                            }
                        }
                    };
                    return settings;
                });
            });
    }
}
