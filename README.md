# ShortChangedDegen's ProjectX API Client
Provides common boilerplate functionality for communicating with the [ProjectX REST API and event hubs.](https://gateway.docs.projectx.com/docs/intro)

---
### Overview

### RESTful API Access



### Events and Observers
Messaging uses the Observer pattern to allow for an easy way to subscribe to events.  See more from [MSDN on the Observer Pattern](https://learn.microsoft.com/en-us/dotnet/standard/events/observer-design-pattern#when-to-apply-the-pattern). To receive events, you need to implement IObserver\<T> for the type of event you want to subscribe to. It can then be registered:
```
await projectXHub.Subscribe(contractIds, new MarketDepthObserver());
```

The types of events listed belo. Definitions should mirror the [ProjectX API Documentation](https://gateway.docs.projectx.com/docs/intro).
- MarketQuoteEvent
- MarketTradeEvent
- MarketDepthEvent (Requires L2 Data)
- UserAccountEvent
- UserOrderEvent
- UserTradeEvent
- UserPositionEvent


## Basic Usage
Add a section to the appsettings.json file for `ProjectXClient`, like below.
#### appsettings.json
``` json
...
},
{
    "ProjectXClient": {
        "Username": "<<USERNAME>>",
        "ApiKey": "<<APIKEY>>",
        "ApiUrl": "https://api.topstepx.com",
        "UserHubUrl": "https://rtc.topstepx.com/hubs/user",
        "MarketHubUrl": "https://rtc.topstepx.com/hubs/market",
        "TokenExpirationMinutes": 1380 // 23 "hours", token is good for 24 hours.
      }
},
...
```

### Register required services

Adding `.AddProjectXClientServices` to your HostBuilder will add all the necessary classes to the appropriate ServiceCollection. 

Obviously, it isn't necessary to use this extension method if you're not worried about `ProjectXApi` and `ProjectXHub`.

All registered services are located using the interface they implement, so ProjectXApi is located using something like `app.Services.GetService<IProjectXApi>()`.

```
var app = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    ...
                })
                .AddProjectXClientServices()
                .Build();    
```

### Accessing the ProjectX API
ProjectX REST APIs are made using the `ProjectXApi` class. Everything is organized by the endpoint being called. For instance, accounts can be accessed using `_projectXApi.Accounts`

```csharp
_projectXApi = app.Services.GetService<IProjectXApi>();
await _projectXApi.Accounts.Authenticate(username,  apikey);

```

### Register and Startingthe ProjectXHubs
ProjectX API real-time events are accesed through the `ProjectXHub` class. This is intended to simplify setting up to consume events. Before subscribing to any type of events, it is important to call `projectXHub.StartAsync()`.
It is necessary to implement the IObserver\<T> inteface for any class that needs to receive events. See Scd.ProjectX.Client.Example for a running console app.

```csharp
var projectXHub = app.Services.GetService<IProjectXHub>();
await projectXHub!.StartAsync();

if (contractIds.Any())
{
    await projectXHub.Subscribe(contractIds, new MarketQuoteObserver());
    await projectXHub.Subscribe(contractIds, new MarketTradeObserver());
    await projectXHub.Subscribe(contractIds, new MarketDepthObserver());
}

await projectXHub.Subscribe(new UserAccountObserver());
if (accountIds.Any())
{
    await projectXHub.Subscribe(accountIds, new UserOrderObserver());
    await projectXHub.Subscribe(accountIds, new UserTradeObserver());
    await projectXHub.Subscribe(accountIds, new UserPositionObserver());
```

#### Example IObserver\<T>
```csharp
public class MarketDepthObserver : IObserver<MarketDepthEvent>
    {
        private int _throughputCounter = 0;

        public void OnCompleted() => Console.WriteLine($"onCompleted: {nameof(MarketDepthObserver)}");

        public void OnError(Exception error) => Console.WriteLine(error);

        public void OnNext(MarketDepthEvent value)
        {
            var @event = JsonSerializer.Serialize(value);
            Console.WriteLine($"MarketDepthEvent ({++_throughputCounter}):\n {@event}");
        }
    }
```

### FAQ
- Why?

I decided to share some comomn functionality that needs to be implemented by everyone. Hopefully, it makes it
easier to get started developing with the ProjectX API and doing the interesting things you want to do.

[You can buy me a coffee if you're feeling generous](https://buymeacoffee.com/shortchangeddegen)

- Do you work for ProjectX?

Nope. I am just an independent programmer who follows the markets as a hobby.

- Is ShortChangeDegen a degenerate gambler or something who is always losing money?

No. It was the first thing I could think of when joining a discord for the first time. It was mostly tongue and cheek. We've all been there.
