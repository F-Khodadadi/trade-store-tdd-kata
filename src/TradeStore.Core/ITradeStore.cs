namespace TradeStore.Core;

public interface ITradeStore
{
    IReadOnlyList<Trade> GetByCustomerId(int customerId);
}