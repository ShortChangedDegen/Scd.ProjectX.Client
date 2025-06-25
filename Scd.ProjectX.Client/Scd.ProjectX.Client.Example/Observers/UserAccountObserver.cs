using Scd.ProjectX.Client.Models.Account;
using System.Text.Json;

namespace Scd.ProjectX.Client.Example.Observers
{
    public class UserAccountObserver : IObserver<UserAccountEvent>
    {
        private int _throughputCounter = 0;

        public void OnCompleted() => Console.WriteLine($"onCompleted: {nameof(UserAccountObserver)}");

        public void OnError(Exception error) => Console.WriteLine(error);

        public void OnNext(UserAccountEvent value)
        {
            var @event = JsonSerializer.Serialize(value);
            Console.WriteLine($"UserAccountEvent ({++_throughputCounter}):\n {@event}");
        }
    }
}