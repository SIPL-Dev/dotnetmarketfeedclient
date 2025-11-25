# Quick Start Guide - ODIN Market Feed SDK

Get up and running with ODIN Market Feed SDK in 5 minutes!

## Installation

```bash
dotnet add package ODINMarketFeed.SDK
```

## Basic Implementation

### Step 1: Create the Client

```csharp
using PriceFeedAPI;

var client = new ODINMarketFeedClient();
```

### Step 2: Set Up Event Handlers

```csharp
// Connection opened
client.OnOpen += () => 
{
    Console.WriteLine("✓ Connected to ODIN Market Feed");
};

// Market data received
client.OnMessage += (message) => 
{
    Console.WriteLine($"📊 Data: {message}");
};

// Error occurred
client.OnError += (error) => 
{
    Console.WriteLine($"❌ Error: {error}");
};

// Connection closed
client.OnClose += (code, reason) => 
{
    Console.WriteLine($"🔌 Disconnected: {reason}");
};
```

### Step 3: Connect to Server

```csharp
await client.ConnectAsync(
    host: "your-feed-server.com",
    port: 8080,
    useSSL: true,
    userId: "YOUR_USER_ID",
    apiKey: "YOUR_API_KEY"
);
```

### Step 4: Subscribe to Market Data

```csharp
var tokens = new List<string> 
{ 
    "1_2885",  // NSE segment, token 2885
    "1_22"     // NSE segment, token 22
};

await client.SubscribeTouchlineAsync(tokens);
```

### Step 5: Handle Data

```csharp
client.OnMessage += (message) => 
{
    var msg = message.ToString();
    var fields = msg.Split('|')
        .Select(f => f.Split('='))
        .Where(f => f.Length == 2)
        .ToDictionary(f => f[0], f => f[1]);
    
    if (fields.ContainsKey("8")) // LTP
    {
        Console.WriteLine($"Last Price: {fields["8"]}");
    }
};
```

### Step 6: Clean Up

```csharp
// When done
await client.DisconnectAsync();
client.Dispose();
```

## Complete Example

```csharp
using PriceFeedAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using var client = new ODINMarketFeedClient();
        
        // Events
        client.OnOpen += () => Console.WriteLine("Connected!");
        client.OnMessage += (msg) => Console.WriteLine($"Data: {msg}");
        client.OnError += (err) => Console.WriteLine($"Error: {err}");
        
        // Connect
        await client.ConnectAsync(
            "feed.example.com", 8080, true, 
            "USER_ID", "API_KEY"
        );
        
        // Subscribe
        await client.SubscribeTouchlineAsync(
            new List<string> { "1_2885", "1_22" }
        );
        
        // Keep running
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        
        // Cleanup
        await client.DisconnectAsync();
    }
}
```

## Token Format

Tokens use the format: `{MarketSegmentID}_{Token}`

**Common Segments:**
- `1` = NSE Cash/Equity
- `2` = NSE F&O
- `3` = NSE Currency
- `4` = BSE Cash
- `5` = BSE F&O
- `6` = MCX

## Common Operations

### Pause/Resume Subscription

```csharp
// Pause
await client.SubscribePauseResumeAsync(isPause: true);

// Resume
await client.SubscribePauseResumeAsync(isPause: false);
```

### Unsubscribe from Tokens

```csharp
await client.UnSubscribeTouchlineAsync(new List<string> { "1_2885" });
```

### Disable Compression

```csharp
client.SetCompression(false);
```

## Message Fields Reference

| Tag | Field | Example |
|-----|-------|---------|
| 1 | Market Segment | 1 |
| 7 | Token | 2885 |
| 8 | Last Traded Price | 12345 |
| 3 | Bid Price | 12340 |
| 6 | Ask Price | 12350 |

## Troubleshooting

### Connection Failed
- Verify host and port
- Check credentials (userId, apiKey)
- Ensure firewall allows WebSocket connections

### No Data Received
- Verify tokens are correctly formatted
- Check if subscription was successful
- Ensure connection is established (OnOpen event fired)

### Invalid Token Format
- Use format: `MarketSegmentID_Token`
- Example: `1_2885` (NOT `2885` or `1-2885`)

## Next Steps

- 📖 Read the [Full API Documentation](./API.md)
- 💻 Check out [Sample Projects](../samples/)
- 🤝 Learn [Best Practices](./API.md#best-practices)
- 🐛 Report issues on [GitHub](https://github.com/yourusername/ODINMarketFeedSDK/issues)

## Need Help?

- 📫 GitHub Issues: https://github.com/yourusername/ODINMarketFeedSDK/issues
- 📧 Email: support@example.com
- 📚 Full Documentation: [docs/API.md](./API.md)

---

Happy Trading! 📈
