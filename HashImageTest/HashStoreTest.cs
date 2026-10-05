using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using vu.hashedimage;
using Xunit;

namespace HashImageTest;

public class HashStoreTest
{
    [Fact]
    public void DbUnitTest()
    {
        using var context = new HashStore();
        context.Database.EnsureCreated();
    }

    [Fact]
    public void SaveTest()
    {
        var testImage = Environment.CurrentDirectory + "/data/limg.gif";
        var hash = Utils.ImageToMD5(testImage);
        var stringHash = Utils.HastToUtf8(hash);
        var caption = "83084";
        var task = Utils.FileToMemoryAsync(testImage);
        task.Wait();
        var imageData = task.Result.ToArray();
        
        var captcha = new BizarreCaptcha()
        {
            Hash = hash,
            StringHash = stringHash,
            Caption = caption,
            Image = imageData
        };
        
        var store = new HashStore();

 
        store.Save(captcha);;
    }
    
    
}