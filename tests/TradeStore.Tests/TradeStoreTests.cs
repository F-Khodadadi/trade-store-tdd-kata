using TradeStore.Core;

namespace TradeStore.Tests;

public class TradeStoreTests
{
    [Fact]
    public void GetByCustomerId_ReturnsOnlyTradesForSpecificCustomer()
    {
        // Arrange
        var trades = new[]
        {
            new Trade {TradeId=1,CustomerId=10,Ammount = 50m, currency ="GBP", TradeDate = DateTime.Now},
            new Trade {TradeId=2,CustomerId=10,Ammount = 100m, currency ="USD", TradeDate = DateTime.Now},
            new Trade {TradeId=3,CustomerId=20,Ammount = 200m, currency ="EUR", TradeDate = DateTime.Now},
        };
        
        var store = new Store(trades);
        
        //Act
        var result = store.GetByCustomerId(10);
        
        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, trade => Assert.Equal(10, trade.CustomerId));
    }
    
    [Fact]
    public void GetByCustomerId_ReturnsEmptyListWhenNoTradesForCustomer()
    {
        // Arrange
        var trades = new[]
        {
            new Trade {TradeId=1,CustomerId=10,Ammount = 50m, currency ="GBP", TradeDate = DateTime.Now},
            new Trade {TradeId=2,CustomerId=10,Ammount = 100m, currency ="USD", TradeDate = DateTime.Now},
            new Trade {TradeId=3,CustomerId=20,Ammount = 200m, currency ="EUR", TradeDate = DateTime.Now},
        };
        
        var store = new Store(trades);
        
        //Act
        var result = store.GetByCustomerId(999);
        
        // Assert
        Assert.Empty(result);
    }
}