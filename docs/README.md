# Documentation Index

Welcome to the ODIN Market Feed SDK documentation! This directory contains comprehensive guides and references for using the SDK.

## 📚 Documentation Files

### Getting Started

- **[QUICKSTART.md](QUICKSTART.md)** - Get up and running in 5 minutes
  - Basic setup and configuration
  - Simple code examples
  - Common operations
  - Perfect for first-time users

### Reference Documentation

- **[API.md](API.md)** - Complete API Reference
  - All classes and methods
  - Detailed parameters
  - Return values
  - Code examples
  - Best practices
  - Thread safety considerations

### Guides

- **[PUBLISHING.md](PUBLISHING.md)** - Publishing Guide
  - GitHub repository setup
  - NuGet package publishing
  - Version management
  - CI/CD configuration
  - Maintenance guidelines

- **[STRUCTURE.md](STRUCTURE.md)** - Project Structure
  - Directory organization
  - File descriptions
  - Build outputs
  - Extension points
  - Development workflow

- **[ICON.md](ICON.md)** - Package Icon Guide
  - Icon requirements
  - Design suggestions
  - Implementation steps
  - Resources and tools

## 🚀 Quick Links

### For Users

1. **New to the SDK?** → Start with [QUICKSTART.md](QUICKSTART.md)
2. **Need details?** → Check [API.md](API.md)
3. **Having issues?** → Check API.md's [Error Handling](API.md#error-handling) section

### For Developers

1. **Contributing?** → Read [../CONTRIBUTING.md](../CONTRIBUTING.md)
2. **Publishing?** → Follow [PUBLISHING.md](PUBLISHING.md)
3. **Understanding structure?** → See [STRUCTURE.md](STRUCTURE.md)

## 📖 Documentation by Topic

### Connection Management
- [Connecting to Server](API.md#connectasync)
- [Disconnecting](API.md#disconnectasync)
- [Connection Events](API.md#event-handlers)
- [Error Handling](API.md#error-handling)

### Market Data
- [Subscribing to Touchline](API.md#subscribetouchlineasync)
- [Unsubscribing](API.md#unsubscribetouchlineasync)
- [Pause/Resume](API.md#subscribepauseresumeasync)
- [Message Format](API.md#message-format)

### Advanced Topics
- [Compression](API.md#setcompression)
- [Performance Tips](API.md#performance-optimization)
- [Thread Safety](API.md#thread-safety)
- [Best Practices](API.md#best-practices)

## 🎯 Common Scenarios

### "I want to..."

**...connect and receive market data**
→ [QUICKSTART.md](QUICKSTART.md)

**...understand all available methods**
→ [API.md](API.md)

**...handle errors properly**
→ [API.md - Error Handling](API.md#error-handling)

**...optimize performance**
→ [API.md - Performance Tips](API.md#performance-tips)

**...publish my own version**
→ [PUBLISHING.md](PUBLISHING.md)

**...understand the codebase**
→ [STRUCTURE.md](STRUCTURE.md)

**...contribute to the project**
→ [CONTRIBUTING.md](../CONTRIBUTING.md)

## 📋 Checklists

### First-Time Setup Checklist

- [ ] Read [QUICKSTART.md](QUICKSTART.md)
- [ ] Install SDK via NuGet
- [ ] Get credentials (userId, apiKey)
- [ ] Run sample application
- [ ] Test connection
- [ ] Subscribe to test tokens

### Before Going Live Checklist

- [ ] Read [API.md - Best Practices](API.md#best-practices)
- [ ] Implement error handling
- [ ] Add reconnection logic
- [ ] Test with production tokens
- [ ] Monitor performance
- [ ] Set up logging
- [ ] Review security considerations

### Publishing Checklist

- [ ] Update version numbers
- [ ] Update [CHANGELOG.md](../CHANGELOG.md)
- [ ] Run all tests
- [ ] Build release packages
- [ ] Create git tags
- [ ] Publish to NuGet
- [ ] Create GitHub release
- [ ] Update documentation

## 🔍 Search Topics

Common search terms and where to find them:

| Looking for... | Found in... |
|---------------|-------------|
| Installation | [QUICKSTART.md](QUICKSTART.md) |
| Connection parameters | [API.md - ConnectAsync](API.md#connectasync) |
| Event handling | [API.md - Event Handlers](API.md#event-handlers) |
| Token format | [API.md - Token Format](API.md#token-format) |
| Message parsing | [API.md - Message Format](API.md#message-format) |
| Error codes | [API.md - Error Handling](API.md#error-handling) |
| Performance tuning | [API.md - Performance Tips](API.md#performance-tips) |
| Thread safety | [API.md - Thread Safety](API.md#thread-safety) |
| Publishing process | [PUBLISHING.md](PUBLISHING.md) |
| Project structure | [STRUCTURE.md](STRUCTURE.md) |

## 📊 Documentation Statistics

| Document | Purpose | Target Audience |
|----------|---------|-----------------|
| QUICKSTART.md | Get started fast | New users |
| API.md | Complete reference | All users |
| PUBLISHING.md | Publishing guide | Maintainers |
| STRUCTURE.md | Architecture | Developers |
| ICON.md | Design guide | Maintainers |

## 🤝 Contributing to Documentation

Found an issue or want to improve the docs?

1. Check [CONTRIBUTING.md](../CONTRIBUTING.md) for guidelines
2. Submit issues for errors or unclear sections
3. Create pull requests for improvements
4. Suggest new documentation topics

### Documentation Guidelines

When contributing to docs:
- Use clear, simple language
- Include code examples
- Add cross-references
- Keep formatting consistent
- Test all code samples

## 📞 Getting Help

### Can't find what you need?

1. **Search the docs** - Use Ctrl+F in your browser
2. **Check examples** - Look at [samples/](../samples/)
3. **GitHub Issues** - Search existing issues
4. **Create an issue** - Ask your question
5. **Email support** - support@example.com

### Improving the Documentation

See something wrong? Found a typo? Have a suggestion?

- Open an issue: [GitHub Issues](https://github.com/yourusername/ODINMarketFeedSDK/issues)
- Submit a PR: [How to Contribute](../CONTRIBUTING.md)
- Email us: support@example.com

## 📝 Documentation TODO

Future documentation improvements:

- [ ] Add video tutorials
- [ ] Create interactive examples
- [ ] Add troubleshooting flowcharts
- [ ] Include more real-world scenarios
- [ ] Add performance benchmarks
- [ ] Create FAQ section
- [ ] Add multi-language support

## 🔗 External Resources

### .NET Resources
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [WebSocket Documentation](https://docs.microsoft.com/en-us/dotnet/api/system.net.websockets)

### Related Topics
- [NuGet Documentation](https://docs.microsoft.com/en-us/nuget/)
- [Git Documentation](https://git-scm.com/doc)
- [GitHub Actions](https://docs.github.com/en/actions)

---

**Last Updated:** November 2024

**Documentation Version:** 1.0.0

For the main project README, see [../README.md](../README.md)
