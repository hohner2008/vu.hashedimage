using Microsoft.Playwright;

Console.WriteLine("Hello, World!");

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


    if (!string.IsNullOrEmpty(image_url))
    {
        // Если ссылка относительная, дополните её базовым URL страницы
        var absoluteUrl = new Uri(new Uri(page.Url), image_url).AbsoluteUri;
    
        using var client = new HttpClient();
        byte[] imageBytes = client.GetByteArrayAsync(absoluteUrl).Result;
        File.WriteAllBytesAsync("/home/lusa/image.gif", imageBytes).Wait();
    }

    
    
    var password = page.Locator("[type=\"password\"]").AllAsync().Result;
    password[0].FillAsync("9020test").Wait();

    var mail = page.Locator("[name=\"email\"]").AllAsync().Result;
    mail[0].FocusAsync().Wait();
    mail[0].FillAsync("test@test.com").Wait();
    
    var no_email = page.Locator("[name=\"noemail\"]").AllAsync().Result;
    no_email[0].ClickAsync().Wait();
    
    var toChat = page.Locator("[name=\"OK\"]").AllAsync().Result;
    toChat[0].ClickAsync().Wait();

    return "test";
}
