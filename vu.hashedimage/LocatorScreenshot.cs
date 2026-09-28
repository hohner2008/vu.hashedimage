using Microsoft.Playwright;

namespace vu.hashedimage;

public static class LocatorScreenshot
{
    private static LocatorScreenshotOptions _options;
    
    public static LocatorScreenshotOptions Options
    {
        get => _options;
    }
    static LocatorScreenshot()
    {
        _options = new LocatorScreenshotOptions();
    }

    public static async Task<byte[]> MakeScreenshot(ILocator? l)
    {
        
        LocatorScreenshot.Options.Path = "screenshot.png"; 
        return await l?.ScreenshotAsync(_options);
        
    }
}