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
    
    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenInitialDataIsNull()
    {
        // Arrange
        IEnumerable<Trade> trades = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Store(trades));
    }
    
    [Fact]
    public void Add_AddsTradeToStore()
    {
        // Arrange
        var trades = new List<Trade>
        {
            new Trade {TradeId=1,CustomerId=10,Ammount = 50m, currency ="GBP", TradeDate = DateTime.Now},
            new Trade {TradeId=2,CustomerId=10,Ammount = 100m, currency ="USD", TradeDate = DateTime.Now},
        };
        
        var store = new Store(trades);
        
        var newTrade = new Trade {TradeId=3,CustomerId=20,Ammount = 200m, currency ="EUR", TradeDate = DateTime.Now};
        
        // Act
        store.Add(newTrade);
        
        // Assert
        var result = store.GetByCustomerId(20);
        Assert.Single(result);
        Assert.Equal(newTrade.TradeId, result[0].TradeId);
    }
    
    [Fact]
    public void Remove_RemovesTradeByTradeId()
    {
        // Arrange
        var trades = new List<Trade>
        {
            new Trade { TradeId = 1, CustomerId = 10, Ammount = 50m, currency = "GBP", TradeDate = DateTime.Now },
            new Trade { TradeId = 2, CustomerId = 10, Ammount = 100m, currency = "USD", TradeDate = DateTime.Now },
            new Trade { TradeId = 3, CustomerId = 20, Ammount = 200m, currency = "EUR", TradeDate = DateTime.Now }
        };

        var store = new Store(trades);

        // Act
        store.Remove(2);

        // Assert
        var result = store.GetByCustomerId(10);

        Assert.Single(result);
        Assert.Equal(1, result[0].TradeId);
    }
    
    [Fact]
    public void Remove_WhenTradeDoesNotExist_DoesNothing()
    {
        // Arrange
        var trades = new List<Trade>
        {
            new Trade { TradeId = 1, CustomerId = 10, Ammount = 50m, currency = "GBP", TradeDate = DateTime.Now }
        };

        var store = new Store(trades);

        // Act
        store.Remove(999);

        // Assert
        var result = store.GetByCustomerId(10);

        Assert.Single(result);
    }
}