using Scd.ProjectX.Client.Messaging.Dispatchers;
using Scd.ProjectX.Client.Models.MarketData;

namespace Scd.ProjectX.Client.Messaging
{
    public interface IMarketEventHub : IDisposable
    {
        /// <summary>
        /// Gets the market depth event hub.
        /// </summary>
        IEventDispatcher<MarketDepthEvent>? MarketDepthHub { get; }

        /// <summary>
        /// Gets the market quote event hub.
        /// </summary>
        IEventDispatcher<MarketQuoteEvent>? MarketQuoteHub { get; }

        /// <summary>
        /// Gets the market trade event hub.
        /// </summary>
        IEventDispatcher<MarketTradeEvent>? MarketTradeHub { get; }

        /// <summary>
        /// Subscribes one or more observers to <see cref="MarketDepthHub">.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketDepthEvent>[] observers);

        /// <summary>
        /// Subscribes one or more observers to <see cref="MarketQuoteEvent">.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketQuoteEvent>[] observers);

        /// <summary>
        /// Subscribes one or more observers to <see cref="MarketTradeEvent">.
        /// </summary>
        /// <param name="observers">One or more observers.</param>
        Task Subscribe(IEnumerable<string> contractIds, params IObserver<MarketTradeEvent>[] observers);

        /// <summary>
        /// Starts the <see cref="MarketEventHub"/>.
        /// </summary>
        /// <returns>A task.</returns>
        Task StartAsync();
    }
}