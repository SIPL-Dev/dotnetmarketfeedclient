# Getting Started with ODIN Market Feed SDK

Welcome! This guide will help you get your ODIN Market Feed SDK up and running on GitHub and ready for publication.

## 📦 What You Have

A complete, production-ready SDK with:

✅ Multi-framework support (.NET 8.0 & .NET Framework 4.8)  
✅ WebSocket connectivity with compression  
✅ Complete documentation  
✅ Sample application  
✅ CI/CD pipeline (GitHub Actions)  
✅ NuGet package configuration  
✅ Professional project structure  

## 🚀 Quick Setup (5 steps)

### Step 1: Customize the Package

Edit `src/ODINMarketFeed.SDK/ODINMarketFeed.SDK.csproj`:

```xml
<Authors>Your Name</Authors>
<Company>Your Company</Company>
<PackageProjectUrl>https://github.com/YOURUSERNAME/ODINMarketFeedSDK</PackageProjectUrl>
<RepositoryUrl>https://github.com/YOURUSERNAME/ODINMarketFeedSDK</RepositoryUrl>
```

Update `LICENSE`:
```
Copyright (c) 2024 [Your Name or Organization]
```

### Step 2: Test Locally

```bash
# Navigate to the SDK directory
cd path/to/ODINMarketFeedSDK

# Restore dependencies
dotnet restore

# Build the solution
dotnet build --configuration Release

# Run the sample (update credentials first!)
cd samples/ConsoleApp
# Edit Program.cs and add your credentials
dotnet run
```

### Step 3: Push to GitHub

```bash
# Initialize git repository (if not already done)
git init

# Add all files
git add .

# Create initial commit
git commit -m "Initial commit: ODIN Market Feed SDK v1.0.0"

# Create GitHub repository (via web interface)
# Then connect it:
git remote add origin https://github.com/YOURUSERNAME/ODINMarketFeedSDK.git
git branch -M main
git push -u origin main

# Create version tag
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0
```

### Step 4: Build NuGet Package

```bash
# Create the NuGet package
dotnet pack --configuration Release --output ./nupkg

# You'll find: ODINMarketFeed.SDK.1.0.0.nupkg
```

### Step 5: Publish to NuGet (Optional)

```bash
# Get API key from https://www.nuget.org/account/apikeys

# Publish
dotnet nuget push nupkg/ODINMarketFeed.SDK.1.0.0.nupkg \
    --api-key YOUR_NUGET_API_KEY \
    --source https://api.nuget.org/v3/index.json
```

## 📚 Next Steps

### Essential Customizations

1. **Update README.md**
   - Replace `yourusername` with your GitHub username
   - Update contact emails
   - Add actual server details (if public)

2. **Update Sample Credentials**
   - Edit `samples/ConsoleApp/Program.cs`
   - Add real host, userId, apiKey for testing

3. **Add Package Icon** (Recommended)
   - Create 256x256 PNG icon
   - Save as `icon.png` in root directory
   - See [docs/ICON.md](docs/ICON.md) for details

4. **Configure GitHub Actions** (Optional)
   - Add `NUGET_API_KEY` secret in GitHub repository settings
   - Enables automatic publishing on push to main

### Testing

```bash
# Verify build on both frameworks
dotnet build -f net8.0
dotnet build -f net48

# Test the sample application
cd samples/ConsoleApp
dotnet run
```

### Documentation

All documentation is in the `docs/` folder:

- **[docs/QUICKSTART.md](docs/QUICKSTART.md)** - 5-minute user guide
- **[docs/API.md](docs/API.md)** - Complete API reference
- **[docs/PUBLISHING.md](docs/PUBLISHING.md)** - Detailed publishing guide
- **[docs/STRUCTURE.md](docs/STRUCTURE.md)** - Project structure
- **[docs/ICON.md](docs/ICON.md)** - Icon creation guide

## 🎯 Publishing Checklist

Before publishing, ensure:

- [ ] Package metadata updated in .csproj
- [ ] LICENSE copyright holder updated
- [ ] README.md customized
- [ ] Sample application credentials updated (or removed)
- [ ] Tested on .NET 8.0
- [ ] Tested on .NET Framework 4.8 (Windows)
- [ ] All documentation reviewed
- [ ] GitHub repository created
- [ ] Git tags created (v1.0.0)
- [ ] NuGet package built and tested
- [ ] (Optional) Package icon added

