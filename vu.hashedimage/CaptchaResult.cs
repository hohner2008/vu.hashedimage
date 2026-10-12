using SixLabors.ImageSharp.Formats;

namespace vu.hashedimage;

public struct CaptchaResult
{
    public byte[] data;
    public string? text;
    
    public CaptchaResult(byte[] data,string? text)
    {
        this.data = data;
        this.text = text;
    }
    
    

}