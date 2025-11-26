# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0] - 2024-11-24

### Added
- Initial release of ODIN Market Feed SDK
- WebSocket connectivity with SSL support
- ZLIB compression support for efficient data transfer
- Message fragmentation and defragmentation handling
- Touchline and BestFive subscription support
- Pause/Resume functionality for data broadcasts
- Multi-framework support (.NET 8.0 and .NET Framework 4.8)
- Event-driven architecture with OnOpen, OnMessage, OnError, OnClose events
- Comprehensive documentation and samples
- Console sample application

### Security
- Input validation for all connection parameters
- Secure WebSocket (WSS) support
- API key authentication

[Unreleased]: https://github.com/SIPL-Dev/ODINMarketFeedSDK/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/SIPL-Dev/ODINMarketFeedSDK/releases/tag/v1.0.0
