using TradeStore.Core;
using TradeStore.Core.Model;

namespace TradeStore.Tests;

public class TradeStoreTests
{
    private static readonly DateTime TradeDate =
        new(2026, 7, 22, 10, 0, 0, DateTimeKind.Utc);
    [Fact]
    public void GetByCustomerId_ReturnsOnlyTradesForSpecificCustomer()
    {
        // Arrange
        var store = new Core.TradeStore(CreateTrades());
        
        //Act
        var result = store.GetByCustomerId(10);
        
        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal([1, 2], result.Select(trade => trade.TradeId));
    }
    
    [Fact]
    public void GetByCustomerId_ReturnsEmptyListWhenNoTradesForCustomer()
    {
        // Arrange
        var store = new Core.TradeStore(CreateTrades());
        
        //Act
        var result = store.GetByCustomerId(999);
        
        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenInitialDataIsNull()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Core.TradeStore(null!));
    }
    
    [Fact]
    public void Add_AddsTradeToStore()
    {
        // Arrange
        var store = new Core.TradeStore(CreateTrades());
        
        var newTrade = new Trade {TradeId=999,Customer = new Customer { CustomerId = 99, Name = "Jane Doe", Email = "" }, Amount = 200m, Currency ="EUR", TradeDate = TradeDate};
        
        // Act
        store.Add(newTrade);
        
        // Assert
        var result = store.GetByCustomerId(99);
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
            TradeDate = TradeDate
        };

        var store = new Core.TradeStore(new List<Trade> { existingTrade });

        var duplicateTrade = new Trade
        {
            TradeId = 1,
            Customer = new Customer { CustomerId = 20, Name = "Jane Doe", Email = "" },
            Amount = 100m,
            Currency = "USD",
            TradeDate = TradeDate
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => store.Add(duplicateTrade));
    }
    
    [Fact]
    public void Remove_RemovesTradeByTradeId()
    {
        var store = new Core.TradeStore(CreateTrades());

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
            new Trade { TradeId = 1, Customer = new Customer { CustomerId = 10, Name = "John Doe", Email = "" }, Amount = 50m, Currency = "GBP", TradeDate = TradeDate }
        };

        var store = new Core.TradeStore(trades);

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
        var store = new Core.TradeStore(CreateTrades());

        // Act
        store.Remove(t => t.Customer.CustomerId == 10);

        // Assert
        var result = store.GetByCustomerId(10);

        Assert.Empty(result);
    }
    
    [Fact]
    public void Remove_ThrowsArgumentNullException_WhenPredicateIsNull()
    {
        var store = new Core.TradeStore(CreateTrades());

        var exception = Assert.Throws<ArgumentNullException>(
            () => store.Remove((Func<Trade, bool>)null!));

        Assert.Equal("predicate", exception.ParamName);
    }
    
    private static List<Trade> CreateTrades()
    {
        return
        [
            CreateTrade(1, 10, 50m, "GBP"),
            CreateTrade(2, 10, 100m, "USD"),
            CreateTrade(3, 20, 200m, "EUR")
        ];
    }

    private static Trade CreateTrade(
        int tradeId,
        int customerId,
        decimal amount,
        string currency)
    {
        return new Trade
        {
            TradeId = tradeId,
            Customer = new Customer { CustomerId = customerId, Name = $"Customer {customerId}", Email = $"customer{customerId}@example.com" },
            Amount = amount,
            Currency = currency,
            TradeDate = new DateTime(
                2026, 7, 22, 10, 0, 0, DateTimeKind.Utc)
        };
    }
    
    
}