## 🛠️ Development Workflow

### Making Changes

```bash
# Create feature branch
git checkout -b feature/my-new-feature

# Make changes
# ... edit files ...

# Test changes
dotnet build
dotnet test  # when tests are added

# Commit changes
git add .
git commit -m "feat: Add new feature"

# Push branch
git push origin feature/my-new-feature

# Create Pull Request on GitHub
```

### Releasing New Version

```bash
# Update version in .csproj
# <Version>1.0.1</Version>

# Update CHANGELOG.md

# Commit version bump
git add .
git commit -m "chore: Bump version to 1.0.1"

# Create tag
git tag -a v1.0.1 -m "Release version 1.0.1"

# Push everything
git push origin main
git push origin v1.0.1

# Build and publish
dotnet pack --configuration Release --output ./nupkg
dotnet nuget push nupkg/ODINMarketFeed.SDK.1.0.1.nupkg \
    --api-key YOUR_API_KEY \
    --source https://api.nuget.org/v3/index.json
```

## 📖 Project Structure

```
ODINMarketFeedSDK/
├── src/                          # Source code
│   └── ODINMarketFeed.SDK/      # Main SDK project
├── samples/                      # Example applications
│   └── ConsoleApp/              # Console sample
├── docs/                         # Documentation
├── .github/workflows/           # CI/CD configuration
├── README.md                     # Main documentation
├── LICENSE                       # MIT License
└── ODINMarketFeed.sln           # Visual Studio solution
```

## 🔧 Requirements

### Development
- Visual Studio 2022 or VS Code
- .NET SDK 8.0 or later
- .NET Framework 4.8 Developer Pack (for .NET 4.8 target)
- Git

### Runtime
- .NET 8.0 or .NET Framework 4.8+
- Windows (for .NET Framework 4.8)
- Windows, Linux, or macOS (for .NET 8.0)

## 🐛 Troubleshooting

### Build Issues

**Error: SDK not found**
```bash
# Install .NET 8.0 SDK
# Download from: https://dotnet.microsoft.com/download/dotnet/8.0
```

**Error: .NET Framework 4.8 not found**
```bash
# Install .NET Framework 4.8 Developer Pack
# Download from: https://dotnet.microsoft.com/download/dotnet-framework/net48
```

### Git Issues

**Error: Permission denied**
```bash
# Set up GitHub authentication
gh auth login
# OR use SSH keys
```

## 📞 Support

- **Documentation**: See [docs/](docs/) folder
- **Issues**: [GitHub Issues](https://github.com/YOURUSERNAME/ODINMarketFeedSDK/issues)
- **Questions**: Create a GitHub Discussion
- **Email**: Update with your support email

## 🎓 Learning Resources

### For Users
- Start with [docs/QUICKSTART.md](docs/QUICKSTART.md)
- Full API reference: [docs/API.md](docs/API.md)
- Check [samples/ConsoleApp/](samples/ConsoleApp/) for examples

### For Developers
- Project structure: [docs/STRUCTURE.md](docs/STRUCTURE.md)
- Publishing guide: [docs/PUBLISHING.md](docs/PUBLISHING.md)
- Contributing: [CONTRIBUTING.md](CONTRIBUTING.md)

### External Resources
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [NuGet Documentation](https://docs.microsoft.com/en-us/nuget/)
- [GitHub Actions](https://docs.github.com/en/actions)

## ✅ Success Checklist

You're ready when:

- [ ] SDK builds successfully (`dotnet build`)
- [ ] Sample runs with real credentials
- [ ] Code pushed to GitHub
- [ ] Repository properly configured
- [ ] Documentation reviewed and customized
- [ ] NuGet package created successfully
- [ ] (Optional) Published to NuGet.org

## 🚢 Ready to Ship?

Your SDK is production-ready! Follow the detailed guide in [docs/PUBLISHING.md](docs/PUBLISHING.md) for step-by-step publishing instructions.

---

**Need Help?** Check the [docs/](docs/) folder or create an issue on GitHub.

**Ready to Use?** Users can start with [docs/QUICKSTART.md](docs/QUICKSTART.md).

Happy Coding! 🎉
