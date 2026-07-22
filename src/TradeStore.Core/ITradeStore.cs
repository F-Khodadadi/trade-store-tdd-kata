using TradeStore.Core.Model;

namespace TradeStore.Core;

public interface ITradeStore
{
    /// <summary>
    /// Returns a list of trades for a specific customer based on the provided customerId.
    /// </summary>
    /// <param name="customerId">The ID of the customer whose trades are to be retrieved.</param>
    /// <returns>A read-only list of trades for the specified customer.</returns>
    IReadOnlyList<Trade> GetByCustomerId(int customerId);
    
    /// <summary>
    /// Adds a new trade to the store.
    /// </summary>
    /// <param name="trade">The trade to be added.</param>
    void Add(Trade trade);
    
    /// <summary>
    /// Removes a trade from the store based on the provided tradeId. If no trade with the specified ID exists, no action is taken.
    /// </summary>
    /// <param name="tradeId">The ID of the trade to be removed.</param>
    void Remove(int tradeId);
    
    /// <summary>
    /// Removes trades from the store that match the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each trade for a condition.</param>
    void Remove(Func<Trade, bool> predicate);
    
}