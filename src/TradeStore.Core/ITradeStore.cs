namespace TradeStore.Core;

public interface ITradeStore
{
    IReadOnlyList<Trade> GetByCustomerId(int customerId);
    void Add(Trade trade);
    void Remove(int tradeId);
    void Remove(Func<Trade, bool> predicate);
    
}