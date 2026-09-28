using System.Drawing;
using Xunit;
using vu.hashedimage;

namespace HashImageTest;

public class DownloadWatcherTest
{
    public string result = "";
    public bool onHold = true;
    
    void TestHadler(object? sender, FileDownloadedEventArgs? arg)
    {
        result = arg?.Path;
        onHold = false;
    }
    [Fact]
    void DownloadEventTest()
    {
        DownloadsWatcher watcher = new DownloadsWatcher();
        watcher.FileName = "limg.gif";
        watcher.FileDownloaded += TestHadler;
        
        string path = Directory.GetCurrentDirectory() + "/data/";
        
        string destDir = @"/home/lusa/Downloads/";
        if (File.Exists(destDir + watcher.FileName))
        {
            File.Delete(destDir + watcher.FileName);
        }
        File.Copy(Path.Combine(path, "limg.gif"), Path.Combine(destDir, "limg.gif"));

        while (onHold == true)
        {
            
        }
        
        Assert.Equal("/tmp/captcha",result);
        
    }
}