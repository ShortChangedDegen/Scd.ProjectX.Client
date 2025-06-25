using Scd.ProjectX.Client.Models.Trades;
using System.Text.Json;

namespace Scd.ProjectX.Client.Example.Observers
{
    public class UserTradeObserver : IObserver<UserTradeEvent>
    {
        private int _throughputCounter = 0;
        public void OnCompleted()
        {
            Console.WriteLine($"onCompleted: {nameof(UserTradeObserver)}");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public void OnNext(UserTradeEvent value)
        {
            var @event = JsonSerializer.Serialize(value);
            Console.WriteLine($"UserTradeEvent ({++_throughputCounter}):\n {@event}");
        }
    }
}