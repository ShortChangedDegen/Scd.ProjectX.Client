using Scd.ProjectX.Client.Models.Trades;
using System.Text.Json;

namespace Scd.ProjectX.Client.Example.Observers
{
    public class UserOrderObserver : IObserver<UserTradeEvent>
    {
        public void OnCompleted()
        {
            Console.WriteLine($"onCompleted: {nameof(UserTradeEvent)}");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public void OnNext(UserTradeEvent value)
        {
            var @event = JsonSerializer.Serialize(value);
            Console.WriteLine(@event);
        }
    }
}