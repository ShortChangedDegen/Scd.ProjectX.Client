using Scd.ProjectX.Client.Models.Positions;
using System.Text.Json;

namespace Scd.ProjectX.Client.Example.Observers
{
    public class UserPositionObserver : IObserver<UserPositionEvent>
    {
        public void OnCompleted()
        {
            Console.WriteLine($"onCompleted: {nameof(UserPositionObserver)}");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public void OnNext(UserPositionEvent value)
        {
            var @event = JsonSerializer.Serialize(value);
            Console.WriteLine(@event);
        }
    }
}