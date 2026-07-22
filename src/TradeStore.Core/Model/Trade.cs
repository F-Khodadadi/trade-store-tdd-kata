namespace TradeStore.Core;

public class Trade
{
    public int TradeId { get; set; }
    public int CustomerId { get; set; }
    public decimal Ammount { get; set; }
    public string currency { get; set; } = string.Empty;
    public DateTime TradeDate { get; set; }
}