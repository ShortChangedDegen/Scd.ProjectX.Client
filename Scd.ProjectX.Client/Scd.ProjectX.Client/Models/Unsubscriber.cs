using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Models
{
    internal class Unsubscriber<T> : IDisposable
    {
        private readonly IList<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;
        private bool _isDisposed = false;

        internal Unsubscriber(IList<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = Guard.NotNull(observers, nameof(observers));
            _observer = Guard.NotNull(observer, nameof(observer));
        }

        protected async Task Dispose(bool isDisposing = false)
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}