# ODIN Market Feed SDK

[![NuGet](https://img.shields.io/nuget/v/ODINMarketFeed.SDK.svg)](https://www.nuget.org/packages/ODINMarketFeed.SDK)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%204.8-blue)](https://dotnet.microsoft.com/)

A robust and efficient .NET SDK for connecting to ODIN Market Data Feed via WebSocket with built-in compression support. This SDK enables real-time market data streaming for trading applications.

## Features

- ✅ **Multi-Framework Support**: Compatible with .NET 8.0 and .NET Framework 4.8
- 🚀 **Real-time Data**: WebSocket-based streaming for low-latency market data
- 🗜️ **Built-in Compression**: ZLIB compression support for optimized bandwidth usage
- 📦 **Message Fragmentation**: Automatic handling of fragmented messages
- 🔄 **Reconnection Support**: Built-in connection management
- 📊 **Touchline Data**: Subscribe to real-time price updates
- ⏸️ **Pause/Resume**: Control data flow as needed
- 🎯 **Type-Safe**: Strongly-typed models for market data

## Installation

### NuGet Package Manager
```bash
Install-Package ODINMarketFeed.SDK
```

### .NET CLI
```bash
dotnet add package ODINMarketFeed.SDK
```

### Package Reference
```xml
<PackageReference Include="ODINMarketFeed.SDK" Version="1.0.0" />
```

## Quick Start

### Basic Usage

```csharp
using PriceFeedAPI;

// Create client instance
var client = new ODINMarketFeedClient();

// Subscribe to events
client.OnOpen += () => 
{
    Console.WriteLine("Connected to ODIN Market Feed");
};

client.OnMessage += (message) => 
{
    Console.WriteLine($"Received: {message}");
};

client.OnError += (error) => 
{
    Console.WriteLine($"Error: {error}");
};

client.OnClose += (code, reason) => 
{
    Console.WriteLine($"Connection closed: {code} - {reason}");
};

// Connect to the feed
await client.ConnectAsync(
    host: "market-feed.example.com",
    port: 8080,
    useSSL: true,
    userId: "your-user-id",
    apiKey: "your-api-key"
);

// Subscribe to market data
var tokens = new List<string> 
{ 
    "1_2885",  // NSE segment, token 2885
    "1_22"     // NSE segment, token 22
};

await client.SubscribeTouchlineAsync(
    tokenList: tokens,
    responseType: "0",  // 0 = Normal touchline, 1 = Fixed length native data
    LTPChangeOnly: false
);

// Keep the application running
Console.ReadLine();

// Cleanup
await client.DisconnectAsync();
client.Dispose();
```

### Advanced Usage

#### Compression Control

```csharp
var client = new ODINMarketFeedClient();

// Disable compression (if needed)
client.SetCompression(false);
```

#### Pause/Resume Subscription

```csharp
// Pause the broadcast (e.g., when app is minimized)
await client.SubscribePauseResumeAsync(isPause: true);

// Resume the broadcast
await client.SubscribePauseResumeAsync(isPause: false);
```

#### Unsubscribe from Tokens

```csharp
var tokensToUnsubscribe = new List<string> { "1_2885", "1_22" };
await client.UnSubscribeTouchlineAsync(tokensToUnsubscribe);
```

## Token Format

Tokens follow the format: `{MarketSegmentID}_{Token}`

**Market Segment IDs:**
- `1` - NSE Cash/Equity
- `2` - NSE Futures & Options
- `3` - NSE Currency
- `4` - BSE Cash/Equity
- `5` - BSE Futures & Options
- `6` - MCX Commodity

**Example:**
- `1_2885` - NSE Cash segment, token 2885
- `2_12345` - NSE F&O segment, token 12345

## Events

### OnOpen
Fired when the WebSocket connection is successfully established.

```csharp
client.OnOpen += () => 
{
    // Connection established
};
```

### OnMessage
Fired when a message is received from the feed.

```csharp
client.OnMessage += (message) => 
{
    // Process market data
    // Message format: "63=FT3.0|64=105|1=1|7=2885|8=12345|..."
};
```

### OnError
Fired when an error occurs.

```csharp
client.OnError += (error) => 
{
    // Handle error
};
```

### OnClose
Fired when the connection is closed.

```csharp
client.OnClose += (code, reason) => 
{
    // Handle disconnection
};
```

## Message Format

Market data messages use a pipe-delimited format:

```
63=FT3.0|64=105|1={MarketSegmentId}|7={Token}|8={LTP}|...
```

**Common Fields:**
- `1` - Market Segment ID
- `7` - Token/Security ID
- `8` - Last Traded Price (LTP)
- `2` - Best Bid Quantity
- `3` - Best Bid Price
- `5` - Best Ask Quantity
- `6` - Best Ask Price
- `73` - Last Trade Time
- `74` - Last Update Time
- `75` - Open Price
- `76` - Close Price
- `77` - High Price
- `78` - Low Price

## Configuration

### Connection Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| host | string | Yes | WebSocket server hostname |
| port | int | Yes | WebSocket server port (1-65535) |
| useSSL | bool | Yes | Use secure WebSocket (wss://) |
| userId | string | Yes | Your user ID |
| apiKey | string | Yes | Your API key |

### Response Types

| Value | Description |
|-------|-------------|
| 0 | Normal touchline (pipe-delimited format) |
| 1 | Fixed length native binary data |

## Sample Projects

Check the [`samples`](./samples) directory for complete working examples:

- **ConsoleApp** - Basic console application demonstrating SDK usage
- **WindowsService** - Example of running as a Windows service
- **AspNetCore** - Integration with ASP.NET Core

## API Reference

### ODINMarketFeedClient Methods

#### ConnectAsync
```csharp
Task ConnectAsync(string host, int port, bool useSSL, string userId, string apiKey)
```
Establishes connection to the ODIN Market Feed.

#### DisconnectAsync
```csharp
Task DisconnectAsync()
```
Gracefully closes the WebSocket connection.

#### SubscribeTouchlineAsync
```csharp
Task SubscribeTouchlineAsync(
    IEnumerable<string> tokenList, 
    string responseType = "0", 
    bool LTPChangeOnly = false
)
```
Subscribes to touchline data for specified tokens.

#### UnSubscribeTouchlineAsync
```csharp
Task UnSubscribeTouchlineAsync(IEnumerable<string> tokenList)
```
Unsubscribes from touchline data for specified tokens.

#### SubscribePauseResumeAsync
```csharp
Task SubscribePauseResumeAsync(bool isPause)
```
Pauses or resumes the market data broadcast.

#### SetCompression
```csharp
void SetCompression(bool enabled)
```
Enables or disables ZLIB compression.

## Requirements

- **.NET 8.0** or **.NET Framework 4.8** or higher
- Active ODIN Market Feed account with valid credentials
- Network connectivity to ODIN Market Feed servers

## Troubleshooting

### Connection Issues

1. **Verify credentials**: Ensure userId and apiKey are correct
2. **Check network**: Verify firewall settings allow WebSocket connections
3. **Port accessibility**: Ensure the specified port is not blocked
4. **SSL certificate**: For SSL connections, ensure valid certificates

### Message Processing

1. **Enable logging**: Add console output for debugging
2. **Check token format**: Verify tokens follow `{segment}_{token}` format
3. **Monitor events**: Subscribe to OnError event for detailed error messages

## Performance Tips

1. **Compression**: Keep compression enabled for production use
2. **Token management**: Unsubscribe from unused tokens to reduce bandwidth
3. **Event handlers**: Keep event handlers lightweight and async
4. **Connection pooling**: Reuse client instances when possible

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For issues, questions, or contributions, please:

- 📫 Open an issue on [GitHub](https://github.com/yourusername/ODINMarketFeedSDK/issues)
- 📧 Contact support at: support@example.com
- 📖 Check the [documentation](./docs)

## Changelog

### Version 1.0.0
- Initial release
- WebSocket connectivity
- ZLIB compression support
- Touchline subscription
- Message fragmentation handling
- Multi-framework support (.NET 8.0 and .NET Framework 4.8)

## Acknowledgments

- Built with ❤️ for the trading community
- Thanks to all contributors

---

**Disclaimer**: This SDK is provided as-is. Always test thoroughly in a development environment before using in production.
