using Microsoft.Playwright;

namespace vu.hashedimage;

public static class LocatorScreenshot
{
    private static LocatorScreenshotOptions _options = new LocatorScreenshotOptions();
    
    public static LocatorScreenshotOptions Options
    {
        get => _options;
    }

    public static void MakeScreenshot(ILocator l)
    {
        l.ScreenshotAsync(_options).Wait();
    }
}