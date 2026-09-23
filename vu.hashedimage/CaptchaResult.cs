using SixLabors.ImageSharp.Formats;

namespace vu.hashedimage;

public struct CaptchaResult
{
    public byte[] data;
    public string? url;
    public IImageFormat format;
    public string? text;
    
    public CaptchaResult(byte[] data,string? url, IImageFormat format,string? text)
    {
        this.data = data;
        this.url = url;
        this.format = format;
        this.text = text;
    }
    
    

}