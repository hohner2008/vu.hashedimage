using Microsoft.Playwright;
using vu.hashedimage;
using Xunit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;


namespace HashImageTest;

public class LocatorScreenshotTest
{

    [Fact]
    async Task ReturnBytesArrayTest()
    {
        var playwright = await Playwright.CreateAsync();
        var firefox = playwright.Firefox;
        var browser = await firefox.LaunchAsync(new() { Headless = false });
        var page = await browser.NewPageAsync();
        await page.GotoAsync("https://fastly.picsum.photos/id/430/200/300.jpg?hmac=souGSmvwQ6KlJgthGYBGSWB22Y7MpK5xlgLYwvtbXzg");
        var l = page.Locator("img");
        
        var data = await LocatorScreenshot.MakeScreenshot(l);
        var format = Image.DetectFormat(new ReadOnlySpan<byte>(data));
        Assert.Equal("image/png",format.DefaultMimeType);

        Assert.NotNull(data);

        int count =data.Length;
        Assert.True(count>10000);

    }
}