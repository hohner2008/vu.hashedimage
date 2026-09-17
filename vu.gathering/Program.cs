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
        let grab_clicked = false;
        let restart_clicked = false;

        let informer = document.createElement('i');
        informer.id = 'informer';
        
        let row = document.createElement('tr');
        let buttonCell1 = document.createElement('td');
        let buttonCell2 = document.createElement('td');

        let btn = document.createElement('button');
        btn.innerHTML = 'GRAB';
        btn.onclick = function() {
            grab_clicked = true;
            informer.innerText = 'grab_clicked';
            row.appendChild(informer);
            condition = false;
            console.log(grab_clicked);
        };

       let btn2 = document.createElement('button');
        btn2.innerHTML = 'RESTART';
        btn2.onclick = function() {
            restart_clicked = true;
            informer.innerText = 'restart_clicked';
            row.appendChild(informer);
            condition = false;
            console.log(restart_clicked);
        };
        buttonCell1.appendChild(btn);
        buttonCell2.appendChild(btn2);
        row.appendChild(buttonCell1);
        row.appendChild(buttonCell2);

        const tbody = document.querySelector('body');
        // Insert the new row at the very top of the tbody
        tbody.insertBefore(row, tbody.firstChild);

       
    ";
       
        page.EvaluateAsync(script).Wait();
        var toChat = page.Locator("input.l[value='В ЧАТ!']").AllAsync().Result;
        toChat[0].EvaluateAsync("el => el.remove()").Wait();
    
    var buttons = page.Locator("button").AllAsync().Result;
    ILocator grab,restart;
    foreach (var button in buttons)
    {
        string str = button.InnerTextAsync().Result;
        if (str == "GRAB") grab = button;
        else if (str == "RESTART") restart = button;
    }
    
    
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