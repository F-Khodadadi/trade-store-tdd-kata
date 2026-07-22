namespace TradeStore.Core;

public class Store: ITradeStore
{
    private readonly List<Trade> _trades; // private as its internal list

    public Store(IEnumerable<Trade> initialData)
    {
        ArgumentNullException.ThrowIfNull(initialData, nameof(initialData)); //prevents new store(new)
        _trades = initialData.ToList(); //Tolist() means owns its own copy and safer
    }

    public IReadOnlyList<Trade> GetByCustomerId(int customerId)
    {
        return _trades.Where(t => t.Customer.CustomerId == customerId).ToList();
    }
    
    public void Add(Trade trade)
    {
        ArgumentNullException.ThrowIfNull(trade, nameof(trade));
        _trades.Add(trade);
    }
    
    public void Remove(int tradeId)
    {
        var tradeToRemove = _trades.FirstOrDefault(t => t.TradeId == tradeId);
        if (tradeToRemove != null)
        {
            _trades.Remove(tradeToRemove);
        }
    }

    public void Remove(Func<Trade, bool> predicate)
    {
        _trades.RemoveAll(t => predicate(t));
        // or _trades.RemoveAll(predicate.Invoke); 
    }
}