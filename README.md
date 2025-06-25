# ShortChangeDegen's ProjectX API Client [^1]
Provides common boilerplate functionality for communicating with the [ProjectX REST API and event hubs.](https://gateway.docs.projectx.com/docs/intro) [^1]
[^1]: No affiliation with ProjectX is implied or intended.
---


### Overview

### RESTful API Access



### Events and Observers
Messaging uses the Observer pattern to allow for an easy way to subscribe to events.  See more from [MSDN on the Observer Pattern](https://learn.microsoft.com/en-us/dotnet/standard/events/observer-design-pattern#when-to-apply-the-pattern)

### Solution Structure


### Basic Usage
#### appsettings.json
``` json
...
},
{
    "ProjectXSdk": {
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

#### Bootstrapping
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
if (accountIds?.Any() ?? false)
{
    await projectXHub.Subscribe(accountIds, new UserOrderObserver());
    await projectXHub.Subscribe(accountIds, new UserTradeObserver());
    await projectXHub.Subscribe(accountIds, new UserPositionObserver());
}
```

### FAQ
 I decided to share some comon functionality that needs to be implemented by everyone and that can be easy to get wrong.

 ---

 [^1]: No affiliation with ProjectX is implied or intended.
