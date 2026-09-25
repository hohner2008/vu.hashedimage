using Microsoft.Playwright;

namespace vu.hashedimage;

public static class LocatorScreenshot
{
    private static LocatorScreenshotOptions _options = new LocatorScreenshotOptions();
    
    public static LocatorScreenshotOptions Options
    {
        get => _options;
    }

    public static byte[] MakeScreenshot(ILocator l)
    {
        var task = l.ScreenshotAsync(_options).WaitAsync(new CancellationToken(false));
        task.Wait();
        return task.Result;
    }
}