using Xunit;
using static vu.hashedimage.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
    
namespace HashImageTest;

public class UtilsTest
{
    [Fact]
    public static void ImageToMD5Test()
    {
        string path = Directory.GetCurrentDirectory() + "/data/";
        var p = path + "limg.gif";
        
        byte[] hash = ImageToMD5(p);
    }
}