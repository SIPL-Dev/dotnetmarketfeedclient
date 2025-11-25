# Project Structure

```
ODINMarketFeedSDK/
│
├── .github/
│   └── workflows/
│       └── dotnet.yml                    # CI/CD pipeline for automated builds
│
├── src/
│   └── ODINMarketFeed.SDK/
│       ├── Compression/
│       │   ├── ZLib/
│       │   │   ├── zlibConst.cs         # ZLib constants
│       │   │   └── ZStream.cs           # ZLib stream wrapper
│       │   └── ZLIBCompressor.cs        # Compression implementation
│       ├── ODINMarketFeedClient.cs      # Main client class
│       ├── FragmentationHandler.cs      # Message fragmentation handler
│       └── ODINMarketFeed.SDK.csproj    # Project file with multi-targeting
│
├── samples/
│   └── ConsoleApp/
│       ├── Program.cs                    # Console application example
│       └── ConsoleApp.csproj            # Sample project file
│
├── tests/                                # Unit tests (to be added)
│
├── docs/
│   ├── API.md                           # Complete API documentation
│   ├── QUICKSTART.md                    # Quick start guide
│   ├── PUBLISHING.md                    # Publishing to GitHub/NuGet guide
│   └── ICON.md                          # Package icon instructions
│
├── ODINMarketFeed.sln                   # Visual Studio solution file
├── README.md                            # Main documentation
├── LICENSE                              # MIT License
├── CHANGELOG.md                         # Version history
├── CONTRIBUTING.md                      # Contribution guidelines
├── .gitignore                           # Git ignore patterns
└── icon.png                             # Package icon (to be created)
```

## Key Files Description

### Source Files (`src/`)

**ODINMarketFeedClient.cs**
- Main SDK entry point
- WebSocket connection management
- Event-driven architecture
- Market data subscription methods

**FragmentationHandler.cs**
- Handles message fragmentation/defragmentation
- Manages partial message buffering
- Ensures complete message delivery

**ZLIBCompressor.cs**
- Implements ZLIB compression/decompression
- Optimizes bandwidth usage
- Handles binary data efficiently

**Compression/ZLib/***
- ZLib library implementation
- Stream handling for compression
- Constants and utilities

### Project Configuration

**ODINMarketFeed.SDK.csproj**
```xml
Key Features:
- Multi-targeting: net8.0 and net48
- NuGet package metadata
- Documentation generation
- Source Link support
```

### Samples

**samples/ConsoleApp/**
- Working example of SDK usage
- Demonstrates all major features
- Shows best practices
- Interactive console interface

### Documentation

**README.md**
- Overview and features
- Installation instructions
- Basic usage examples
- API reference summary

**docs/API.md**
- Detailed API documentation
- All methods and properties
- Code examples
- Best practices

**docs/QUICKSTART.md**
- 5-minute getting started guide
- Minimal viable implementation
- Common operations

**docs/PUBLISHING.md**
- Step-by-step publishing guide
- GitHub setup instructions
- NuGet publishing process
- Maintenance guidelines

## Build Outputs

When built, the project creates:

```
bin/
├── Debug/
│   ├── net8.0/
│   │   ├── ODINMarketFeed.SDK.dll
│   │   ├── ODINMarketFeed.SDK.xml
│   │   └── ...
│   └── net48/
│       ├── ODINMarketFeed.SDK.dll
│       ├── ODINMarketFeed.SDK.xml
│       └── ...
└── Release/
    └── ... (same structure)
```

## NuGet Package Structure

When packed, creates:

```
ODINMarketFeed.SDK.1.0.0.nupkg
├── lib/
│   ├── net8.0/
│   │   └── ODINMarketFeed.SDK.dll
│   └── net48/
│       └── ODINMarketFeed.SDK.dll
├── icon.png
├── README.md
├── LICENSE
└── [Content_Types].xml
```

## Multi-Framework Support

### .NET 8.0
- Modern async/await patterns
- Latest language features
- Built-in WebSocket support
- High performance

### .NET Framework 4.8
- Legacy application support
- Enterprise environment compatibility
- Windows-only deployment
- Requires System.Net.WebSockets.Client NuGet package

## Dependencies

### .NET 8.0
- No external dependencies (WebSockets built-in)

### .NET Framework 4.8
- System.Net.WebSockets.Client (4.3.2)
- System.Threading.Tasks.Extensions (4.5.4)

### Development Dependencies (All Frameworks)
- Microsoft.SourceLink.GitHub (for debugging)

## Namespace Organization

```
PriceFeedAPI
├── ODINMarketFeedClient              # Main client
├── FragmentationHandler              # Message handling
├── CompressionStatus                 # Enum
├── MarketData                        # Data model
└── ZLIBCompressor                    # Compression

FTIL.Compression.ZLib
├── zlibConst                         # Constants
└── ZStream                           # Stream implementation
```

## Configuration Files

**.gitignore**
- Excludes build artifacts
- Ignores IDE-specific files
- Protects sensitive data

**.github/workflows/dotnet.yml**
- Automated build on push
- Runs tests
- Creates NuGet packages
- Auto-publishes on main branch

**ODINMarketFeed.sln**
- Visual Studio solution
- Links all projects
- Organizes solution structure

## Extension Points

The SDK can be extended by:

1. **Custom Event Handlers**
   - OnOpen, OnMessage, OnError, OnClose

2. **Message Parsing**
   - Custom parsers for specific message types

3. **Compression Strategies**
   - Alternative compression algorithms

4. **Connection Management**
   - Custom retry logic
   - Connection pooling

## Future Structure Considerations

Potential additions:

```
├── tests/
│   ├── ODINMarketFeed.SDK.Tests/
│   └── ODINMarketFeed.SDK.IntegrationTests/
│
├── samples/
│   ├── ConsoleApp/
│   ├── AspNetCoreWebApp/
│   └── WindowsServiceApp/
│
└── benchmarks/
    └── ODINMarketFeed.SDK.Benchmarks/
```

## Version Control

**Git Strategy:**
- `main` - stable releases
- `develop` - development branch
- `feature/*` - feature branches
- `bugfix/*` - bug fix branches
- `hotfix/*` - urgent production fixes

**Tags:**
- `v1.0.0` - version releases
- Follow semantic versioning

## Building and Packaging

### Local Development

```bash
# Restore
dotnet restore

# Build
dotnet build

# Run sample
cd samples/ConsoleApp
dotnet run

# Create package
dotnet pack --configuration Release
```

### CI/CD

GitHub Actions automatically:
1. Builds on every push
2. Runs tests
3. Creates packages
4. Publishes to NuGet (on main branch)

## Best Practices Applied

✅ Separation of concerns
✅ Clear project organization
✅ Comprehensive documentation
✅ Multi-framework support
✅ CI/CD ready
✅ Open source friendly
✅ Following .NET conventions
✅ NuGet best practices

---

This structure supports:
- Easy maintenance
- Clear navigation
- Professional presentation
- Community contributions
- Automated workflows
- Multiple deployment targets
