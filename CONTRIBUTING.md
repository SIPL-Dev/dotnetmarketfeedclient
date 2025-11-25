# Contributing to ODIN Market Feed SDK

Thank you for your interest in contributing to the ODIN Market Feed SDK! This document provides guidelines and instructions for contributing.

## Code of Conduct

By participating in this project, you agree to maintain a respectful and inclusive environment for all contributors.

## How to Contribute

### Reporting Bugs

Before creating a bug report, please check the existing issues to avoid duplicates. When creating a bug report, include:

- **Clear title and description**
- **Steps to reproduce** the behavior
- **Expected behavior**
- **Actual behavior**
- **Environment details** (.NET version, OS, etc.)
- **Code samples** if applicable
- **Error messages** and stack traces

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, include:

- **Clear title and description**
- **Use case** explaining why this enhancement would be useful
- **Proposed implementation** if you have ideas
- **Examples** of how it would work

### Pull Requests

1. **Fork the repository** and create your branch from `main`
2. **Follow coding standards** (see below)
3. **Add tests** if applicable
4. **Update documentation** as needed
5. **Ensure tests pass** locally
6. **Create a pull request** with a clear description

#### Branch Naming Convention

- `feature/` - New features
- `bugfix/` - Bug fixes
- `hotfix/` - Urgent fixes
- `docs/` - Documentation updates
- `refactor/` - Code refactoring

Example: `feature/add-market-depth-support`

## Development Setup

### Prerequisites

- Visual Studio 2022 or later (or VS Code with C# extension)
- .NET 8.0 SDK
- .NET Framework 4.8 Developer Pack
- Git

### Building the Project

```bash
# Clone the repository
git clone https://github.com/yourusername/ODINMarketFeedSDK.git
cd ODINMarketFeedSDK

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests (when available)
dotnet test

# Build NuGet package
dotnet pack --configuration Release
```

### Running the Sample

```bash
cd samples/ConsoleApp
dotnet run
```

## Coding Standards

### C# Style Guide

- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and concise
- Use async/await for asynchronous operations

### Code Formatting

- Use 4 spaces for indentation (no tabs)
- Place opening braces on new lines
- Use `var` when the type is obvious
- Add blank lines between method definitions
- Organize using statements alphabetically

### Example

```csharp
/// <summary>
/// Subscribes to market data for the specified tokens.
/// </summary>
/// <param name="tokenList">List of tokens to subscribe</param>
/// <param name="responseType">Type of response (0 or 1)</param>
/// <returns>Task representing the async operation</returns>
public async Task SubscribeAsync(IEnumerable<string> tokenList, string responseType = "0")
{
    if (tokenList == null || !tokenList.Any())
    {
        throw new ArgumentException("Token list cannot be null or empty", nameof(tokenList));
    }

    // Implementation
    await SendSubscriptionRequestAsync(tokenList, responseType);
}
```

## Testing

- Write unit tests for new features
- Ensure all tests pass before submitting PR
- Aim for meaningful test coverage
- Use descriptive test names

```csharp
[Fact]
public async Task SubscribeAsync_WithValidTokens_ShouldSucceed()
{
    // Arrange
    var client = new ODINMarketFeedClient();
    var tokens = new List<string> { "1_2885" };

    // Act & Assert
    await client.SubscribeAsync(tokens);
}
```

## Documentation

- Update README.md for user-facing changes
- Update XML documentation comments for API changes
- Add code examples where appropriate
- Update CHANGELOG.md following [Keep a Changelog](https://keepachangelog.com/) format

## Commit Messages

Write clear and meaningful commit messages:

```
feat: Add market depth subscription support

- Implement SubscribeMarketDepthAsync method
- Add market depth response parsing
- Update documentation with examples
```

### Commit Message Format

```
<type>: <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation only
- `style`: Code style changes (formatting, etc.)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

## Review Process

1. **Automated checks** must pass (build, tests)
2. **Code review** by at least one maintainer
3. **Address feedback** from reviewers
4. **Merge** when approved

## Getting Help

- 💬 **GitHub Discussions**: For questions and discussions
- 🐛 **GitHub Issues**: For bugs and feature requests
- 📧 **Email**: support@example.com for private inquiries

## Recognition

Contributors will be recognized in:
- README.md acknowledgments section
- Release notes for significant contributions
- Git commit history

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to ODIN Market Feed SDK! 🚀
