using Microsoft.Playwright;
using vu.core;


String MainCaptchaGathering(IPage page)
{
    var locators = page.Locator("input.l").AllAsync();
    var data = locators.Result;
    data[0].FillAsync("wttest").Wait();
    data[0].PressAsync("Tab").Wait();
    data[1].FocusAsync().Wait();
    var t2 = data[1].FillAsync("9020");
    var locators2 = page.Locator("[type='submit']").AllAsync();
    locators2.Wait();
    var data2 = locators2.Result;
    data2[0].ClickAsync().Wait();
    var url = page.Url;
    var txts = page.GetByText("пароль не підходить").AllAsync().Result;
    var achtung = page.GetByText("ПОПЕРЕДЖЕННЯ").AllAsync().Result;
    
    var checkbox = page.Locator("[type='checkbox']").AllAsync().Result;
    if (checkbox.Count == 1)
    {
        checkbox[0].ClickAsync().Wait();
    }
    
    var okButton = page.Locator("input.l2").AllAsync().Result;
    
    okButton[0].ClickAsync().Wait();
    
    var imgLocator = page.Locator("img").AllAsync().Result;
    string? attributeValue = imgLocator[0].GetAttributeAsync("src").Result;
 
    string? url_base = new Uri(page.Url).Host;
    string? image_url =  "https://" + url_base + attributeValue;


    string script = @"
        let row = document.createElement(""tr"");
        let buttonCell1 = document.createElement(""td"");
        let buttonCell2 = document.createElement(""td"");

        let btn = document.createElement('button');
        btn.innerHTML = 'GRAB';
        btn.onclick = function() {
            alert('Button clicked!');
        };

       let btn2 = document.createElement('button');
        btn2.innerHTML = 'RESTART';
        btn2.onclick = function() {
            alert('Button clicked!');
        };
        buttonCell1.appendChild(btn);
        buttonCell2.appendChild(btn2);
        row.appendChild(buttonCell1);
        row.appendChild(buttonCell2);

        document.body.appendChild(row);
    ";
    page.EvaluateAsync(script).Wait();
    
    var toChat = page.Locator("input.l[value='В ЧАТ!']").AllAsync().Result;
    toChat[0].EvaluateAsync("el => el.remove()").Wait();


    if (!string.IsNullOrEmpty(image_url))
    {
        // Если ссылка относительная, дополните её базовым URL страницы
        var absoluteUrl = new Uri(new Uri(page.Url), image_url).AbsoluteUri;
    
        using var client = new HttpClient();
        byte[] imageBytes = client.GetByteArrayAsync(absoluteUrl).Result;
        File.WriteAllBytesAsync("/home/lusa/image.gif", imageBytes).Wait();
    }

    

    return "test";
}

Console.WriteLine("Initializing Playwright dependencies...");

// 1. Programmatically install the necessary browser binaries (e.g., Chromium)
// This replaces the manual "pwsh bin/Debug/net8.0/playwright.ps1 install" step.
// It skips downloading if the binaries are already present.
int exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "firefox" });
        
if (exitCode != 0)
{
    Console.WriteLine($"Failed to install browser binaries. Exit code: {exitCode}");
    return;
}

Console.WriteLine("Browsers successfully checked/installed.");

//exitCode = Microsoft.Playwright.Program.Main(new[] { "install-deps"});


var url = "https://bizarre.kiev.ua/";
var crawler = new BaseCrawler(new Uri(url));
crawler.SetMainFunction(MainCaptchaGathering);
await crawler.RunBrowser(BaseCrawler.BrowserName.Firefox);
Console.ReadLine();