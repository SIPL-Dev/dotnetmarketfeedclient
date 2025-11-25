# ODIN Market Feed SDK - API Documentation

## Table of Contents

1. [Getting Started](#getting-started)
2. [Client Initialization](#client-initialization)
3. [Connection Management](#connection-management)
4. [Event Handlers](#event-handlers)
5. [Market Data Subscription](#market-data-subscription)
6. [Message Format](#message-format)
7. [Error Handling](#error-handling)
8. [Best Practices](#best-practices)

## Getting Started

### Installation

Install the NuGet package:

```bash
dotnet add package ODINMarketFeed.SDK
```

### Basic Usage

```csharp
using PriceFeedAPI;

var client = new ODINMarketFeedClient();
await client.ConnectAsync("host", 8080, true, "userId", "apiKey");
```

## Client Initialization

### Constructor

```csharp
public ODINMarketFeedClient()
```

Creates a new instance of the ODIN Market Feed client.

**Example:**
```csharp
var client = new ODINMarketFeedClient();
```

### SetCompression

```csharp
public void SetCompression(bool enabled)
```

Enables or disables ZLIB compression for data transmission.

**Parameters:**
- `enabled` (bool): True to enable compression, false to disable

**Default:** Compression is enabled by default

**Example:**
```csharp
client.SetCompression(false); // Disable compression
```

## Connection Management

### ConnectAsync

```csharp
public async Task ConnectAsync(
    string host, 
    int port, 
    bool useSSL, 
    string userId, 
    string apiKey
)
```

Establishes a WebSocket connection to the ODIN Market Feed server.

**Parameters:**
- `host` (string): The WebSocket server hostname or IP address
- `port` (int): The WebSocket server port (1-65535)
- `useSSL` (bool): Whether to use secure WebSocket (wss://)
- `userId` (string): Your user ID for authentication
- `apiKey` (string): Your API key for authentication

**Throws:**
- `ArgumentException`: If parameters are invalid
- `WebSocketException`: If connection fails

**Example:**
```csharp
await client.ConnectAsync(
    host: "market-feed.example.com",
    port: 8080,
    useSSL: true,
    userId: "USER123",
    apiKey: "your-api-key-here"
);
```

### DisconnectAsync

```csharp
public async Task DisconnectAsync()
```

Gracefully closes the WebSocket connection.

**Example:**
```csharp
await client.DisconnectAsync();
```

### Dispose

```csharp
public void Dispose()
```

Releases all resources used by the client. Implements IDisposable.

**Example:**
```csharp
using (var client = new ODINMarketFeedClient())
{
    // Use client
}
// Automatically disposed
```

## Event Handlers

### OnOpen

```csharp
public event Action OnOpen
```

Fired when the WebSocket connection is successfully established.

**Example:**
```csharp
client.OnOpen += () => 
{
    Console.WriteLine("Connected!");
};
```

### OnMessage

```csharp
public event Action<object> OnMessage
```

Fired when a message is received from the server.

**Parameters:**
- `message` (object): The received message (typically a string)

**Example:**
```csharp
client.OnMessage += (message) => 
{
    var msg = message.ToString();
    Console.WriteLine($"Received: {msg}");
    
    // Parse the message
    var fields = ParseMessage(msg);
};
```

### OnError

```csharp
public event Action<string> OnError
```

Fired when an error occurs.

**Parameters:**
- `error` (string): Error message describing what went wrong

**Example:**
```csharp
client.OnError += (error) => 
{
    Console.WriteLine($"Error: {error}");
    // Log error or handle reconnection
};
```

### OnClose

```csharp
public event Action<int, string> OnClose
```

Fired when the connection is closed.

**Parameters:**
- `code` (int): WebSocket close status code
- `reason` (string): Reason for closure

**Example:**
```csharp
client.OnClose += (code, reason) => 
{
    Console.WriteLine($"Disconnected: {code} - {reason}");
    // Handle reconnection logic
};
```

## Market Data Subscription

### SubscribeTouchlineAsync

```csharp
public async Task SubscribeTouchlineAsync(
    IEnumerable<string> tokenList,
    string responseType = "0",
    bool LTPChangeOnly = false
)
```

Subscribes to touchline (tick-by-tick) market data for specified tokens.

**Parameters:**
- `tokenList` (IEnumerable<string>): List of tokens in format "MarketSegmentID_Token"
- `responseType` (string, optional): "0" for normal format, "1" for fixed-length binary format
- `LTPChangeOnly` (bool, optional): If true, only sends updates when LTP changes

**Token Format:** `{MarketSegmentID}_{Token}`
- Example: "1_2885" (NSE Cash, token 2885)

**Market Segment IDs:**
- 1 = NSE Cash/Equity
- 2 = NSE F&O
- 3 = NSE Currency
- 4 = BSE Cash
- 5 = BSE F&O
- 6 = MCX

**Example:**
```csharp
var tokens = new List<string> 
{ 
    "1_2885",  // NSE Equity - Reliance
    "1_22",    // NSE Equity - ACC
    "2_12345"  // NSE F&O
};

await client.SubscribeTouchlineAsync(
    tokenList: tokens,
    responseType: "0",
    LTPChangeOnly: false
);
```

### UnSubscribeTouchlineAsync

```csharp
public async Task UnSubscribeTouchlineAsync(IEnumerable<string> tokenList)
```

Unsubscribes from touchline data for specified tokens.

**Parameters:**
- `tokenList` (IEnumerable<string>): List of tokens to unsubscribe

**Example:**
```csharp
var tokensToRemove = new List<string> { "1_2885", "1_22" };
await client.UnSubscribeTouchlineAsync(tokensToRemove);
```

### SubscribePauseResumeAsync

```csharp
public async Task SubscribePauseResumeAsync(bool isPause)
```

Pauses or resumes the market data broadcast without unsubscribing.

**Parameters:**
- `isPause` (bool): True to pause, false to resume

**Use Case:** Useful when the application is minimized or data is temporarily not needed.

**Example:**
```csharp
// Pause when app is minimized
await client.SubscribePauseResumeAsync(isPause: true);

// Resume when app is restored
await client.SubscribePauseResumeAsync(isPause: false);
```

## Message Format

Messages from the server use a pipe-delimited format:

```
63=FT3.0|64=105|1=1|7=2885|8=12345|...
```

### Common Field Tags

| Tag | Description | Example |
|-----|-------------|---------|
| 1 | Market Segment ID | 1 |
| 7 | Token/Security ID | 2885 |
| 8 | Last Traded Price (LTP) | 12345 |
| 2 | Best Bid Quantity | 100 |
| 3 | Best Bid Price | 12340 |
| 5 | Best Ask Quantity | 150 |
| 6 | Best Ask Price | 12350 |
| 73 | Last Trade Time | 2024-11-24 143020 |
| 74 | Last Update Time | 2024-11-24 143021 |
| 75 | Open Price | 12300 |
| 76 | Close Price | 12400 |
| 77 | High Price | 12450 |
| 78 | Low Price | 12250 |
| 399 | Decimal Locator | 2 |

### Parsing Messages

```csharp
private Dictionary<string, string> ParseMessage(string message)
{
    var fields = new Dictionary<string, string>();
    var parts = message.Split('|');
    
    foreach (var part in parts)
    {
        var keyValue = part.Split('=');
        if (keyValue.Length == 2)
        {
            fields[keyValue[0]] = keyValue[1];
        }
    }
    
    return fields;
}

// Usage
client.OnMessage += (message) => 
{
    var fields = ParseMessage(message.ToString());
    
    if (fields.ContainsKey("8")) // LTP
    {
        var ltp = fields["8"];
        var decimalLocator = int.Parse(fields["399"]);
        var actualLTP = decimal.Parse(ltp) / (decimal)Math.Pow(10, decimalLocator);
        
        Console.WriteLine($"LTP: {actualLTP}");
    }
};
```

## Error Handling

### Common Errors and Solutions

#### Connection Errors

```csharp
client.OnError += (error) => 
{
    if (error.Contains("Connection failed"))
    {
        // Retry connection with exponential backoff
        await RetryConnection();
    }
};
```

#### Invalid Token Format

```csharp
var isValid = ValidateToken("1_2885");

bool ValidateToken(string token)
{
    var parts = token.Split('_');
    return parts.Length == 2 
        && int.TryParse(parts[0], out _) 
        && int.TryParse(parts[1], out _);
}
```

#### WebSocket State Errors

```csharp
try
{
    await client.SubscribeTouchlineAsync(tokens);
}
catch (InvalidOperationException ex)
{
    // WebSocket is not connected
    Console.WriteLine("Not connected. Please connect first.");
}
```

## Best Practices

### 1. Connection Management

```csharp
// Use connection monitoring
var isConnected = false;

client.OnOpen += () => isConnected = true;
client.OnClose += (code, reason) => 
{
    isConnected = false;
    // Implement reconnection logic
};

// Check connection before operations
if (!isConnected)
{
    await client.ConnectAsync(...);
}
```

### 2. Resource Management

```csharp
// Always dispose of the client
using (var client = new ODINMarketFeedClient())
{
    await client.ConnectAsync(...);
    // Use client
} // Automatically disposed
```

### 3. Subscription Management

```csharp
// Keep track of subscribed tokens
private HashSet<string> subscribedTokens = new HashSet<string>();

async Task SubscribeAsync(IEnumerable<string> tokens)
{
    var newTokens = tokens.Where(t => !subscribedTokens.Contains(t));
    
    if (newTokens.Any())
    {
        await client.SubscribeTouchlineAsync(newTokens);
        subscribedTokens.UnionWith(newTokens);
    }
}
```

### 4. Error Recovery

```csharp
private async Task RetryConnection(int maxRetries = 3)
{
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            await client.ConnectAsync(...);
            return; // Success
        }
        catch (Exception ex)
        {
            if (i == maxRetries - 1) throw;
            
            // Exponential backoff
            await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, i)));
        }
    }
}
```

### 5. Performance Optimization

```csharp
// Batch subscribe tokens
const int BATCH_SIZE = 50;

var tokenBatches = tokens
    .Select((token, index) => new { token, index })
    .GroupBy(x => x.index / BATCH_SIZE)
    .Select(g => g.Select(x => x.token));

foreach (var batch in tokenBatches)
{
    await client.SubscribeTouchlineAsync(batch);
    await Task.Delay(100); // Small delay between batches
}
```

## Thread Safety

The client is thread-safe for basic operations, but for high-frequency updates:

```csharp
private readonly object _lockObject = new object();
private Dictionary<string, decimal> _latestPrices = new Dictionary<string, decimal>();

client.OnMessage += (message) => 
{
    var fields = ParseMessage(message.ToString());
    var token = $"{fields["1"]}_{fields["7"]}";
    var ltp = decimal.Parse(fields["8"]);
    
    lock (_lockObject)
    {
        _latestPrices[token] = ltp;
    }
};
```

## Additional Resources

- [Sample Projects](../samples/)
- [Contributing Guide](../CONTRIBUTING.md)
- [Changelog](../CHANGELOG.md)
- [GitHub Issues](https://github.com/yourusername/ODINMarketFeedSDK/issues)

---

For more information or support, please visit our [GitHub repository](https://github.com/yourusername/ODINMarketFeedSDK).
