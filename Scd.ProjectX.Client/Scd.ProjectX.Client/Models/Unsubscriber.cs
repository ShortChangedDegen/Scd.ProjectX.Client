using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Models
{
    /// <summary>
    /// Unsubscribe an observer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Unsubscriber<T> : IDisposable
        where T : IEvent
    {
        private readonly IList<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;
        private bool _isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Unsubscriber{T}"/> class.
        /// </summary>
        /// <param name="observers">The collection of observers.</param>
        /// <param name="observer">The observer.</param>
        internal Unsubscriber(IList<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = Guard.NotNull(observers, nameof(observers));
            _observer = Guard.NotNull(observer, nameof(observer));
        }

        protected void Dispose(bool isDisposing = false)
        {
            if (_isDisposed)
            {
                return;
            }

            if (isDisposing)
            {
                if (_observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }
            }
            _isDisposed = true;
        }

        /// <summary>
        /// Disposes the resources used by the <see cref="Unsubscriber{T}"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}