using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Retry;
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

                services.AddResiliencePipeline(ResiliencePipelineName, builder =>
                    {
                        builder
                        .AddRetry(new RetryStrategyOptions
                        {
                            BackoffType = DelayBackoffType.Exponential,
                            MaxRetryAttempts = 5,
                            Delay = TimeSpan.FromSeconds(2),
                            // Prevent repetitive race conditions by adding random jitter to the delay
                            UseJitter = true
                        })
                        .AddTimeout(TimeSpan.FromSeconds(5))
                        .Build();
                    });
            });
            
        public static string ResiliencePipelineName { get; } = "scd-projectx-client";
    }
}
