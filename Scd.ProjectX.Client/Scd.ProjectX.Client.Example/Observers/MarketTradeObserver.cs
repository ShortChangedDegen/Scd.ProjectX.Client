using Scd.ProjectX.Client.Models.MarketData;
using System.Text.Json;

namespace Scd.ProjectX.Client.Example.Observers
{
    public class MarketTradeObserver : IObserver<MarketTradeEvent>
    {
        public void OnCompleted()
        {
            Console.WriteLine($"onCompleted: {nameof(MarketTradeObserver)}");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public void OnNext(MarketTradeEvent value)
        {
            var @event = JsonSerializer.Serialize(value);
            Console.WriteLine(@event);
        }
    }
}