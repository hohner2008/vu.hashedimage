using System.Drawing;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Playwright;
using SixLabors.ImageSharp.Formats;
using vu.core;
using vu.gathering;
using vu.hashedimage;
using Image = SixLabors.ImageSharp.Image;


ILocator FindInformer(IPage page, string query)
{
    ILocator informer;
    informer = page.Locator(query);
    if ( informer is null ) FindInformer(page, query);
    return informer;
}

CaptchaResult LoadCaptcha(IPage page)
{
    var imgLocator = page.Locator("img").AllAsync().Result;
    string? attributeValue = imgLocator[0].GetAttributeAsync("src").Result;
    
    imgLocator[0].ScreenshotAsync(new() { Path = "element.png" }).Wait();
    
    string? url = "https://" + new Uri(page.Url).Host +  attributeValue ;
    byte[] data;
    IImageFormat format;
    
    if (!string.IsNullOrEmpty(url))
    {
        var user = Environment.UserName;
        var folder = Utils.GetDownloadFolder();
        Image savedCaptcha = Image.Load("/home/lusa/limg.gif");
        var imageFormat = Image.DetectFormat("/home/lusa/limg.gif");
        using (var httpClient = new HttpClient())
        {
            //Issue the GET request to a URL and read the response into a 
            //stream that can be used to load the image
            var absoluteUrl = new Uri(new Uri(page.Url),url);
            var t = httpClient.GetStreamAsync(absoluteUrl).WaitAsync(new CancellationToken(false));
            var stream = t.Result;
            Memory<byte> memory = new Memory<byte>(new byte[10000]);
            ValueTask<int> res = stream.ReadAsync(memory);
            var imageContent  = memory.ToArray();
            File.WriteAllBytesAsync("/home/lusa/image.gif", imageContent).Wait();
        }
        var task = Image.LoadAsync(url); 
        task.Wait();
        Image image = task.Result;
        using var client = new HttpClient();
        data = client.GetByteArrayAsync(url).WaitAsync(new TimeSpan(1000000000)).Result;
        format = Image.DetectFormat(new ReadOnlySpan<byte>(data));
    }
    else
    {
        // change to another exception
        throw new NoCaptchaException("Need a valid captcha to input!!!");
    }

    string? text = "";
    return new CaptchaResult(data,url,format,text);
}

void SaveCapthchaInDatabase(IPage page)
{
    var captchaInput = page.Locator("input.l[type='text'][name='tmp']").AllAsync().Result;
    if (captchaInput is not null && captchaInput.Count == 1)
    {
        var s = captchaInput[0].InputValueAsync().Result;
        if (s == String.Empty)
        {
            throw new NoCaptchaException("Need a valid captcha to input!!!");
        }
        else
        {
            LoadCaptcha(page);
        }
    }
    else
    {
        throw new NoCaptchaException("Need a valid captcha to input!!!");
    }
}

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

        ILocator informer = null;
        Task waitForLocator = new Task(() =>
        {
            informer = FindInformer(page, "i#informer");
        });
        waitForLocator.RunSynchronously();
        if (informer is null)
        {
            Console.WriteLine(informer.InnerTextAsync().Result);
        }
        else
        {
            var inner = informer.InnerTextAsync().Result;
            if (inner == "grab_clicked")
            {
                Console.WriteLine(inner);
                SaveCapthchaInDatabase(page);
            }
        }
    
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