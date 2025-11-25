# Package Icon

To add a professional icon to your NuGet package:

## Create the Icon

1. **Design Requirements:**
   - Size: 128x128 pixels (minimum), 256x256 pixels (recommended)
   - Format: PNG with transparency
   - File name: `icon.png`
   - Simple, recognizable design
   - Works well at small sizes

2. **Design Suggestions:**
   - Use your company logo
   - Create a chart/graph icon representing market data
   - Use colors that represent finance/trading (green, blue)
   - Keep it simple and professional

## Option 1: Use an Online Icon Creator

1. Go to https://favicon.io/favicon-generator/
2. Create a simple icon with text "ODIN" or "MF"
3. Download and resize to 256x256
4. Save as `icon.png`

## Option 2: Use Figma/Canva

1. Create a 256x256 canvas
2. Design your icon
3. Export as PNG with transparency
4. Save to repository root as `icon.png`

## Option 3: Hire a Designer

For a professional look, consider hiring a designer from:
- Fiverr
- Upwork
- 99designs

## Adding Icon to Package

Once you have `icon.png`:

```bash
# Place icon in repository root
/ODINMarketFeedSDK/
  ├── icon.png          <-- Place here
  ├── README.md
  └── ...
```

The icon is already configured in the `.csproj` file:

```xml
<PackageIcon>icon.png</PackageIcon>
<None Include="..\..\icon.png" Pack="true" PackagePath="\" />
```

## Placeholder Icon

For now, you can create a simple placeholder:

### Using ImageMagick (command line):
```bash
convert -size 256x256 xc:blue -fill white -gravity center \
  -pointsize 72 -annotate +0+0 'ODIN' icon.png
```

### Using Paint.NET, GIMP, or Photoshop:
1. Create new 256x256 image
2. Fill with a solid color (e.g., #0066CC - blue)
3. Add white text "ODIN" or "MF" in center
4. Save as PNG

## Example Icon Concepts

### Concept 1: Text-based
- Background: Gradient blue (#0066CC to #00AAFF)
- Text: "ODIN" in bold white font
- Border: Subtle white border

### Concept 2: Graph Icon
- Simple line chart icon
- Upward trending line
- Corporate blue color scheme

### Concept 3: Ticker Symbol
- Stock ticker style
- Arrow pointing up
- Market data theme

## Testing the Icon

After adding the icon:

```bash
# Pack the NuGet package
dotnet pack --configuration Release

# Check the .nupkg file contains the icon
# Extract the .nupkg (it's just a ZIP file) and verify icon.png is included
```

The icon will appear:
- On NuGet.org package page
- In Visual Studio NuGet Package Manager
- In package search results

## Icon Best Practices

✅ **DO:**
- Use vector graphics when possible
- Test at different sizes
- Use high contrast
- Keep it simple
- Make it recognizable at 32x32

❌ **DON'T:**
- Use too much detail
- Use thin lines that disappear when small
- Use low contrast colors
- Use copyrighted images without permission
- Use photographs (they don't scale well)

## Resources

Free icon resources:
- https://www.flaticon.com (ensure commercial license)
- https://iconmonstr.com (free for commercial use)
- https://heroicons.com (free, open source)
- https://fontawesome.com (free icons available)

**Note:** Always check licensing before using any icon!

---

**For now:** The package will work without an icon, but adding one makes it more professional and recognizable.
