# Publishing Guide for ODIN Market Feed SDK

This guide walks you through the process of publishing the ODIN Market Feed SDK to GitHub and NuGet.

## Prerequisites

- Git installed
- GitHub account
- .NET SDK 8.0 or later
- NuGet account (for publishing to NuGet.org)
- Visual Studio 2022 or VS Code (optional but recommended)

## Step 1: Prepare the Repository

### 1.1 Update Package Information

Edit `src/ODINMarketFeed.SDK/ODINMarketFeed.SDK.csproj` and update:

```xml
<Authors>Your Name</Authors>
<Company>Your Company</Company>
<PackageProjectUrl>https://github.com/yourusername/ODINMarketFeedSDK</PackageProjectUrl>
<RepositoryUrl>https://github.com/yourusername/ODINMarketFeedSDK</RepositoryUrl>
```

### 1.2 Update README

Update the following in `README.md`:
- Replace `yourusername` with your GitHub username
- Update support email addresses
- Add actual market feed server details (if public)
- Update any company-specific information

### 1.3 Review License

Ensure `LICENSE` file has the correct copyright holder:
```
Copyright (c) 2024 [Your Name or Organization]
```

## Step 2: Initialize Git Repository

```bash
cd ODINMarketFeedSDK

# Initialize git repository
git init

# Add all files
git add .

# Create initial commit
git commit -m "Initial commit: ODIN Market Feed SDK v1.0.0"
```

## Step 3: Create GitHub Repository

### 3.1 Create Repository on GitHub

1. Go to https://github.com/new
2. Repository name: `ODINMarketFeedSDK`
3. Description: "ODIN Market Feed Client SDK - Real-time market data streaming for .NET"
4. Choose Public or Private
5. **Do NOT** initialize with README (we already have one)
6. Click "Create repository"

### 3.2 Push to GitHub

```bash
# Add remote
git remote add origin https://github.com/yourusername/ODINMarketFeedSDK.git

# Push to GitHub
git branch -M main
git push -u origin main
```

### 3.3 Create Tags

```bash
# Create a version tag
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0
```

## Step 4: Configure GitHub Repository

### 4.1 Add Repository Topics

In your GitHub repository:
1. Click "About" gear icon
2. Add topics: `dotnet`, `csharp`, `websocket`, `market-data`, `trading`, `sdk`, `nuget`

### 4.2 Create GitHub Release

1. Go to "Releases" → "Create a new release"
2. Choose tag: `v1.0.0`
3. Release title: `v1.0.0 - Initial Release`
4. Description: Copy content from CHANGELOG.md
5. Click "Publish release"

### 4.3 Enable GitHub Actions (Optional)

The workflow is already configured in `.github/workflows/dotnet.yml`.

To enable NuGet publishing via GitHub Actions:
1. Go to repository Settings → Secrets and variables → Actions
2. Add secret: `NUGET_API_KEY` with your NuGet API key

## Step 5: Build and Test Locally

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build --configuration Release

# Run tests (if you have tests)
dotnet test --configuration Release

# Create NuGet package
dotnet pack --configuration Release --output ./nupkg
```

The package will be created in `./nupkg/ODINMarketFeed.SDK.1.0.0.nupkg`

## Step 6: Publish to NuGet.org

### 6.1 Create NuGet Account

1. Go to https://www.nuget.org
2. Sign in or create an account
3. Go to https://www.nuget.org/account/apikeys
4. Create an API key with "Push" permissions

### 6.2 Publish via Command Line

```bash
# Navigate to package directory
cd nupkg

# Publish to NuGet
dotnet nuget push ODINMarketFeed.SDK.1.0.0.nupkg \
    --api-key YOUR_NUGET_API_KEY \
    --source https://api.nuget.org/v3/index.json
