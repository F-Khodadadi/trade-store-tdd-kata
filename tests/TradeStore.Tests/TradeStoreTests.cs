using TradeStore.Core;
using TradeStore.Core.Model;

namespace TradeStore.Tests;

public class TradeStoreTests
{
    [Fact]
    public void GetByCustomerId_ReturnsOnlyTradesForSpecificCustomer()
    {
        // Arrange
        var trades = new[]
        {
            new Trade {TradeId=1,Customer = new Customer { CustomerId = 10, Name = "John Doe", Email = "" }, Amount = 50m, Currency ="GBP", TradeDate = DateTime.Now},
            new Trade {TradeId=2,Customer = new Customer { CustomerId = 10, Name = "John Doe", Email = "" }, Amount = 100m, Currency ="USD", TradeDate = DateTime.Now},
            new Trade {TradeId=3,Customer = new Customer { CustomerId = 20, Name = "Jane Doe", Email = "" }, Amount = 200m, Currency ="EUR", TradeDate = DateTime.Now},
        };
        
        var store = new Store(trades);
        
        //Act
        var result = store.GetByCustomerId(10);
        
        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, trade => Assert.Equal(10, trade.Customer.CustomerId));
    }
    
    [Fact]
    public void GetByCustomerId_ReturnsEmptyListWhenNoTradesForCustomer()
    {
        // Arrange
        var trades = new[]
        {
            new Trade {TradeId=1,Customer = new Customer { CustomerId = 10, Name = "John Doe", Email = "" }, Amount = 50m, Currency ="GBP", TradeDate = DateTime.Now},
            new Trade {TradeId=2,Customer = new Customer { CustomerId = 10, Name = "John Doe", Email = "" }, Amount = 100m, Currency ="USD", TradeDate = DateTime.Now},
            new Trade {TradeId=3,Customer = new Customer { CustomerId = 20, Name = "Jane Doe", Email = "" }, Amount = 200m, Currency ="EUR", TradeDate = DateTime.Now},
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
            new Trade {TradeId=1,Customer = new Customer { CustomerId = 10, Name = "John Doe", Email = "" }, Amount = 50m, Currency ="GBP", TradeDate = DateTime.Now},
            new Trade {TradeId=2,Customer = new Customer { CustomerId = 10, Name = "John Doe", Email = "" }, Amount = 100m, Currency ="USD", TradeDate = DateTime.Now},    
        };
        
        var store = new Store(trades);
        
        var newTrade = new Trade {TradeId=3,Customer = new Customer { CustomerId = 20, Name = "Jane Doe", Email = "" }, Amount = 200m, Currency ="EUR", TradeDate = DateTime.Now};
        
        // Act
        store.Add(newTrade);
        
        // Assert
        var result = store.GetByCustomerId(20);
        Assert.Single(result);
        Assert.Equal(newTrade.TradeId, result[0].TradeId);
    }

    [Fact]
    public void Add_ThrowsInvalidOperationException_WhenTradeIdAlreadyExists()
    {
        // Arrange
        var existingTrade = new Trade
        {
            TradeId = 1,
            Customer = new Customer { CustomerId = 10, Name = "John Doe", Email = "" },
            Amount = 50m,
            Currency = "GBP",
            TradeDate = DateTime.Now
        };

        var store = new Store(new List<Trade> { existingTrade });

        var duplicateTrade = new Trade
        {
            TradeId = 1,
            Customer = new Customer { CustomerId = 20, Name = "Jane Doe", Email = "" },
            Amount = 100m,
            Currency = "USD",
            TradeDate = DateTime.Now
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => store.Add(duplicateTrade));
    }
    
    [Fact]
    public void Remove_RemovesTradeByTradeId()
    {
        // Arrange
        var trades = new List<Trade>
        {
            new Trade { TradeId = 1, Customer = new Customer { CustomerId = 10, Name = "John Doe", Email = "" }, Amount = 50m, Currency = "GBP", TradeDate = DateTime.Now },
            new Trade { TradeId = 2, Customer = new Customer { CustomerId = 10, Name = "John Doe", Email = "" }, Amount = 100m, Currency = "USD", TradeDate = DateTime.Now },
            new Trade { TradeId = 3, Customer = new Customer { CustomerId = 20, Name = "Jane Doe", Email = "" }, Amount = 200m, Currency = "EUR", TradeDate = DateTime.Now }
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
            new Trade { TradeId = 1, Customer = new Customer { CustomerId = 10, Name = "John Doe", Email = "" }, Amount = 50m, Currency = "GBP", TradeDate = DateTime.Now }
        };

        var store = new Store(trades);

        // Act
        store.Remove(999);

        // Assert
        var result = store.GetByCustomerId(10);

        Assert.Single(result);
    }

    [Fact]
    public void Remove_WithPredicate_RemovesMatchingTrades()
    {
        //Arrange
        var customer = new Customer { CustomerId = 10, Name = "John Doe", Email = ""};
        
        var trades = new List<Trade>
        {
            new Trade { TradeId = 1, Customer = customer, Amount = 50m, Currency = "GBP", TradeDate = DateTime.Now },
            new Trade { TradeId = 2, Customer = customer, Amount = 100m, Currency = "USD", TradeDate = DateTime.Now },
            new Trade { TradeId = 3, Customer = new Customer { CustomerId = 20, Name = "Jane Doe", Email = "" }, Amount = 200m, Currency = "EUR", TradeDate = DateTime.Now }
        };

        var store = new Store(trades);

        // Act
        store.Remove(t => t.Customer.CustomerId == 10);

        // Assert
        var result = store.GetByCustomerId(10);

        Assert.Empty(result);
    }
    
    
}