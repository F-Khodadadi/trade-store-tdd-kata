namespace TradeStore.Core;

public class Store
{
    private readonly List<Trade> _trades; // private as its internal list

    public Store(IEnumerable<Trade> initialData)
    {
        _trades = initialData.ToList(); //Tolist() means owns its own copy and safer
    }

    public IReadOnlyList<Trade> GetByCustomerId(int customerId)
    {
        return _trades.Where(t => t.CustomerId == customerId).ToList();
    }
}