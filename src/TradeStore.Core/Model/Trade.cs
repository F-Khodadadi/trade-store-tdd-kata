namespace TradeStore.Core.Model;

public class Trade
{
    public int TradeId { get; set; }
    public Customer Customer { get; set; } = new Customer();
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime TradeDate { get; set; }
}