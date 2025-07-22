using Scd.ProjectX.Client.Models.MarketData;
using System.Text.Json;

namespace Scd.ProjectX.Client.Example.Observers
{
    public class MarketDepthObserver : IObserver<MarketDepthEvent>
    {
        private int _throughputCounter = 0;

        public void OnCompleted() => Console.WriteLine($"onCompleted: {nameof(MarketDepthObserver)}");

        public void OnError(Exception error) => Console.WriteLine(error);

        public void OnNext(MarketDepthEvent value)
        {
            // Hint: Dont's block if you want to keep up.
            var @event = JsonSerializer.Serialize(value);
            Console.WriteLine($"MarketDepthEvent ({++_throughputCounter}):\n {@event}");
        }
    }
}