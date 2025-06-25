using Scd.ProjectX.Client.Models;

namespace Scd.ProjectX.Client.Messaging.Dispatchers
{
    /// <summary>
    /// Defines a contract for an event hub that can publish and
    /// subscribe to events of type <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventDispatcher<TEvent> : IObservable<TEvent>, IDisposable
        where TEvent : IEvent
    {
        void Init();
    }
}