# Trade Store

A small C# project that manages trades in memory using `List<Trade>`, LINQ, delegates and xUnit.

This project was created as a personal learning exercise after a technical interview to reinforce collection manipulation, test-driven development (TDD), and clean object-oriented design.

The goal was to keep the implementation deliberately simple rather than introduce databases, APIs or unnecessary architectural layers.

---

## Project Goals

- Practice TDD using small red-green-refactor cycles
- Build clean, readable C# code
- Demonstrate collection manipulation using `List<T>`
- Practice LINQ and delegates
- Write maintainable unit tests

---

## Current Features

The store currently supports:

- Initialising a store with an existing collection of trades
- Retrieving trades by customer ID
- Returning an empty collection when no trades match
- Adding new trades
- Preventing duplicate `TradeId` values
- Removing a trade by its ID
- Removing trades using `Func<Trade, bool>`
- Guarding against invalid null arguments

---

## Solution Structure

```text
trade-store-tdd-kata
├── src
│   └── TradeStore.Core
│       ├── ITradeStore.cs
│       ├── Store.cs
│       ├── TradeStore.Core.csproj
│       └── Model
│           ├── Trade.cs
│           └── Customer.cs
│
├── tests
│   └── TradeStore.Tests
│       ├── TradeStoreTests.cs
│       └── TradeStore.Tests.csproj
│
├── TradeStore.sln
└── README.md
```

---

## Domain Model

### Trade

Represents a single trade.

Properties include:

- TradeId
- CustomerId
- Customer
- Amount
- Currency
- TradeDate

### Customer

Represents the customer associated with a trade.

Properties include:

- CustomerId
- Name
- Email

The `Trade` model contains both a `CustomerId` and a navigation property:

```csharp
public virtual Customer? Customer { get; set; }
```

This mirrors a common domain model where a trade references its related customer object.

---

## Store

`Store` is an in-memory implementation of `ITradeStore`.

It owns a private collection:

```csharp
private readonly List<Trade> _trades;
```

The constructor copies the supplied collection:

```csharp
_trades = initialData.ToList();
```

instead of storing the caller's original list.

This ensures the store owns its own state and cannot be modified accidentally from outside the class.

---

## Design Decisions

### Why List instead of Dictionary?

The original exercise focused on collection manipulation and LINQ.

A `List<Trade>` keeps the implementation simple and demonstrates filtering, searching and removing items using standard collection operations.

Although a `Dictionary<int, Trade>` would provide faster lookups by trade ID, it would complicate filtering by customer, currency or date and change the focus of the exercise.

---

### Why use `Func<Trade, bool>`?

Instead of creating many removal methods such as:

- RemoveByCustomer()
- RemoveByCurrency()
- RemoveByTradeDate()

the store exposes a single flexible method:

```csharp
Remove(Func<Trade, bool> predicate)
```

This allows callers to define any removal criteria.

Examples:

```csharp
store.Remove(t => t.CustomerId == 10);
```

```csharp
store.Remove(t => t.Currency == "GBP");
```

```csharp
store.Remove(t => t.TradeDate < DateTime.Today);
```

Using delegates keeps the API small while making it significantly more flexible.

---

### Why validate duplicate Trade IDs?

`TradeId` is treated as the unique identifier for a trade.

Before inserting a trade, the store checks whether the identifier already exists and throws an exception if a duplicate is detected.

This protects the integrity of the collection.

---

## Test-Driven Development

The project was developed using small TDD cycles.

For each behaviour:

1. Write a failing test
2. Implement the smallest change required to pass
3. Refactor while keeping all tests green
4. Commit the completed behaviour

Each feature was added independently to produce a clean commit history and make the evolution of the solution easy to follow.

---

## Running the Tests

```bash
dotnet restore
dotnet test
```

---

## Future Improvements

Possible extensions include:

- Thread-safe implementation
- Aggregate queries
- Pagination
- Async persistence
- Performance comparison between `List` and `Dictionary`
- Python implementation of the same exercise