using Scd.ProjectX.Client.Models.MarketData;
using System.Text.Json;

namespace Scd.ProjectX.Client.Example.Observers
{
    public class MarketQuoteObserver : IObserver<MarketQuoteEvent>
    {
        private int _throughputCounter = 0;

        public void OnCompleted()
        {
            Console.WriteLine($"onCompleted: {nameof(MarketQuoteObserver)}");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public void OnNext(MarketQuoteEvent value)
        {
            var @event = JsonSerializer.Serialize(value);            
            Console.WriteLine($"MarketQuoteEvent ({++_throughputCounter}):\n {@event}");
        }
    }
}