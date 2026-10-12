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

 
        //store.Save(captcha);

        var obj =store.FindByHash(stringHash);

        obj.Caption = "11111";
        
        store.UpdateEntity(obj);

        //store.Delete(obj);
        var i = 0;
        for (i = 0; i < 10; i++)
        {
            captcha.Id += 100;
            captcha.Caption = captcha.Caption + i.ToString();
            captcha.StringHash = captcha.StringHash + i.ToString() +"100";
            store.Save(captcha);
        }

        List<BizarreCaptcha> list = store.GetAllRecords();
    }
    
    
}