```

### 6.3 Publish via NuGet.org UI

1. Go to https://www.nuget.org/packages/manage/upload
2. Upload the `.nupkg` file
3. Click "Submit"

### 6.4 Verify Publication

- Check https://www.nuget.org/packages/ODINMarketFeed.SDK
- It may take a few minutes to appear in search results

## Step 7: Update Documentation

### 7.1 Add NuGet Badge to README

Update README.md with actual NuGet version:

```markdown
[![NuGet](https://img.shields.io/nuget/v/ODINMarketFeed.SDK.svg)](https://www.nuget.org/packages/ODINMarketFeed.SDK)
```

### 7.2 Update GitHub Repository Description

Set repository description and website:
- Description: "ODIN Market Feed Client SDK - Real-time market data streaming for .NET"
- Website: https://www.nuget.org/packages/ODINMarketFeed.SDK

## Step 8: Post-Publication Tasks

### 8.1 Announce Release

- Create a blog post (if applicable)
- Share on social media
- Notify stakeholders
- Update company documentation

### 8.2 Monitor Package

- Watch for issues on GitHub
- Monitor download statistics on NuGet
- Respond to community feedback

## Updating the Package

### For Bug Fixes (Patch Version)

```bash
# Update version in .csproj
# Change: <Version>1.0.0</Version>
# To:     <Version>1.0.1</Version>

# Build and pack
dotnet pack --configuration Release --output ./nupkg

# Create git tag
git tag -a v1.0.1 -m "Release version 1.0.1"
git push origin v1.0.1

# Publish to NuGet
dotnet nuget push nupkg/ODINMarketFeed.SDK.1.0.1.nupkg \
    --api-key YOUR_NUGET_API_KEY \
    --source https://api.nuget.org/v3/index.json
```

### For New Features (Minor Version)

```bash
# Update version to 1.1.0
# Follow same steps as above
```

### For Breaking Changes (Major Version)

```bash
# Update version to 2.0.0
# Update CHANGELOG.md with breaking changes
# Follow same steps as above
```

## Continuous Integration/Deployment

The included GitHub Actions workflow automatically:
- Builds the project on every push
- Runs tests
- Creates NuGet packages
- Publishes to NuGet.org (on main branch, if API key is configured)

To enable:
1. Add `NUGET_API_KEY` secret to GitHub repository
2. Push to main branch
3. Workflow will automatically publish

## Troubleshooting

### Build Errors

```bash
# Clean build artifacts
dotnet clean
rm -rf bin obj

# Restore and rebuild
dotnet restore
dotnet build
```

### NuGet Push Errors

**Error: Package already exists**
- You cannot replace an existing version
- Increment version number and try again

**Error: Invalid API key**
- Verify API key is correct
- Check API key permissions (needs Push permission)
- Regenerate API key if needed

**Error: Package validation failed**
- Ensure all required metadata is present
- Check package size limits
- Review NuGet package requirements

### GitHub Actions Errors

Check `.github/workflows/dotnet.yml` and ensure:
- Workflow file syntax is correct
- Secrets are properly configured
- Build targets are correct

## Best Practices

1. **Version Management**
   - Follow Semantic Versioning (SemVer)
   - Update CHANGELOG.md for every release
   - Create git tags for all releases

2. **Testing**
   - Test locally before publishing
   - Verify on multiple .NET versions
   - Test in both Windows and Linux (if applicable)

3. **Documentation**
   - Keep README.md up to date
   - Update API documentation
   - Provide migration guides for breaking changes

4. **Communication**
   - Announce releases clearly
   - Document breaking changes
   - Respond to issues promptly

## Security Considerations

1. **API Keys**
   - Never commit API keys to repository
   - Use GitHub Secrets for CI/CD
   - Rotate keys periodically

2. **Code Signing**
   - Consider signing assemblies for production
   - Use strong name signing if required

3. **Vulnerability Scanning**
   - Use tools like Dependabot
   - Keep dependencies updated
   - Monitor security advisories

## Support and Maintenance

### Regular Maintenance Tasks

- [ ] Monitor GitHub issues
- [ ] Review pull requests
- [ ] Update dependencies
- [ ] Test with new .NET versions
- [ ] Update documentation
- [ ] Respond to community feedback

### Long-term Support

Consider establishing:
- LTS (Long Term Support) versions
- Support channels (email, Discord, Slack)
- Release schedule
- Deprecation policy

## Additional Resources

- [NuGet Documentation](https://docs.microsoft.com/en-us/nuget/)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Semantic Versioning](https://semver.org/)
- [Keep a Changelog](https://keepachangelog.com/)

---

**Congratulations!** Your SDK is now published and ready for the community to use.

For questions or issues with this guide, please open an issue on GitHub.
